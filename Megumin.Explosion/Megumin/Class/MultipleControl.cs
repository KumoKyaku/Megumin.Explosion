using System;
using System.Collections.Generic;
using System.Linq;

namespace Megumin
{
    /// <summary>
    /// 多重值控制器。
    /// 不提供默认值访问，也不保存默认值。会导致过渡设计。
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <remarks>
    /// 用例：当前声音100，
    /// A功能需要将声音压低为20，A功能结束后恢复原音量。
    /// B功能需要将声音改为50，B功能结束后恢复音量。
    /// <para></para>
    /// 常规做法：A设置20前取得当前音量，结束后设置回去。
    /// 这种做法可能导致B功能开始时取得的当前值是A已经设置的音量20。
    /// 最终结束时B将音量设置为20，导致音量出错。
    /// <para></para>
    /// 实现思路：
    /// 控制器由值持有者初始化，并将自身和默认设定到控制器中。
    /// 想要控制声音，将一个对象当作key传递给控制器，
    /// 控制器从所有想要控制值的列表中选出想要的值。
    /// <para></para>
    /// 用例2：多个功能想要黑屏Loading。只要有一个功能还需要Loading，那么Loading就不该消失。
    /// </remarks>
    public class MultipleControl<K,V>
    {
        protected readonly Dictionary<K, V> Controllers = new Dictionary<K, V>();

        /// <summary>
        /// 排序用Linq表达式
        /// </summary>
        protected IEnumerable<V> SortLinq;

        /// <summary>
        /// 将值设置到目标上
        /// </summary>
        protected Action<V> OnControlledValue;

        /// <summary>
        /// 默认值Key
        /// </summary>
        public readonly K DefaultKey;

        /// <summary>
        /// 当前值
        /// </summary>
        public V Current { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultKey">默认key</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="onControlledValue">设置值回调函数</param>
        public MultipleControl(K defaultKey, V defaultValue, Action<V> onControlledValue)
        {
            DefaultKey = defaultKey;
            Controllers[defaultKey] = defaultValue;
            OnControlledValue = onControlledValue;
            InitSortLinq();
            ApplyValue();
        }

        /// <summary>
        /// 初始化值计算
        /// </summary>
        /// <remarks>默认根据值升序</remarks>
        protected virtual void InitSortLinq()
        {
            SortLinq = from kv in Controllers
                      orderby kv.Value ascending
                      select kv.Value;
        }

        /// <summary>
        /// 开始控制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Control(K key, V value)
        {
            Controllers[key] = value;
            ApplyValue();
        }

        /// <summary>
        /// 取消控制
        /// </summary>
        /// <param name="key"></param>
        public void Cancel(K key)
        {
            Controllers.Remove(key);
            ApplyValue();
        }

        /// <summary>
        /// 取消除默认值以外的所有控制
        /// </summary>
        /// <param name="key"></param>
        public void CancelAll()
        {
            var defaultValue = Controllers[DefaultKey];
            Controllers.Clear();
            Controllers[DefaultKey] = defaultValue;
            ApplyValue();
        }

        /// <summary>
        /// 应用值
        /// </summary>
        protected virtual void ApplyValue()
        {
            var result = SortLinq.FirstOrDefault();
            Current = result;
            OnControlledValue?.Invoke(result);
        }

        /// <summary>
        /// 返回当前值
        /// </summary>
        /// <param name="multipleControl"></param>
        public static implicit operator V(MultipleControl<K, V> multipleControl)
        {
            return multipleControl.Current;
        }
    }

    ///<inheritdoc/>
    public class SimpleMultipleControl<V> : MultipleControl<object, V>
    {
        ///<inheritdoc/>
        public SimpleMultipleControl(object defaultHandle, V defaultValue, Action<V> OnChangedValue) 
            : base(defaultHandle, defaultValue, OnChangedValue)
        {
        }
    }

    /// <summary>
    /// 黑屏Loading控制
    /// </summary>
    ///<inheritdoc/>
    public class BlackScreenMultipleControl : SimpleMultipleControl<bool>
    {
        ///<inheritdoc/>
        public BlackScreenMultipleControl(object defaultHandle, bool defaultValue, Action<bool> OnChangedValue) 
            : base(defaultHandle, defaultValue, OnChangedValue)
        {
        }

        protected override void InitSortLinq()
        {
            //bool true 是1 false 是 0，所以需要降序排列
            SortLinq = from kv in Controllers
                       orderby kv.Value descending
                       select kv.Value;
        }
    }
}


