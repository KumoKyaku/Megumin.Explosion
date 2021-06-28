using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    public class XYZ<T> : IXYZ<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }

        public static implicit operator XY<T>(XYZ<T> xyz)
        {
            return new XY<T>() { X = xyz.X, Y = xyz.Y };
        }

        public static implicit operator XZ<T>(XYZ<T> xyz)
        {
            return new XZ<T>() { X = xyz.X, Z = xyz.Z };
        }
    }

    public class XY<T> : IXY<T>
    {
        public T X { get; set; }
        public T Y { get; set; }

        public static implicit operator XYZ<T>(XY<T> xy)
        {
            return new XYZ<T>() { X = xy.X, Y = xy.Y };
        }
    }

    public class XZ<T> : IXZ<T>
    {
        public T X { get; set; }
        public T Z { get; set; }

        public static implicit operator XYZ<T>(XZ<T> xz)
        {
            return new XYZ<T>() { X = xz.X, Z = xz.Z };
        }
    }

    public class YZ<T> : IYZ<T>
    {
        public T Y { get; set; }
        public T Z { get; set; }

        public static implicit operator XYZ<T>(YZ<T> yz)
        {
            return new XYZ<T>() { Y = yz.Y, Z = yz.Z };
        }
    }
}
