using Megumin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    /// <summary>
    /// 值扩展
    /// </summary>
    public static class StructExtension_28FDB7156FD24F39B5EA39D95892E328
    {
        /// <summary>
        /// 将值限定在指定范围内
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cur"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp<T>(this ref T cur,in T min,in T max) where T: struct,IComparable<T>
        {
            var mm = min.CompareTo(max);
            var cm = cur.CompareTo(min);
            if (mm == 0)
            {
                if (cm == 0)
                {
                    return;
                }
                cur = min;
            }
            else if (mm > 0)
            {
                if (cur.CompareTo(max) < 0)
                {
                    cur = max;
                    return;
                }

                if (cm > 0)
                {
                    cur = min;
                    return;
                }

                return;
            }
            else
            {
                if (cm < 0)
                {
                    cur = min;
                    return;
                }

                if (cur.CompareTo(max) > 0)
                {
                    cur = max;
                    return;
                }

                return;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp<T>(this ref T cur,in Threshold<T> threshold) where T : struct, IComparable<T>
        {
            cur.Clamp(threshold.Lower, threshold.Upper);
        }
    }
}
