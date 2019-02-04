using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 内部 在ConcurrentQueue 和 HashSet之间做了很多取舍，最终使用ConcurrentQueue。所以请千万小心，不要将同一个
    /// 数组Push进Pool中两次。
    /// 也许有一天会用Spinlock优化。
    /// <para></para>
    /// 大于16384的请求会直接分配内存，池不起作用，池返回的长度通常比请求长度大。
    /// </summary>
    public class ByteArrayPool
    {
        //32 64 128 256 512 1024 2048 4096 8192 16384
        readonly Bucket[] pools = new Bucket[10];

        /// <summary>
        /// 容量是10
        /// </summary>
        readonly Bucket pool65536;

        internal static readonly ByteArrayPool ForMemory = new ByteArrayPool(0.05f);

        /// <summary>
        /// 全局共用空数组
        /// </summary>
        public static readonly byte[] Empty = new byte[0];

        /// <summary>
        /// 愚蠢的二分查找
        /// </summary>
        /// <param name="minimumLength"></param>
        /// <returns></returns>
        Bucket FindBucket(int minimumLength)
        {
            if (minimumLength == 8192)
            {
                return pools[8];
            }

            if (minimumLength == 16384)
            {
                return pools[9];
            }

            if (minimumLength == 512)
            {
                return pools[4];
            }
            else if (minimumLength < 512)
            {
                if (minimumLength == 128)
                {
                    return pools[2];
                }
                else if (minimumLength < 128)
                {
                    if (minimumLength <= 32)
                    {
                        return pools[0];
                    }
                    else if (minimumLength <= 64)
                    {
                        return pools[1];
                    }
                    else
                    {
                        return pools[2];
                    }
                }
                else
                {
                    if (minimumLength <= 256)
                    {
                        return pools[3];
                    }
                    else
                    {
                        return pools[4];
                    }
                }
            }
            else
            {
                if (minimumLength == 4096)
                {
                    return pools[7];
                }
                else if (minimumLength < 4096)
                {
                    if (minimumLength <= 1024)
                    {
                        return pools[5];
                    }
                    else
                    {
                        if (minimumLength <= 2048)
                        {
                            return pools[6];
                        }
                        else
                        {
                            return pools[7];
                        }
                    }
                }
                else
                {
                    if (minimumLength <= 8192)
                    {
                        return pools[8];
                    }
                    else if (minimumLength > 16384)
                    {
                        return null;
                    }
                    else
                    {
                        return pools[9];
                    }
                }
            }
        }

        /// <summary>
        /// 初始化字节数组池
        /// </summary>
        public ByteArrayPool() : this(0)
        {

        }

        internal ByteArrayPool(float initScale)
        {
            for (int i = 0; i < 10; i++)
            {
                pools[i] = new Bucket((int)Math.Pow(2, i + 5), (10 - i) * 50, initScale);
            }
            pool65536 = new Bucket(65536, 10, initScale);
        }


        /// <summary>
        /// 取得buffer,保证buffer长度大于等于参数长度,最大8192
        /// <para>在返回前已经附加了清零操作</para>
        /// </summary>
        /// <param name="minimunLenght"></param>
        /// <returns></returns>
        public byte[] Rent(int minimunLenght)
        {
            if (minimunLenght <= 0)
            {
                ///增加对长度为0的数组支持
                return Empty;
            }

            var buffer = FindBucket(minimunLenght)?.Rent();
            if (buffer != null)
            {
                Array.Clear(buffer, 0, buffer.Length);
            }
            else
            {
                buffer = new byte[minimunLenght];
            }

            return buffer;
        }

        /// <summary>
        /// 归还buffer
        /// <para>请千万小心，不要将同一个数组Push进Pool中两次，会发生致命错误，池中没有内置去重机制。</para>
        /// </summary>
        /// <param name="buffer"></param>
        public void Return(byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }

            var length = buffer.Length;
            if (length == 0)
            {
                ///增加对长度为0的数组支持
                return;
            }

            FindBucket(length)?.Return(buffer);
        }


        /// <summary>
        /// 取得长度为65536的buffer
        /// </summary>
        public byte[] Rent65536()
        {
            return pool65536.Rent();
        }

        /// <summary>
        /// 归还长度为65536的buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Return65536(byte[] buffer)
        {
            if (buffer.Length != 65536)
            {
                return;
            }

            pool65536.Return(buffer);
        }
        /// <summary>
        /// 取得长度为16384的buffer
        /// </summary>
        public byte[] Rent16384()
        {
            return pools[9].Rent();
        }

        /// <summary>
        /// 归还长度为16384的buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Return16384(byte[] buffer)
        {
            if (buffer.Length != 16384)
            {
                return;
            }

            pools[9].Return(buffer);
        }

        /// <summary>
        /// 清楚当前所有缓存，继续申请会重新分配内存
        /// </summary>
        public void ClearPool()
        {
            for (int i = 0; i < pools.Length; i++)
            {
                var bucket = pools[i];
                pools[i] = new Bucket(pools[i].Size, bucket.MaxCacheCount, 0);
            }
        }
 
        class Bucket
        {
            /// <summary>
            /// 创建桶，并初始化最大容量的1/20
            /// </summary>
            /// <param name="Size"></param>
            /// <param name="maxCacheCount"></param>
            /// <param name="initScale">初始化比例</param>
            public Bucket(int Size,int maxCacheCount = 97,float initScale = 0.05f)
            {
                this.Size = Size;
                this.MaxCacheCount = maxCacheCount;
                bufferPoop = new ConcurrentQueue<byte[]>();
                for (int i = 0; i < MaxCacheCount * initScale; i++)
                {
                    bufferPoop.Enqueue(Create());
                }
            }

            public int Size { get; }
            /// <summary>
            /// 池中最大保留实例个数
            /// </summary>
            public int MaxCacheCount { get; set; }

            readonly ConcurrentQueue<byte[]> bufferPoop;
            internal byte[] Rent()
            {
                if (bufferPoop.TryDequeue(out var buffer))
                {
                    ///双检测
                    if (buffer == null)
                    {
                        return Create();
                    }
                    else
                    {
                        return buffer;
                    }
                }
                else
                {
                    return Create();
                }
            }

            byte[] Create()
            {
                return new byte[Size];
            }

            internal void Return(byte[] buffer)
            {
                if (bufferPoop.Count >= MaxCacheCount)
                {
                    ///丢弃
                }
                else
                {
                    bufferPoop.Enqueue(buffer);
                }
            }
        }
    }


}
