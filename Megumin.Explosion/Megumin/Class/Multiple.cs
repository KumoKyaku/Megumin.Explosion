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
    public abstract partial class Multiple<K, V> : IMultiple<K, V>
    {
        protected readonly Dictionary<K, V> ElementDic = new Dictionary<K, V>();
        public event OnValueChanged<V> ValueChanged;
        public event OnValueChanged<(K Key, V Value)> ValueChangedKV;
        public event OnValueChanged<(K Key, V Value)> KeyOrValueChanged;

        public abstract K DefaultKey { get; }
        public abstract V DefaultValue { get; }
        public V Current { get; protected set; }
        public K CurrentKey { get; protected set; }

        /// <summary>
        /// 应用值,使用<see cref="EqualityComparer{T}"/>比较是否发生改变,优化了装箱.
        /// </summary>
        protected virtual void ApplyValue(bool forceRaiseEvent = false)
        {
            var oldValue = Current;
            var oldKey = CurrentKey;
            var (newKey, newValue) = CalNewValue();
            Current = newValue;
            CurrentKey = newKey;

            bool flagV = EqualsValue(oldValue, newValue);
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

            if (!flagV || forceRaiseEvent)
            {
                OnValueChanged(newValue, oldValue);
                OnValueChangedKV((newKey, newValue), (oldKey, oldValue));
            }

            if (!flagV || !EqualsKey(oldKey, newKey) || forceRaiseEvent)
            {
                OnKeyOrValueChanged((newKey, newValue), (oldKey, oldValue));
            }
        }

        /// <summary>
        /// 在控制项没有变动的情况下，触发ApplyValue，尝试触发事件。
        /// </summary>
        public void Refresh(bool forceRaiseEvent = false)
        {
            ApplyValue(forceRaiseEvent);
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

        protected virtual bool EqualsValue(V x, V y)
        {
            bool flag = EqualityComparer<V>.Default.Equals(x, y);
            return flag;
        }

        //public bool Equals(object objA, object objB)
        //{
        //    return object.Equals(objA, objB);
        //}

        protected virtual bool EqualsKey(object x, object y)
        {
            return Equals(x, y);
        }

        protected bool EqualsValue(bool x, bool y)
        {
            return x == y;
        }

        protected bool EqualsValue(int x, int y)
        {
            return x == y;
        }

        protected virtual bool EqualsValue<N>(N x, N y)
            where N : IEquatable<N>
        {
            bool flag = x.Equals(y);
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

        public V Add(K key, V value, bool forceRaiseEvent = false)
        {
            ElementDic[key] = value;
            ApplyValue(forceRaiseEvent);
            return Current;
        }

        public V Remove(K key, V value = default, bool forceRaiseEvent = false)
        {
            ElementDic.Remove(key);
            ApplyValue(forceRaiseEvent);
            return Current;
        }

        public virtual V RemoveAll(bool forceRaiseEvent = false)
        {
            ElementDic.Clear();

            //if (typeof(K).IsValueType || DefaultKey != null)
            //{
            //    //泛型与null比较，IL会装箱，jit会优化掉装箱，IL2CPP不清楚，认为不会优化装箱
            //    //所以加个值类型判定,还需要排除Nullable的null情况。判断太多太过于繁琐。

            //    //微软源码直接用泛型与null比较,这里不做判断了，增加两个构造函数保证DefaultKey 不为null。
            //}

            ElementDic[DefaultKey] = DefaultValue;
            ApplyValue(forceRaiseEvent);
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
