using System;
using System.Collections.Generic;

namespace Megumin
{
    /// <summary>
    /// 解决泛型不能直接判断 == 问题
    /// <seealso cref="System.Numerics.IEqualityOperators{TSelf, TOther, TResult}"/>"/>
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public class EqualComparer<V>
    {
        protected virtual bool EqualsKey(object x, object y)
        {
            return x == y;
        }

        protected virtual bool EqualsValue(object x, object y)
        {
            return x == y;
        }

        protected bool EqualsValue(bool x, bool y)
        {
            return x == y;
        }

        protected bool EqualsValue(int x, int y)
        {
            return x == y;
        }

        protected bool EqualsValue(float x, float y)
        {
            return x == y;
        }

        protected virtual bool EqualsValue(V x, V y)
        {
            bool flag = EqualityComparer<V>.Default.Equals(x, y);
            return flag;
        }

        protected virtual bool EqualsValue<N>(N x, N y)
            where N : IEquatable<N>
        {
            bool flag = x.Equals(y);
            return flag;
        }
    }
}



