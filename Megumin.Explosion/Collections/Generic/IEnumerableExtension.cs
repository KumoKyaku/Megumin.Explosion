using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
    }
}


