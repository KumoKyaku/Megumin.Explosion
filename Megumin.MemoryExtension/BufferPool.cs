using Megumin;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Buffers
{
    /// <summary>
    /// 与MemoryPool不同，这里保证取出的Memory长度和申请的长度相同，不会比申请的更长。
    /// <para></para>
    /// 与MemoryPool不同，内存申请时已经进行了清零处理，请放心使用。
    /// </summary>
    public static class BufferPool
    {
        static readonly ByteMemoryPool pool = new ByteMemoryPool();

        /// <summary>
        /// 从托管内存取得Memory
        /// </summary>
        /// <param name="minBufferSize"></param>
        /// <returns></returns>
        public static IMemoryOwner<byte> Rent(int minBufferSize = 0) => pool.Rent(minBufferSize);
        /// <summary>
        /// 从非托管内存取得Memory
        /// <para>**注意：堆外内存无法取出数组。MemoryMarshal.TryGetArray对此类无效。**</para>
        /// </summary>
        /// <param name="minBufferSize"></param>
        /// <returns></returns>
        public static IMemoryOwner<byte> NativeRent(int minBufferSize = 0)
            => new UnmanagedMemoryManager<byte>(minBufferSize);
    }

    /// <summary>
    /// 与内置的池不同，这里保证取出的Memory长度和申请的长度相同，不会比申请的更长。
    /// </summary>
    internal class ByteMemoryPool : MemoryPool<byte>
    {
        internal ByteMemoryPool() { }

        protected override void Dispose(bool disposing)
        {
            base.Dispose();
        }

        public override IMemoryOwner<byte> Rent(int minBufferSize) => new ByteOwner(minBufferSize);

        public override int MaxBufferSize => int.MaxValue;

        class ByteOwner : IMemoryOwner<byte>,IRealLength
        {
            private byte[] array;
            private Memory<byte> _memory;
            private readonly object innerlock = new object();

            public Memory<byte> Memory
            {
                get
                {
                    lock (innerlock)
                    {
                        CheckDispose();
                        return _memory;
                    }
                }
            }

            void CheckDispose()
            {
                if (array == null)
                {
                    throw new ObjectDisposedException(nameof(IMemoryOwner<byte>));
                }
            }

            public ByteOwner(int mininumLength)
            {
                array = ByteArrayPool.ForMemory.Rent(mininumLength);
                _memory = new Memory<byte>(array, 0, mininumLength);
            }

            public void Dispose()
            {
                lock (innerlock)
                {
                    _memory = null;
                    if (array != null)
                    {
                        array = null;
                        ByteArrayPool.ForMemory.Return(array);
                    }
                }
            }

            public int RealLength
            {
                get
                {
                    lock (innerlock)
                    {
                        CheckDispose();
                        return array.Length;
                    }
                }
            }

            //public void ResizeVisualLength(int newStartPosition, int newEndPosition)
            //{
            //    lock (innerlock)
            //    {
            //        CheckDispose();
            //        var newlength = newEndPosition - newStartPosition;
            //        if (newlength < 0)
            //        {
            //            throw new ArgumentOutOfRangeException();
            //        }
            //        else if (newlength == 0)
            //        {
            //            if (_memory.Length != 0)
            //            {
            //                _memory = new Memory<byte>(array, 0, 0);
            //            }
            //        }
            //        else
            //        {
            //            if (newlength > array.Length)
            //            {
            //                var newArray =  ByteArrayPool.ForMemory.Rent(newlength);
            //                if (newStartPosition == 0)
            //                {
            //                    _memory.Span.CopyTo(newArray);
            //                }
            //                else if (newStartPosition > 0)
            //                {
            //                    _memory.Span.Slice(newStartPosition).CopyTo(newArray);
            //                }
            //                else
            //                {
            //                    //todo
            //                }


            //                _memory = new Memory<byte>(newArray, 0, newlength);
            //                ByteArrayPool.ForMemory.Return(array);
            //                array = newArray;
            //                return;
            //            }
            //            else
            //            {
            //                //todo
            //            }
            //        }
            //    }
            //}
        }

    }

    /// <summary>
    /// 真实长度
    /// </summary>
    public interface IRealLength
    {
        /// <summary>
        /// 真实长度
        /// </summary>
        int RealLength { get; }
    }

    /// <summary>
    /// 可调整大小的
    /// </summary>
    [Obsolete("",true)]
    public interface IResizable:IRealLength
    {
        /// <summary>
        /// 重设可见长度，当真实长度不够时自动扩容，数据不变
        /// <para></para>
        /// 新的可见长度为<paramref name="newEndPosition"/> - <paramref name="newStartPosition"/>
        /// <para>当起始位置是负数时，在头部插入 | <paramref name="newStartPosition"/> |  的长度。</para>
        /// </summary>
        /// <param name="newStartPosition"></param>
        /// <param name="newEndPosition"></param>
        void ResizeVisualLength(int newStartPosition, int newEndPosition);
    }

    /// <summary>
    /// 网络协议栈缓冲区
    /// <para>用于组装协议栈中动态控制缓冲区大小</para>
    /// </summary>
    [Obsolete("",true)]
    public interface INetstackBuffer:IResizable,IMemoryOwner<byte>
    {
        ///为什么废弃可调整大小功能？
        ///
        ///写到一半时发现，
        ///
        ///如果可见位置变小，那么不需要拷贝数据，但是第二次将可见位置变大时，
        ///新的可见位置将会有未知的数据而不是0，这决定了调整可见大小必须拷贝数据。
        ///那么在内部调整大小不如调用者重新租赁新的缓冲区。
        ///
        ///逻辑就是，如果你想使用大的buffer,那么就必须保存有效数据未知信息index + length
        ///如果你想使用memory.length 表示有效信息位置，那么在扩容时就必须数据拷贝。
        ///
        ///需要考虑的是，并不只是在尾部扩容，也可在头部扩容。
        ///指定一个额外的memrory字段来表示游戏数据不能解决问题。

        ///所以 https://github.com/skywind3000/kcp/wiki/Network-Layer 无法避免内存拷贝。

    }
}
