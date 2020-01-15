using Megumin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    public interface IX<T>
    {
        T X { get; set; }
    }

    public interface IY<T>
    {
        T Y { get; set; }
    }

    public interface IZ<T>
    {
        T Z { get; set; }
    }

    public interface IW<T>
    {
        T W { get; set; }
    }

    public interface IXY<T> : IX<T>, IY<T>
    {
    }

    public interface IXZ<T> : IX<T>, IZ<T>
    {
    }

    public interface IYZ<T> : IY<T>, IZ<T>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>不要重写比较方法</remarks>
    /// <typeparam name="T"></typeparam>
    public interface IXYZ<T> : IX<T>, IY<T>, IZ<T>, IXY<T>, IXZ<T>, IYZ<T>
    {
    }
}

public static class XYZExtension_66E8DCAC
{
    public static void Deconstruct<T>(this IXYZ<T> value,out T x, out T y, out T z)
    {
        x = value.X;
        y = value.Y;
        z = value.Z;
    }

    public static void Deconstruct<T>(this IXZ<T> value, out T x, out T z)
    {
        x = value.X;
        z = value.Z;
    }

    public static void Deconstruct<T>(this T value, out int x, out int y, out int z)
        where T : IXYZ<int>
    {
        x = value.X;
        y = value.Y;
        z = value.Z;
    }

    public static void Deconstruct<V>(this V value, out int x, out int z)
        where V : IXZ<int>
    {
        x = value.X;
        z = value.Z;
    }
}

public static class XYZExtension_765F0A8D
{
    public static void Deconstruct<V>(this V value, out int x, out int y)
        where V : IXY<int>
    {
        x = value.X;
        y = value.Y;
    }

    public static void Deconstruct<V>(this IXY<V> value, out V x, out V y)
    {
        x = value.X;
        y = value.Y;
    }
}

public static class XYZExtension_78970A59
{
    public static void Deconstruct<V>(this V value, out int y, out int z)
        where V : IYZ<int>
    {
        y = value.Y;
        z = value.Z;
    }

    public static void Deconstruct<V>(this IYZ<V> value, out V y, out V z)
    {
        y = value.Y;
        z = value.Z;
    }
}