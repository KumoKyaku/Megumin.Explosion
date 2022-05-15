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

        public static void SortAaBbCc(this List<string> list)
        {
            list.Sort((a, b) => a.CompareAaBbCc(b));
        }

        public struct Remover<T>
        {
            private readonly ICollection<T> list;
            private List<T> cache;

            public Remover(ICollection<T> list)
            {
                this.list = list;
                cache = ListPool<T>.Rent();
            }

            public void Push(T item)
            {
                DelayRemove(item);
            }

            public void DelayRemove(T item)
            {
                if (cache == null)
                {
                    cache = ListPool<T>.Rent();
                }
                cache.Add(item);
            }

            public void RemoveNow()
            {
                foreach (var item in cache)
                {
                    list.Remove(item);
                }
                ListPool<T>.Return(ref cache);
                cache = null;
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
    }
}


