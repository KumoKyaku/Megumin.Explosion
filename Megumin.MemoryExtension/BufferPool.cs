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

        class ByteOwner : IMemoryOwner<byte>
        {
            private byte[] array;
            private Memory<byte> _memory;

            public Memory<byte> Memory
            {
                get
                {
                    if (_memory.IsEmpty)
                    {
                        throw new ObjectDisposedException(nameof(IMemoryOwner<byte>));
                    }
                    return _memory;
                }
            }

            public ByteOwner(int mininumLength)
            {
                this.array = ByteArrayPool.ForMemory.Rent(mininumLength);
                if (mininumLength <= 0)
                {
                    _memory = Memory<byte>.Empty;
                }
                else
                {
                    _memory = new Memory<byte>(array, 0, mininumLength);
                    _memory.Span.Clear();
                }
            }

            public void Dispose()
            {
                _memory = Memory<byte>.Empty;
                if (array != null)
                {
                    ByteArrayPool.ForMemory.Return(array);
                    array = null;
                }
            }
        }

    }
}
