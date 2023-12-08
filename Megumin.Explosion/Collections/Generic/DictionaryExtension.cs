using Megumin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Collections.Generic
{
    public static class KeyValuePairExtension_5EF11360ED17491CA476C08A2272FE4F
    {
        public static void Deconstruct<TKey, TValue>(this in KeyValuePair<TKey, TValue> pair,
                                                     out TKey Key,
                                                     out TValue Value)
        {
            Key = pair.Key;
            Value = pair.Value;
        }
    }

    /// <summary>
    /// 字典扩展
    /// </summary>
    public static class DictionaryExtension_5191D922C5B740EBB5B4C72E5DA5C11C
    {
        [Obsolete("不是绝对安全的，没有固定指针，内存移动时会出bug,以后要仔细研究一下", true)]
        public static void RemoveAllUnSafe<K, V>(this Dictionary<K, V> source, Func<KeyValuePair<K, V>, bool> predicate)
        {
            if (predicate == null || source == null)
            {
                return;
            }

            unsafe
            {
                var rl = stackalloc IntPtr[source.Count];
                int index = 0;
                lock (source)
                {
                    try
                    {
                        foreach (var item in source)
                        {
                            if (predicate(item))
                            {
                                rl[index] = (IntPtr)GCHandle.Alloc(item.Key);
                            }
                            else
                            {
                                rl[index] = IntPtr.Zero;
                            }
                            index++;
                        }

                        for (int i = 0; i < index; i++)
                        {
                            IntPtr intPtr = rl[i];
                            if (intPtr != IntPtr.Zero)
                            {
                                GCHandle handle = (GCHandle)intPtr;
                                source.Remove((K)handle.Target);
                            }
                        }
                    }
                    finally
                    {
                        for (int i = 0; i < index; i++)
                        {
                            IntPtr intPtr = rl[i];
                            if (intPtr != IntPtr.Zero)
                            {
                                GCHandle handle = (GCHandle)intPtr;
                                if (handle.IsAllocated)
                                {
                                    handle.Free();
                                }
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 使用<see cref="ListPool{T}"/>缓存key实现。
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        public static void RemoveAll<K, V>(this Dictionary<K, V> source, Func<KeyValuePair<K, V>, bool> predicate)
        {
            if (predicate == null || source == null || source.Count == 0)
            {
                return;
            }

            using (var handle = ListPool<K>.Shared.Rent(out var removeList))
            {
                foreach (var item in source)
                {
                    if (predicate(item))
                    {
                        removeList.Add(item.Key);
                    }
                }

                foreach (var key in removeList)
                {
                    source.Remove(key);
                }
            }
        }

        /// <summary>
        /// 如果字典特别大可能会爆栈
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        public static void RemoveAll<V>(this Dictionary<int, V> source, Func<KeyValuePair<int, V>, bool> predicate)
        {
            if (predicate == null || source == null || source.Count == 0)
            {
                return;
            }

            unsafe
            {
                var rList = stackalloc int[source.Count];
                int index = 0;

                lock (source)
                {
                    foreach (var item in source)
                    {
                        if (predicate(item))
                        {
                            rList[index] = item.Key;
                            index++;
                        }
                    }

                    for (int i = 0; i < index; i++)
                    {
                        source.Remove(rList[i]);
                    }
                }
            }
        }

        public static void RemoveAll<V>(this Dictionary<long, V> source, Func<KeyValuePair<long, V>, bool> predicate)
        {
            if (predicate == null || source == null || source.Count == 0)
            {
                return;
            }

            unsafe
            {
                var rList = stackalloc long[source.Count];
                int index = 0;

                lock (source)
                {
                    foreach (var item in source)
                    {
                        if (predicate(item))
                        {
                            rList[index] = item.Key;
                            index++;
                        }
                    }

                    for (int i = 0; i < index; i++)
                    {
                        source.Remove(rList[i]);
                    }
                }
            }
        }

        public struct Remover<K, V>
        {
            private readonly IDictionary<K, V> list;
            private List<K> cache;

            public Remover(IDictionary<K, V> list)
            {
                this.list = list;
                cache = ListPool<K>.Shared.Rent();
            }

            public void Push(K item)
            {
                DelayRemove(item);
            }

            public void DelayRemove(K item)
            {
                if (cache == null)
                {
                    cache = ListPool<K>.Shared.Rent();
                }
                cache.Add(item);
            }

            public void RemoveNow()
            {
                foreach (var item in cache)
                {
                    list.Remove(item);
                }
                ListPool<K>.Shared.Return(ref cache);
                cache = null;
            }
        }

        /// <summary>
        /// 获得一个在foreach中使用的删除器
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Remover<K, V> GetForeachRemover<K, V>(this IDictionary<K, V> source)
        {
            return new Remover<K, V>(source);
        }
    }
}
