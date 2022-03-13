using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// <inheritdoc cref="IMultiple{K, V}"/>
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public abstract class Multiple<K, V> : IMultiple<K, V>
    {
        protected readonly Dictionary<K, V> ElementDic = new Dictionary<K, V>();
        public event OnValueChanged<V> ValueChanged;

        public abstract K DefaultKey { get; }
        public abstract V DefaultValue { get; }
        public V Current { get; protected set; }
        public K CurrentKey { get; protected set; }

        protected virtual void ApplyValue()
        {
            var oldValue = Current;
            //var oldKey = CurrentKey;
            var (newKey, newValue) = CalNewValue();
            Current = newValue;
            CurrentKey = newKey;

            bool flagV = EqualityComparer<V>.Default.Equals(oldValue, newValue);
            //if (old is IEquatable<V> oe)
            //{
            //    //转成接口必然装箱
            //    flagV = oe.Equals(Current);
            //}
            //else
            //{
            //    //Equals方法两个对象都要装箱
            //    flagV = Equals(old, Current);
            //}

            if (!flagV)
            {
                OnValueChanged(newValue, oldValue);
            }
        }

        /// <summary>
        /// 计算新的值, 返回值也可以用KeyValuePair,没什么区别.
        /// </summary>
        /// <returns></returns>
        /// <remarks>有的多重控制并不是比较大小,例如FlagEnum,可能时其他运算</remarks>
        protected abstract (K Key, V Value) CalNewValue();

        protected void OnValueChanged(V newValue, V oldValue)
        {
            ValueChanged?.Invoke(newValue, oldValue);
        }

        public V Add(K key, V value)
        {
            ElementDic[key] = value;
            ApplyValue();
            return Current;
        }

        public V Remove(K key, V value = default)
        {
            ElementDic.Remove(key);
            ApplyValue();
            return Current;
        }

        public virtual V RemoveAll()
        {
            ElementDic.Clear();

            //if (typeof(K).IsValueType || DefaultKey != null)
            //{
            //    //泛型与null比较，IL会装箱，jit会优化掉装箱，IL2CPP不清楚，认为不会优化装箱
            //    //所以加个值类型判定,还需要排除Nullable的null情况。判断太多太过于繁琐。

            //    //微软源码直接用泛型与null比较,这里不做判断了，增加两个构造函数保证DefaultKey 不为null。
            //}

            ElementDic[DefaultKey] = DefaultValue;
            ApplyValue();
            return Current;
        }

        /// <summary>
        /// 返回当前值
        /// </summary>
        public static implicit operator V(Multiple<K, V> multiple)
        {
            return multiple.Current;
        }
    }
}
