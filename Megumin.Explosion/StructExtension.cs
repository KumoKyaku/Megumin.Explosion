﻿using Megumin;
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
            if (cur.CompareTo(min) < 0)
            {
                cur = min;
            }
            else if (cur.CompareTo(max) > 0)
            {
                cur = max;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp<T>(this ref T cur,in Threshold<T> threshold) where T : struct, IComparable<T>
        {
            cur.Clamp(threshold.Lower, threshold.Upper);
        }

        /// <summary>
        /// 将值限定在指定范围内,不知道两个边界谁大谁小，消息比<see cref="Clamp{T}(ref T, in T, in T)"/>低一些。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cur"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampIn<T>(this ref T cur, in T value1, in T value2) where T : struct, IComparable<T>
        {
            var mm = value1.CompareTo(value2);
            var cm = cur.CompareTo(value1);
            if (mm == 0)
            {
                if (cm == 0)
                {
                    return;
                }
                cur = value1;
            }
            else if (mm > 0)
            {
                if (cur.CompareTo(value2) < 0)
                {
                    cur = value2;
                    return;
                }

                if (cm > 0)
                {
                    cur = value1;
                    return;
                }

                return;
            }
            else
            {
                if (cm < 0)
                {
                    cur = value1;
                    return;
                }

                if (cur.CompareTo(value2) > 0)
                {
                    cur = value2;
                    return;
                }

                return;
            }
        }
    }
}
