using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Megumin
{
    /// <summary>
    /// 多重值控制器。
    /// [不提供默认值访问，也不保存默认值。会导致过渡设计。] 后来证明需要访问,加上了!!!
    /// </summary>
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
    /// <para></para>
    /// 用例3: 多个地方各自不同禁用某些物品使用,类型是FlagEnum,每处禁用的物品类型有重叠,
    /// 这时用一个枚举值来存共有哪些禁用,就无法实现需求.
    /// </remarks>
    public interface IMultipleControlable<K, V> : IMultiple<K, V>
    {
        /// <summary>
        /// 开始控制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="raiseEvent">Ignore = -1,Force:101, <seealso cref="RaiseEvent"/></param>
        V Control(K key, V value, int raiseEvent = 0);
        /// <summary>
        /// 取消控制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">因为总是复制粘贴Control,参数个数对不上,这个值没有使用,只是为了对其参数个数.</param>
        /// <param name="raiseEvent">Ignore = -1,Force:101, <seealso cref="RaiseEvent"/></param>
        V Cancel(K key, V value = default, int raiseEvent = 0);
        /// <summary>
        /// 取消除默认值以外的所有控制
        /// </summary>
        /// <param name="raiseEvent">Ignore = -1,Force:101, <seealso cref="RaiseEvent"/></param>
        V CancelAll(int raiseEvent = 0);
    }

    /// <inheritdoc cref="IMultipleControlable{K, V}"/>
    /// <summary>
    /// 多重值控制器。
    /// [不提供默认值访问，也不保存默认值。会导致过渡设计。] 后来证明需要访问,加上了!!!
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="C">构造中使用参数类型,初始化排序缓存时使用的参数</typeparam>
    public abstract class MultipleControlBase<K, V, C> :
        Multiple<K, V>,
        IMultipleControlable<K, V>
    {
        /// <summary>
        /// TODO,使用最大堆最小堆优化
        /// </summary>
        protected Dictionary<K, V> Controllers => ElementDic;

        protected readonly K defaultKey;
        protected readonly V defaultValue;
        public override K DefaultKey => defaultKey;
        public override V DefaultValue => defaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="onValueChangedKV"></param>
        /// <param name="onValueChanged"></param>
        /// <param name="init"></param>
        public MultipleControlBase(K defaultKey,
                               V defaultValue,
                               OnChanged<(K, V)> onValueChangedKV = null,
                               OnChanged<V> onValueChanged = null,
                               C init = default)
        {
            this.defaultKey = defaultKey;
            this.defaultValue = defaultValue;
            Controllers[defaultKey] = defaultValue;
            InitInCtor(init);

            if (onValueChanged != null)
            {
                ValueChanged += onValueChanged;
            }

            if (onValueChangedKV != null)
            {
                ValueChangedKV += onValueChangedKV;
            }

            ApplyValue();
        }

        /// <summary>
        /// 构造函数中间调用的虚方法,用于初始化排序字段,在第一次<see cref="Multiple{K, V}.ApplyValue"/>前调用.
        /// <para>不需要排序的或者计算方式不需要缓存的,可以忽略这个函数.</para>
        /// 这个主要作用用来初始化排序的linq表达式.
        /// </summary>
        protected virtual void InitInCtor(C init)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V Control(K key, V value, int raiseEvent = 0) => Add(key, value, raiseEvent);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V Cancel(K key, V value = default, int raiseEvent = 0) => Remove(key, value, raiseEvent);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V CancelAll(int raiseEvent = 0) => RemoveAll(raiseEvent);

        /// <summary>
        /// 返回当前值
        /// </summary>
        /// <param name="multipleControl"></param>
        public static implicit operator V(MultipleControlBase<K, V, C> multipleControl)
        {
            return multipleControl.Current;
        }
    }

    ///<inheritdoc/>
    public class MultipleControl<K, V> : MultipleControlBase<K, V, bool>
        where V : IEquatable<V>
    {
        /// <summary>
        /// 排序用Linq表达式
        /// </summary>
        protected IEnumerable<KeyValuePair<K, V>> SortLinqKV;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="defaultKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="onValueChangedKV"></param>
        /// <param name="onValueChanged"></param>
        /// <param name="ascending">true 按升序排列，结果为应用最小值，false为降序排列，结果为应用最大值。</param>
        public MultipleControl(K defaultKey,
                               V defaultValue,
                               OnChanged<(K, V)> onValueChangedKV = null,
                               OnChanged<V> onValueChanged = null,
                               bool ascending = true)
            : base(defaultKey, defaultValue, onValueChangedKV, onValueChanged, ascending)
        {
        }

        protected override void InitInCtor(bool init)
        {
            InitSortLinq(init);
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
        /// 计算新的值, 返回值也可以用KeyValuePair,没什么区别.
        /// </summary>
        /// <returns></returns>
        /// <remarks>有的多重控制并不是比较大小,例如FlagEnum,可能时其他运算</remarks>
        protected override (K Key, V Value) CalNewValue()
        {
            var result = SortLinqKV.FirstOrDefault();
            return (result.Key, result.Value);
        }
    }

    /// <summary>
    /// 开启控制，只要有个一个控制源为true，结果就是true.
    /// </summary>
    /// <remarks>
    /// 例如处理黑屏过渡
    /// </remarks>
    public class AnyTrueControl : MultipleControl<object, bool>
    {
        public AnyTrueControl(object defaultHandle,
                              bool defaultValue,
                              OnChanged<(object, bool)> onValueChangedKV = null,
                              OnChanged<bool> onValueChanged = null)
            : base(defaultHandle, defaultValue, onValueChangedKV, onValueChanged, false)
        {

        }

        public AnyTrueControl(OnChanged<(object, bool)> onValueChangedKV = null,
                              OnChanged<bool> onValueChanged = null)
            : this(new(), false, onValueChangedKV, onValueChanged)
        {

        }
    }

    /// <summary>
    /// 关闭控制，只要有一个控制源为false,结果就是false.
    /// </summary>
    public class AnyFalseControl : MultipleControl<object, bool>
    {
        public AnyFalseControl(object defaultHandle,
                               bool defaultValue,
                               OnChanged<(object, bool)> onValueChangedKV = null,
                               OnChanged<bool> onValueChanged = null)
            : base(defaultHandle, defaultValue, onValueChangedKV, onValueChanged, true)
        {

        }

        public AnyFalseControl(OnChanged<(object, bool)> onValueChangedKV = null,
                               OnChanged<bool> onValueChanged = null)
            : this(new(), true, onValueChangedKV, onValueChanged)
        {

        }
    }

}


