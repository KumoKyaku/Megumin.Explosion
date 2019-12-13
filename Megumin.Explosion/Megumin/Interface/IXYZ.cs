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
        void Deconstruct(out T x, out T y);
    }

    public interface IXZ<T> : IX<T>, IZ<T>
    {
        void Deconstruct(out T x, out T z);
    }

    public interface IYZ<T> : IY<T>, IZ<T>
    {
        void Deconstruct(out T y, out T z);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>不要重写比较方法</remarks>
    /// <typeparam name="T"></typeparam>
    public interface IXYZ<T> : IX<T>, IY<T>, IZ<T>, IXY<T>, IXZ<T>, IYZ<T>
    {
        void Deconstruct(out T x, out T y, out T z);
    }
}