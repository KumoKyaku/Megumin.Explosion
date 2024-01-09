using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Megumin;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensionD36DB03F995A41EA836C18482535C530
    {
        static Random random = new Random(DateTimeOffset.UtcNow.GetHashCode());

        public static T Random<T>(this IList<T> enumerable)
        {
            if (enumerable.Count > 0)
            {
                return enumerable[random.Next(0, enumerable.Count)];
            }
            return default;
        }

        public static int RemoveAllNull<T>(this List<T> enumerable)
            where T : class
        {
            return enumerable.RemoveAll(static ele => ele == null);
        }

        public static bool ValidIndex<T>(this IList<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        public static bool ValidIndex(this Array array, int index)
        {
            return index >= 0 && index < array.Length;
        }

        public static T Random<T>(this ICollection<T> enumerable)
        {
            if (enumerable.Count > 0)
            {
                return enumerable.ElementAt(random.Next(0, enumerable.Count));
            }
            return default;
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            var count = enumerable.Count();
            if (count > 0)
            {
                return enumerable.ElementAt(random.Next(0, count));
            }
            return default;
        }

        public static bool AddIfNotContains<T>(this ICollection<T> enumerable, T item)
            where T : class
        {
            if (enumerable.Contains(item))
            {
                return false;
            }

            enumerable.Add(item);
            return true;
        }

        /// <summary>
        /// 按照AaBbCc排序比较
        /// </summary>
        /// <param name="list"></param>
        public static void SortAaBbCc(this List<string> list)
        {
            list.Sort((a, b) => a.CompareAaBbCc(b));
        }

        public struct Remover<T> : IDisposable, IEnumerable<T>
        {
            private readonly ICollection<T> sourceCollection;
            /// <summary>
            /// 待移除缓存区
            /// </summary>
            private List<T> cache;

            public Remover(ICollection<T> collection)
            {
                this.sourceCollection = collection;
                cache = ListPool<T>.Shared.Rent();
            }

            /// <summary>
            /// 添加元素到待移除缓存区
            /// </summary>
            /// <param name="item"></param>
            public void Push(T item)
            {
                RemoveDelay(item);
            }

            /// <summary>
            /// 添加元素到待移除缓存区
            /// </summary>
            /// <param name="item"></param>
            public void Add(T item)
            {
                RemoveDelay(item);
            }

            /// <summary>
            /// 添加元素到待移除缓存区
            /// </summary>
            /// <param name="item"></param>
            public void DelayRemove(T item)
            {
                RemoveDelay(item);
            }

            /// <summary>
            /// 添加元素到待移除缓存区
            /// </summary>
            /// <param name="item"></param>
            public void RemoveDelay(T item)
            {
                if (cache == null)
                {
                    cache = ListPool<T>.Shared.Rent();
                }
                cache.Add(item);
            }

            /// <summary>
            /// 现在从源容器中移除所有待移除缓存区中的元素
            /// </summary>
            public void RemoveNow()
            {
                if (cache != null)
                {
                    foreach (var item in cache)
                    {
                        sourceCollection.Remove(item);
                    }
                    ListPool<T>.Shared.Return(ref cache);
                    cache = null;
                }
            }

            public void Dispose()
            {
                RemoveNow();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return ((IEnumerable<T>)cache).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)cache).GetEnumerator();
            }
        }

        /// <summary>
        /// 获得一个在foreach中使用的删除器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Remover<T> GetForeachRemover<T>(this ICollection<T> list)
        {
            return new Remover<T>(list);
        }

        public static List<T> RentRemoveList<T>(this ICollection<T> list)
        {
            return ListPool<T>.Shared.Rent();
        }

        public static void ReturnRemoveList<T>(this ICollection<T> list, ref List<T> element, bool forceSafeCheck = false)
        {
            ListPool<T>.Shared.Return(ref element, forceSafeCheck);
        }
    }
}


