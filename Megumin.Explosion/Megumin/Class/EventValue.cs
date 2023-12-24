using System;
using System.Collections.Generic;

namespace Megumin
{
    [Serializable]
    public partial class EventValue<T> : EqualComparer<T>
    {
        /// <summary>
        /// 这里public是为了能在unity中序列化，这里没办法标记SerializeField
        /// </summary>
        public T value;

        public event OnGet<T> OnValueGet;
        public event OnSet<T> OnValueSet;
        public event OnChanged<T> OnValueChanged;

        public T Value
        {
            get
            {
                OnValueGet?.Invoke(value);
                return value;
            }

            set
            {
                SetValue(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="raiseEvent">Ignore = -1,Force:101, <seealso cref="RaiseEvent"/></param>
        public void SetValue(T value, int raiseEvent = 0)
        {
            var old = this.value;
            this.value = value;

            //先赋值完成在触发回调
            if (raiseEvent >= 0)
            {
                OnValueSet?.Invoke(value, old);
                if (raiseEvent > 100)
                {
                    if (OnValueChanged != null)
                    {
                        if (!EqualsValue(value, old))
                        {
                            OnValueChanged.Invoke(value, old);
                        }
                    }
                }
            }
        }

        public void SetValueSilent(T value)
        {
            SetValue(value, -1);
        }
    }

    public partial class EventValue<T> : IObservable<T>
    {
        /// <summary>
        /// 为什么需要这个类型包装一次，IObserver.OnNext 不能与ValueChange时间匹配。
        /// </summary>
        protected class ValueObservable : IDisposable
        {
            public EventValue<T> Source { get; internal set; }
            public IObserver<T> Observer { get; private set; }

            private void OnValueChange(T newValue, T oldValue)
            {
                Observer.OnNext(newValue);
            }

            public void Dispose()
            {
                Source.OnValueChanged -= this.OnValueChange;
            }

            internal protected void Subscribe(EventValue<T> eventValue, IObserver<T> observer)
            {
                this.Source = eventValue;
                this.Observer = observer;
                Source.OnValueChanged += this.OnValueChange;
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer != null)
            {
                ValueObservable o = new ValueObservable();
                o.Subscribe(this, observer);
                return o;
            }
            return null;
        }
    }
}




