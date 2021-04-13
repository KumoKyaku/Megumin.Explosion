using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// int 最大值最小值表示无穷
    /// <para/>https://stackoverflow.com/questions/21312081/how-to-represent-integer-infinity
    /// </summary>
    [Serializable]
    public struct iint : IComparable<iint>
    {
        public int _int;

        private iint(int value)
        {
            _int = value;
        }

        public static implicit operator int(iint iint)
        {
            return iint._int;
        }

        public static implicit operator iint(int other)
        {
            return new iint(other);
        }

        /// <summary>
        /// 是不是正无穷
        /// </summary>
        public bool IsPositiveInfinity { get { return _int == int.MaxValue; } }

        /// <summary>
        /// 是不是负无穷
        /// </summary>
        public bool IsNegativeInfinity { get { return _int == int.MinValue; } }

        /// <summary>
        /// 正无穷
        /// </summary>
        public static readonly iint PositiveInfinity = int.MaxValue;

        /// <summary>
        /// 负无穷
        /// </summary>
        public static readonly iint NegativeInfinity = int.MinValue;

        public static bool operator ==(iint a, iint b)
        {
            return a._int == b._int;
        }

        public static bool operator !=(iint a, iint b)
        {
            return a._int != b._int;
        }

        public override bool Equals(object obj)
        {
            if (obj is iint iint)
            {
                return this == iint;
            }
#pragma warning disable HAA0102 // Non-overridden virtual method call on value type
            return base.Equals(obj);
#pragma warning restore HAA0102 // Non-overridden virtual method call on value type
        }

        public override int GetHashCode()
        {
            return _int.GetHashCode();
        }

        public static bool operator >(iint a, iint b)
        {
            return a._int > b._int;
        }

        public static bool operator <(iint a, iint b)
        {
            return a._int < b._int;
        }

        public static iint operator +(iint a, iint b)
        {
            if (a.IsPositiveInfinity)
            {
                if (b.IsNegativeInfinity)
                {
                    return 0;
                }
                else
                {
                    return PositiveInfinity;
                }
            }
            else if (a.IsNegativeInfinity)
            {
                if (b.IsPositiveInfinity)
                {
                    return 0;
                }
                else
                {
                    return NegativeInfinity;
                }
            }

            if (b.IsPositiveInfinity)
            {
                if (a.IsNegativeInfinity)
                {
                    return 0;
                }
                else
                {
                    return PositiveInfinity;
                }
            }
            else if (b.IsNegativeInfinity)
            {
                if (a.IsPositiveInfinity)
                {
                    return 0;
                }
                else
                {
                    return NegativeInfinity;
                }
            }

            return a._int + b._int;
        }

        public static iint operator -(iint a, iint b)
        {
            return a + (-b);
        }

        public static iint operator -(iint a)
        {
            if (a.IsNegativeInfinity)
                return PositiveInfinity;
            if (a.IsPositiveInfinity)
                return NegativeInfinity;

            return new iint(-a._int);
        }

        public int CompareTo(iint other)
        {
            return _int.CompareTo(other._int);
        }
    }
}
