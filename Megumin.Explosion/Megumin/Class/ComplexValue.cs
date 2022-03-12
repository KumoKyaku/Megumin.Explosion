using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 对象有多个部分构成
    /// </summary>
    public interface IMultiple<K,V>
    {
        /// <summary>
        /// 默认值Key
        /// </summary>
        K DefaultKey { get; }
        
        /// <summary>
        /// 默认值
        /// </summary>
        /// <remarks>有必要存在,有的需求要设定的值可能就是默认值取反</remarks>
        V DefaultValue { get; }

        /// <summary>
        /// 当前值
        /// </summary>
        V Current { get; }

        /// <summary>
        /// 加入一个构成项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        V Add(K key, V value);
        
        /// <summary>
        /// 移除一个构成项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">因为总是复制粘贴Add,参数个数对不上,这个值没有使用,只是为了对齐参数个数.</param>
        V Remove(K key, V value = default);
        
        /// <summary>
        /// 取消除默认值以外的所有构成项
        /// </summary>
        V RemoveAll();

        /// <summary>
        /// 值发生改变
        /// </summary>
        event OnValueChanged<V> ValueChanged;
    }


    /// <summary>
    /// 复合值，一个值的某些构成是有限时间的。
    /// 和IMultipleControlable很像，但是不排序，是求和。
    /// </summary>
    /// <remarks>
    /// 最初需求是被弹反给自己增加一个被弹反计数，这一个有效期5秒，如果计数大于3，进入被弹反硬直。
    /// 这导致弹反计数中，每个弹反点有自己的生命周期，计算总和十分麻烦。
    /// <para></para>
    /// 需求二：护盾由各种护盾构成，蓝盾黄甲，算是生命值，每个构成项可以被消耗。
    /// 如果HP500，蓝盾200，黄甲300，遇到 350点伤害，消耗掉蓝盾100，黄甲100，HP150。
    /// </remarks>
    public abstract class ComplexValue<K,V>
    {
        protected readonly Dictionary<K, V> Controllers = new Dictionary<K, V>();
        public event OnValueChanged<V> ValueChanged;

        protected readonly K defaultKey;
        protected readonly V defaultValue;
        public K DefaultKey => defaultKey;
        public V DefaultValue => defaultValue;
        public V Current { get; protected set; }
        public K CurrentKey { get; protected set; }

        public V Add(K key, V value)
        {
            Controllers[key] = value;
            ApplyValue();
            return Current;
        }

        public virtual void ApplyValue()
        {
            var oldValue = Current;
            var oldKey = CurrentKey;
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

        protected abstract (K Key, V Value) CalNewValue();

        protected void OnValueChanged(V newValue, V oldValue)
        {
            ValueChanged?.Invoke(newValue, oldValue);
        }
    }

    public class ComplexValueObjInt: ComplexValue<object,int>
    {
        protected override (object Key, int Value) CalNewValue()
        {
            object key = null;
            int value = 0;  
            foreach (var item in Controllers)
            {
                key =item.Key;
                value += item.Value;
            }
            return (key, value);
        }
    }
}


