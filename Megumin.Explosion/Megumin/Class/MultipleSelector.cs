using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{

    public enum WhenEqual
    {
        UseFirst,
        UseLast,
        Unkonw,
    }

    /// <summary>
    /// 从一组对象中选择一个对象
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public abstract class MultipleSelector<K, V> : Multiple<K, V>
        where V : IEquatable<V>
    {
        public MultipleSelector(OnValueChanged<(K, V)> onKeyOrValueChanged = null)
        {
            if (onKeyOrValueChanged != null)
            {
                KeyOrValueChanged += onKeyOrValueChanged;
            }
        }

        public WhenEqual WhenEqual { get; set; } = WhenEqual.UseLast;
        public Dictionary<K, DateTimeOffset> JoinTime { get; } = new();

        public override V Add(K key, V value, bool forceRaiseEvent = false)
        {
            if (key is null)
            {
                return Current;
            }

            JoinTime[key] = DateTimeOffset.UtcNow;
            return base.Add(key, value, forceRaiseEvent);
        }

        public override V Remove(K key, V value = default, bool forceRaiseEvent = false)
        {
            JoinTime.Remove(key);
            return base.Remove(key, value, forceRaiseEvent);
        }

        public override V RemoveAll(bool forceRaiseEvent = false)
        {
            JoinTime.Clear();
            return base.RemoveAll(forceRaiseEvent);
        }
    }

    /// <summary>
    /// 开关权重对象选择器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PrioritySelector<T> : MultipleSelector<T, (bool Enabled, int Priority)>
    {
        public PrioritySelector(OnValueChanged<(T, (bool Enabled, int Priority))> onKeyOrValueChanged = null)
            : base(onKeyOrValueChanged)
        {
        }

        public override T DefaultKey { get; }
        public override (bool Enabled, int Priority) DefaultValue { get; }

        public T Join(T key, bool enabled, int priority, bool forceRaiseEvent = false)
        {
            if (key is null)
            {
                return CurrentKey;
            }

            Add(key, (enabled, priority), forceRaiseEvent);
            return CurrentKey;
        }

        protected override (T Key, (bool Enabled, int Priority) Value) CalNewValue()
        {
            var curKey = CurrentKey;
            var curValue = Current;
            var curJoinTime = DateTimeOffset.MinValue;
            if (curKey is not null)
            {
                JoinTime.TryGetValue(curKey, out curJoinTime);
            }

            foreach (var kv in ElementDic)
            {
                var (Enabled, Priority) = kv.Value;
                if (Enabled)
                {
                    JoinTime.TryGetValue(kv.Key, out var jt);
                    if (Priority > curValue.Priority)
                    {
                        //权重大于当前权重
                        curKey = kv.Key;
                        curValue = kv.Value;
                        curJoinTime = jt;
                    }
                    else if (Priority == curValue.Priority)
                    {
                        //权重与当前权重相等
                        if (jt > curJoinTime)
                        {
                            //使用最后加入的对象
                            curKey = kv.Key;
                            curValue = kv.Value;
                            curJoinTime = jt;
                        }
                    }
                }
            }

            return (curKey, curValue);
        }
    }
}



