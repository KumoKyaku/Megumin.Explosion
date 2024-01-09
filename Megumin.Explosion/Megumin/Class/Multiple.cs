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
    public abstract partial class Multiple<K, V> : EqualComparer<V>, IMultiple<K, V>
    {
        /// <summary>
        /// 所有构成项的字典，如果没有特殊需求不要再外部更改，使用方法。
        /// </summary>
        public readonly Dictionary<K, V> ElementDic = new();
        public event OnChanged<V> ValueChanged;
        public event OnChanged<(K Key, V Value)> ValueChangedKV;
        public event OnChanged<(K Key, V Value)> KeyOrValueChanged;

        public abstract K DefaultKey { get; }
        public abstract V DefaultValue { get; }
        public V Current { get; protected set; }
        public K CurrentKey { get; protected set; }

        /// <summary>
        /// 应用值,使用<see cref="EqualityComparer{T}"/>比较是否发生改变,优化了装箱.
        /// </summary>
        protected virtual void ApplyValue(int raiseEvent = 0)
        {
            var oldValue = Current;
            var oldKey = CurrentKey;
            var (newKey, newValue) = CalNewValue();
            Current = newValue;
            CurrentKey = newKey;


            bool flagV = false;
            bool flagKorV = false;

            if (raiseEvent > 100)
            {
                //强制触发事件，不必判断相等性。
                flagKorV = true;
                flagV = true;
            }
            else if (raiseEvent >= 0)
            {
                flagV = !EqualsValue(oldValue, newValue);
                flagKorV = flagV || !EqualsKey(oldKey, newKey);
            }


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

            if (flagV)
            {
                OnValueChanged(newValue, oldValue);
                OnValueChangedKV((newKey, newValue), (oldKey, oldValue));
            }

            if (flagKorV)
            {
                OnKeyOrValueChanged((newKey, newValue), (oldKey, oldValue));
            }
        }

        public void Refresh(int raiseEvent = 0)
        {
            ApplyValue(raiseEvent);
        }

        /// <summary>
        /// 判定Key是否相等
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <remarks>
        /// 必须使用EqualsKey，EqualsValue两个函数名，不能同名使用重载，否则在子类重写时会提示二义性，无法重写，编译错误。
        /// </remarks>
        protected virtual bool EqualsKey(K x, K y)
        {
            bool flag = EqualityComparer<K>.Default.Equals(x, y);
            return flag;
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

        /// <summary>
        /// 包装一下,不然子类不能调用. Event不带On,方法带On.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        protected void OnValueChangedKV((K, V) newValue, (K, V) oldValue)
        {
            ValueChangedKV?.Invoke(newValue, oldValue);
        }

        protected void OnKeyOrValueChanged((K, V) newValue, (K, V) oldValue)
        {
            KeyOrValueChanged?.Invoke(newValue, oldValue);
        }

        public virtual V Add(K key, V value, int raiseEvent = 0)
        {
            if (key is null)
            {
                return Current;
            }

            ElementDic[key] = value;
            ApplyValue(raiseEvent);
            return Current;
        }

        public virtual V Remove(K key, V value = default, int raiseEvent = 0)
        {
            if (key is null)
            {
                return Current;
            }

            ElementDic.Remove(key);
            ApplyValue(raiseEvent);
            return Current;
        }

        public virtual V RemoveAll(int raiseEvent = 0)
        {
            ElementDic.Clear();

            //if (typeof(K).IsValueType || DefaultKey != null)
            //{
            //    //泛型与null比较，IL会装箱，jit会优化掉装箱，IL2CPP不清楚，认为不会优化装箱
            //    //所以加个值类型判定,还需要排除Nullable的null情况。判断太多太过于繁琐。

            //    //微软源码直接用泛型与null比较,这里不做判断了，增加两个构造函数保证DefaultKey 不为null。
            //}
            if (DefaultKey is not null)
            {
                ElementDic[DefaultKey] = DefaultValue;
            }

            ApplyValue(raiseEvent);
            return Current;
        }

        public V RemoveAll(Func<KeyValuePair<K, V>, bool> predicate, int raiseEvent = 0)
        {
            ElementDic.RemoveAll(predicate);

            if (DefaultKey is not null)
            {
                ElementDic[DefaultKey] = DefaultValue;
            }

            ApplyValue(raiseEvent);
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

    partial class Multiple<K, V> : IObservable<V>
    {
        /// <summary>
        /// 为什么需要这个类型包装一次，IObserver.OnNext 不能与ValueChange时间匹配。
        /// </summary>
        class PropertyObserver : IDisposable
        {
            public Multiple<K, V> Multiple { get; internal set; }
            public IObserver<V> Observer { get; private set; }

            private void OnValueChange(V newValue, V oldValue)
            {
                Observer.OnNext(newValue);
            }

            public void Dispose()
            {
                Multiple.ValueChanged -= this.OnValueChange;
            }

            internal void Subscribe(Multiple<K, V> multiple, IObserver<V> observer)
            {
                this.Multiple = multiple;
                this.Observer = observer;
                Multiple.ValueChanged += this.OnValueChange;
            }
        }

        public IDisposable Subscribe(IObserver<V> observer)
        {
            if (observer != null)
            {
                PropertyObserver o = new PropertyObserver();
                o.Subscribe(this, observer);
                return o;
            }
            return null;
        }
    }

    partial class Multiple<K, V> : IObservable<(K Source, V Value)>
    {
        class PropertyObserverKV : IDisposable
        {
            public Multiple<K, V> Multiple { get; private set; }
            public IObserver<(K Source, V Value)> Observer { get; private set; }

            internal void Subscribe(Multiple<K, V> multiple, IObserver<(K Source, V Value)> observer)
            {
                this.Multiple = multiple;
                this.Observer = observer;
                Multiple.KeyOrValueChanged += this.OnKeyOrValueChanged;
            }

            private void OnKeyOrValueChanged((K Key, V Value) newValue, (K Key, V Value) oldValue)
            {
                Observer.OnNext(newValue);
            }

            public void Dispose()
            {
                Multiple.KeyOrValueChanged -= this.OnKeyOrValueChanged;
            }
        }

        /// <summary>
        /// 当前键值任一发生改变时触发
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<(K Source, V Value)> observer)
        {
            if (observer != null)
            {
                PropertyObserverKV o = new PropertyObserverKV();
                o.Subscribe(this, observer);
                return o;
            }
            return null;
        }
    }
}
