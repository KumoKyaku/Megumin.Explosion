using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// TODO,GC回收不掉，不知道为什么还是有强引用
    /// </summary>
    public class WeakEvent
    {
        public class WObje
        {
            public WeakReference WeakReference;
            public MethodInfo Method { get; set; }

            internal void Invoke()
            {
                if (WeakReference.IsAlive && WeakReference.Target != null)
                {
                    Method.Invoke(WeakReference.Target, null);
                }
            }
        }
        List<WeakReference> callback = new List<WeakReference>();

        List<WObje> wList = new List<WObje>();
        public event Action OnInvoke
        {
            add
            {
                //WeakReference weak = new WeakReference(value);
                //callback.Add(weak);

                WObje wObje = new WObje();
                wObje.WeakReference = new WeakReference(value.Target);
                wObje.Method = value.Method;
                wList.Add(wObje);
            }
            remove
            {
                //callback.RemoveAll(w => (w.Target as Action) == value);
            }
        }

        public void Invoke()
        {
            using (ListPool<WeakReference>.Rent(out var rList))
            {
                foreach (var item in callback)
                {
                    if (item.IsAlive && item.Target is Action action && action.Target != null)
                    {
                        action?.Invoke();
                    }
                    else
                    {
                        rList.Add(item);
                    }
                }

                foreach (var item in rList)
                {
                    callback.Remove(item);
                }
            }

            foreach (var item in wList)
            {
                var weak = item.WeakReference;
                if (weak.IsAlive && weak.Target != null)
                {
                    item.Invoke();
                }
            }
        }
    }
}
