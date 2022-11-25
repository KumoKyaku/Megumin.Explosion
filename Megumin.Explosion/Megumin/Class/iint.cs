using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// int 最大值最小值表示无穷
    /// <para/>https://stackoverflow.com/questions/21312081/how-to-represent-integer-infinity
    /// </summary>
    [Serializable]
    public partial struct iint : IComparable<iint>
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

        public static iint operator +(iint a)
        {
            return a;
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

#if NET7_0_OR_GREATER

    public partial struct iint: INumber<iint>
    {
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public static bool operator >=(iint left, iint right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <=(iint left, iint right)
        {
            throw new NotImplementedException();
        }

        public static iint operator %(iint left, iint right)
        {
            throw new NotImplementedException();
        }

        public static iint Abs(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsCanonical(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsComplexNumber(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsEvenInteger(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsFinite(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsImaginaryNumber(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsInfinity(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsInteger(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsNaN(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsNegative(iint value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<iint>.IsNegativeInfinity(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsNormal(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsOddInteger(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsPositive(iint value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<iint>.IsPositiveInfinity(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsRealNumber(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsSubnormal(iint value)
        {
            throw new NotImplementedException();
        }

        public static bool IsZero(iint value)
        {
            throw new NotImplementedException();
        }

        public static iint MaxMagnitude(iint x, iint y)
        {
            throw new NotImplementedException();
        }

        public static iint MaxMagnitudeNumber(iint x, iint y)
        {
            throw new NotImplementedException();
        }

        public static iint MinMagnitude(iint x, iint y)
        {
            throw new NotImplementedException();
        }

        public static iint MinMagnitudeNumber(iint x, iint y)
        {
            throw new NotImplementedException();
        }

        public static iint Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static iint Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, [MaybeNullWhen(false)] out iint result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string s, NumberStyles style, IFormatProvider provider, [MaybeNullWhen(false)] out iint result)
        {
            throw new NotImplementedException();
        }

        public static iint One { get; }
        public static int Radix { get; }
        public static iint Zero { get; }

        public bool Equals(iint other)
        {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public static iint Parse(ReadOnlySpan<char> s, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, [MaybeNullWhen(false)] out iint result)
        {
            throw new NotImplementedException();
        }

        public static iint Parse(string s, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out iint result)
        {
            throw new NotImplementedException();
        }

        public static iint AdditiveIdentity { get; }

        public static iint operator --(iint value)
        {
            throw new NotImplementedException();
        }

        public static iint operator /(iint left, iint right)
        {
            throw new NotImplementedException();
        }

        public static iint operator ++(iint value)
        {
            throw new NotImplementedException();
        }

        public static iint MultiplicativeIdentity { get; }

        public static iint operator *(iint left, iint right)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<iint>.TryConvertFromChecked<TOther>(TOther value, out iint result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<iint>.TryConvertFromSaturating<TOther>(TOther value, out iint result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<iint>.TryConvertFromTruncating<TOther>(TOther value, out iint result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<iint>.TryConvertToChecked<TOther>(iint value, out TOther result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<iint>.TryConvertToSaturating<TOther>(iint value, out TOther result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<iint>.TryConvertToTruncating<TOther>(iint value, out TOther result)
        {
            throw new NotImplementedException();
        }
    }

#endif
}
