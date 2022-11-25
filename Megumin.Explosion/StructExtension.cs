using Megumin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

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
    public static void Clamp<T>(this ref T cur, in T min, in T max) where T : struct, IComparable<T>
    {
        cur = cur.GetClamp(min, max);
    }

    /// <summary>
    /// 返回限定在指定范围内的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cur"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetClamp<T>(this T cur, in T min, in T max) where T : struct, IComparable<T>
    {
        var res = cur;

        if (min.CompareTo(max) <= 0)
        {
            if (cur.CompareTo(min) < 0)
            {
                res = min;
            }
            else if (cur.CompareTo(max) > 0)
            {
                res = max;
            }
        }
        else
        {
            if (cur.CompareTo(max) < 0)
            {
                res = max;
            }
            else if (cur.CompareTo(min) > 0)
            {
                res = min;
            }
        }

        return res;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp<T>(this ref T cur, in Threshold<T> threshold) where T : struct, IComparable<T>
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
    [Obsolete("", true)]
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="orignal"></param>
    /// <param name="multiple">倍数</param>
    public static void SnapCeil(this ref int orignal, double multiple = 1)
    {
        if (orignal % multiple == 0)
        {

        }
        else
        {
            var scale = Math.Ceiling(orignal / multiple);
            orignal = (int)(scale * multiple);
        }
    }

    /// <summary>
    /// 就近舍入  四舍六入五近偶
    /// </summary>
    /// <param name="orignal"></param>
    /// <param name="multiple"></param>
    public static void SnapRound(this ref int orignal, double multiple = 1)
    {
        if (orignal % multiple == 0)
        {

        }
        else
        {
            var scale = Math.Round(orignal / multiple);
            orignal = (int)(scale * multiple);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="orignal"></param>
    /// <param name="multiple">倍数</param>
    public static void SnapFloor(this ref int orignal, double multiple = 1)
    {
        if (orignal % multiple == 0)
        {

        }
        else
        {
            var scale = Math.Floor(orignal / multiple);
            orignal = (int)(scale * multiple);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="orignal"></param>
    /// <param name="multiple">倍数</param>
    public static void SnapCeil(this ref double orignal, double multiple = 1)
    {
        if (orignal % multiple == 0)
        {

        }
        else
        {
            var scale = Math.Ceiling(orignal / multiple);
            orignal = ((int)scale) * multiple;
        }
    }

    /// <summary>
    /// 就近舍入  四舍六入五近偶
    /// </summary>
    /// <param name="orignal"></param>
    /// <param name="multiple"></param>
    public static void SnapRound(this ref double orignal, double multiple = 1)
    {
        if (orignal % multiple == 0)
        {

        }
        else
        {
            var scale = Math.Round(orignal / multiple);
            orignal = ((int)scale) * multiple;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="orignal"></param>
    /// <param name="multiple">倍数</param>
    public static void SnapFloor(this ref double orignal, double multiple = 1)
    {
        if (orignal % multiple == 0)
        {

        }
        else
        {
            var scale = Math.Floor(orignal / multiple);
            orignal = ((int)scale) * multiple;
        }
    }

    public static unsafe string ToBinaryString(this int x, bool insertSeparator = false)
    {
        if (insertSeparator)
        {
            char[] s = new char[39];

            for (int i = 0; i < 39; i++)
            {
                s[i] = '_';
            }

            for (int i = 0; i < 32; i++)
            {
                var index = i + i / 4;
                s[index] = ((x >> 31 - i) & 1) == 1 ? '1' : '0';
            }

            return new string(s);
        }

        return Convert.ToString(x, 2).PadLeft(32, '0');
    }
}

