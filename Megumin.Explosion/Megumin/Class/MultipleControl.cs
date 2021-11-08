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
    public class MultipleControl<K, V>
        where V : IEquatable<V>
    {
        /// <summary>
        /// TODO,使用最大堆最小堆优化
        /// </summary>
        protected readonly Dictionary<K, V> Controllers = new Dictionary<K, V>();

        /// <summary>
        /// 排序用Linq表达式
        /// </summary>
        protected IEnumerable<KeyValuePair<K, V>> SortLinqKV;


        /// <summary>
        /// 值发生改变
        /// </summary>
        public event OnValueChanged<V> OnValueChanged;

        /// <summary>
        /// 值发生改变
        /// </summary>
        public event OnValueChanged<(K, V)> OnValueChangedKV;

        /// <summary>
        /// 默认值Key
        /// </summary>
        public readonly K DefaultKey;
        /// <summary>
        /// 默认值
        /// </summary>
        /// <remarks>有必要存在,有的需求要设定的值可能就是默认值取反</remarks>
        public readonly V DefaultValue;

        /// <summary>
        /// 当前值
        /// </summary>
        public V Current { get; private set; }
        public K CurrentKey { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="onValueChanged"></param>
        /// <param name="onValueChangedKV"></param>
        /// <param name="ascending"></param>
        public MultipleControl(K defaultKey,
                               V defaultValue,
                               OnValueChanged<V> onValueChanged = null,
                               OnValueChanged<(K, V)> onValueChangedKV = null,
                               bool ascending = true)
        {
            DefaultKey = defaultKey;
            DefaultValue = defaultValue;
            Controllers[defaultKey] = defaultValue;
            InitSortLinq(ascending);

            if (onValueChanged != null)
            {
                OnValueChanged += onValueChanged;
            }

            if (onValueChangedKV != null)
            {
                OnValueChangedKV += onValueChangedKV;
            }

            ApplyValue();
        }

        /// <summary>
        /// 初始化值计算
        /// </summary>
        /// <remarks>默认根据值升序,应用第一个值，也就是结果最小的</remarks>
        protected virtual void InitSortLinq(bool ascending = true)
        {
            if (ascending)
            {
                SortLinqKV = from kv in Controllers
                             orderby kv.Value ascending
                             select kv;
            }
            else
            {
                SortLinqKV = from kv in Controllers
                             orderby kv.Value descending
                             select kv;
            }
        }

        /// <summary>
        /// true 按升序排列，结果为应用最小值，false为降序排列，结果为应用最大值。
        /// </summary>
        /// <param name="ascending"></param>
        public V ReInitSortLinq(bool ascending = true)
        {
            InitSortLinq(ascending);
            ApplyValue();
            return Current;
        }

        /// <summary>
        /// 开始控制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public V Control(K key, V value)
        {
            Controllers[key] = value;
            ApplyValue();
            return Current;
        }

        /// <summary>
        /// 取消控制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">因为总是复制粘贴Control,参数个数对不上,这个值没有使用,只是为了对其参数个数.</param>
        public V Cancel(K key, V value = default)
        {
            Controllers.Remove(key);
            ApplyValue();
            return Current;
        }

        /// <summary>
        /// 取消除默认值以外的所有控制
        /// </summary>
        public V CancelAll()
        {
            Controllers.Clear();
            Controllers[DefaultKey] = DefaultValue;
            ApplyValue();
            return Current;
        }

        /// <summary>
        /// 应用值
        /// </summary>
        protected virtual void ApplyValue()
        {
            var old = Current;
            var oldK = CurrentKey;
            var result = SortLinqKV.FirstOrDefault();
            Current = result.Value;
            CurrentKey = result.Key;

            if (!old.Equals(result))
            {
                OnValueChanged?.Invoke(Current, old);
            }

            if (!old.Equals(result) || !oldK.Equals(CurrentKey))
            {
                OnValueChangedKV?.Invoke((CurrentKey, Current), (oldK, old));
            }
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
    [Obsolete("Use MultipleControl<K, V> instead.")]
    public class MultipleControl<V> : MultipleControl<object, V>
        where V : IEquatable<V>
    {
        public MultipleControl(object defaultHandle,
                               V defaultValue,
                               OnValueChanged<V> onValueChanged = null,
                               OnValueChanged<(object, V)> onValueChangedKV = null,
                               bool ascending = true)
            : base(defaultHandle, defaultValue, onValueChanged, onValueChangedKV, ascending)
        {

        }
    }

    /// <summary>
    /// 开启关闭控制，只有有个一个控制源开启，结果就是开启
    /// </summary>
    /// <remarks>处理黑屏，碰撞盒开闭</remarks>
    public class ActiveControl : MultipleControl<object, bool>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultHandle"></param>
        /// <param name="defaultValue"></param>
        /// <param name="onValueChanged"></param>
        /// <param name="onValueChangedKV"></param>
        /// <param name="ascending">默认false,任意一个开启就开启,true,任意一个关闭就关闭.</param>
        public ActiveControl(object defaultHandle,
                             bool defaultValue,
                             OnValueChanged<bool> onValueChanged = null,
                             OnValueChanged<(object, bool)> onValueChangedKV = null,
                             bool ascending = false)
            : base(defaultHandle, defaultValue, onValueChanged, onValueChangedKV, ascending)
        {

        }
    }


}


