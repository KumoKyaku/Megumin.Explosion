using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 线程安全List池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListPool<T>
    {
        static ConcurrentQueue<List<T>> pool = new ConcurrentQueue<List<T>>();

        /// <summary>
        /// 默认容量10
        /// </summary>
        public static int MaxSize { get; set; } = 10;

        public static List<T> Rent()
        {
            if (pool.TryDequeue(out var list))
            {
                if (list == null || list.Count != 0)
                {
                    return new List<T>();
                }
                return list;
            }
            else
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// 调用者保证归还后不在使用当前list
        /// </summary>
        /// <param name="list"></param>
        public static void Return(List<T> list)
        {
            if (list == null)
            {
                return;
            }

            if (pool.Count < MaxSize)
            {
                list.Clear();
                pool.Enqueue(list);
            }
        }

        public static void Clear()
        {
            while (pool.Count > 0)
            {
                pool.TryDequeue(out var list);
            }
        }

    }
}
