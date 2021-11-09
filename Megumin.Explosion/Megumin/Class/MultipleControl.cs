﻿using System;
using System.Collections.Generic;
using System.Linq;

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
    public interface IMultipleControlable<K, V>
    {
        /// <summary>
        /// 当前值
        /// </summary>
        V Current { get; }
        /// <summary>
        /// 当前值的Key,可能为无效值,看计算方式.
        /// </summary>
        K CurrentKey { get; }

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
        /// 值发生改变
        /// </summary>
        event OnValueChanged<V> ValueChanged;

        /// <summary>
        /// 值发生改变
        /// </summary>
        event OnValueChanged<(K, V)> ValueChangedKV;

        /// <summary>
        /// 开始控制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        V Control(K key, V value);
        /// <summary>
        /// 取消控制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">因为总是复制粘贴Control,参数个数对不上,这个值没有使用,只是为了对其参数个数.</param>
        V Cancel(K key, V value = default);
        /// <summary>
        /// 取消除默认值以外的所有控制
        /// </summary>
        V CancelAll();
    }

    /// <inheritdoc cref="IMultipleControlable{K, V}"/>
    /// <summary>
    /// 多重值控制器。
    /// [不提供默认值访问，也不保存默认值。会导致过渡设计。] 后来证明需要访问,加上了!!!
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="C">构造中使用参数类型,初始化排序缓存时使用的参数</typeparam>
    public abstract class MultipleControlBase<K, V, C> : IMultipleControlable<K, V>
    {
        /// <summary>
        /// TODO,使用最大堆最小堆优化
        /// </summary>
        protected readonly Dictionary<K, V> Controllers = new Dictionary<K, V>();
        public event OnValueChanged<V> ValueChanged;
        public event OnValueChanged<(K, V)> ValueChangedKV;
        protected readonly K defaultKey;
        protected readonly V defaultValue;
        public K DefaultKey => defaultKey;
        public V DefaultValue => defaultValue;
        public V Current { get; protected set; }
        public K CurrentKey { get; protected set; }

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
                               OnValueChanged<(K, V)> onValueChangedKV = null,
                               OnValueChanged<V> onValueChanged = null,
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
        /// 构造函数中间调用的虚方法,用于初始化排序字段,在第一次<see cref="ApplyValue"/>前调用.
        /// <para>不需要排序的或者计算方式不需要缓存的,可以忽略这个函数.</para>
        /// 这个主要作用用来初始化排序的linq表达式.
        /// </summary>
        protected virtual void InitInCtor(C init)
        {

        }


        public V Control(K key, V value)
        {
            Controllers[key] = value;
            ApplyValue();
            return Current;
        }


        public V Cancel(K key, V value = default)
        {
            Controllers.Remove(key);
            ApplyValue();
            return Current;
        }

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
            var result = CalNewValue();
            Current = result.Value;
            CurrentKey = result.Key;

            if (!old.Equals(result))
            {
                OnValueChanged(Current, old);
            }

            if (!old.Equals(result) || !oldK.Equals(CurrentKey))
            {
                OnValueChangedKV((CurrentKey, Current), (oldK, old));
            }
        }

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

        /// <summary>
        /// 计算新的值, 返回值也可以用KeyValuePair,没什么区别.
        /// </summary>
        /// <returns></returns>
        /// <remarks>有的多重控制并不是比较大小,例如FlagEnum,可能时其他运算</remarks>
        protected abstract (K Key, V Value) CalNewValue();

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
        /// 
        /// </summary>
        /// <param name="defaultKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="onValueChangedKV"></param>
        /// <param name="onValueChanged"></param>
        /// <param name="ascending">true 按升序排列，结果为应用最小值，false为降序排列，结果为应用最大值。</param>
        public MultipleControl(K defaultKey,
                               V defaultValue,
                               OnValueChanged<(K, V)> onValueChangedKV = null,
                               OnValueChanged<V> onValueChanged = null,
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

    ///<inheritdoc/>
    [Obsolete("Use MultipleControl<K, V> instead.")]
    public class MultipleControl<V> : MultipleControl<object, V>
        where V : IEquatable<V>
    {
        public MultipleControl(object defaultHandle,
                               V defaultValue,
                               OnValueChanged<(object, V)> onValueChangedKV = null,
                               OnValueChanged<V> onValueChanged = null,
                               bool ascending = true)
            : base(defaultHandle, defaultValue, onValueChangedKV, onValueChanged, ascending)
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
        /// <param name="onValueChangedKV"></param>
        /// <param name="onValueChanged"></param>
        /// <param name="ascending">默认false,任意一个开启就开启,true,任意一个关闭就关闭.</param>
        public ActiveControl(object defaultHandle,
                             bool defaultValue,
                             OnValueChanged<(object, bool)> onValueChangedKV = null,
                             OnValueChanged<bool> onValueChanged = null,
                             bool ascending = false)
            : base(defaultHandle, defaultValue, onValueChangedKV, onValueChanged, ascending)
        {

        }
    }

    /// <summary>
    /// 用于FlagEnum,最好用枚举类型实现一次.
    /// 性能有些额外损失,至少每次计算都要Tostring,甚至装箱一次,不确定.<see cref="CalNewValue"/>.
    /// </summary>
    /// <inheritdoc/>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MultipleControlEnum<K, V> : MultipleControlBase<K, V, bool>
        where V : struct, Enum, IConvertible
    {
        public MultipleControlEnum(K defaultKey,
                                   V defaultValue,
                                   OnValueChanged<(K, V)> onValueChangedKV = null,
                                   OnValueChanged<V> onValueChanged = null,
                                   bool init = false) : base(defaultKey, defaultValue, onValueChangedKV, onValueChanged, init)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override (K Key, V Value) CalNewValue()
        {
            int temp = DefaultValue.ToInt32(null);

            foreach (var item in Controllers)
            {
                var b = item.Value.ToInt32(null);
                temp |= b;
            }

            var res = (V)Enum.Parse(typeof(V), temp.ToString());
            return (default, res);
        }

        /// <summary>
        /// https://github.com/dotnet/runtime/issues/14084
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enum"></param>
        /// <param name="flag"></param>
        /// <param name="on"></param>
        /// <returns></returns>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //internal static T SetFlag<T>(this T @enum, T flag, bool on) where T : struct, Enum
        //{
        //    if (Unsafe.SizeOf<T>() == 1)
        //    {
        //        byte x = (byte)((Unsafe.As<T, byte>(ref @enum) & ~Unsafe.As<T, byte>(ref flag))
        //            | (-Unsafe.As<bool, byte>(ref on) & Unsafe.As<T, byte>(ref flag)));
        //        return Unsafe.As<byte, T>(ref x);
        //    }
        //    else if (Unsafe.SizeOf<T>() == 2)
        //    {
        //        var x = (short)((Unsafe.As<T, short>(ref @enum) & ~Unsafe.As<T, short>(ref flag))
        //            | (-Unsafe.As<bool, byte>(ref on) & Unsafe.As<T, short>(ref flag)));
        //        return Unsafe.As<short, T>(ref x);
        //    }
        //    else if (Unsafe.SizeOf<T>() == 4)
        //    {
        //        uint x = (Unsafe.As<T, uint>(ref @enum) & ~Unsafe.As<T, uint>(ref flag))
        //           | ((uint)-Unsafe.As<bool, byte>(ref on) & Unsafe.As<T, uint>(ref flag));
        //        return Unsafe.As<uint, T>(ref x);
        //    }
        //    else
        //    {
        //        ulong x = (Unsafe.As<T, ulong>(ref @enum) & ~Unsafe.As<T, ulong>(ref flag))
        //           | ((ulong)-(long)Unsafe.As<bool, byte>(ref on) & Unsafe.As<T, ulong>(ref flag));
        //        return Unsafe.As<ulong, T>(ref x);
        //    }
        //}
    }

    /// <summary>
    /// 枚举示例<see cref="CalNewValue"/>. FlagEnum排序没有意义,使用|运算.
    /// </summary>
    public sealed class MultipleControlKeypadSudoku : MultipleControlBase<object, KeypadSudoku, bool>
    {
        public MultipleControlKeypadSudoku(object defaultKey,
                                           KeypadSudoku defaultValue,
                                           OnValueChanged<(object, KeypadSudoku)> onValueChangedKV = null,
                                           OnValueChanged<KeypadSudoku> onValueChanged = null,
                                           bool init = false)
            : base(defaultKey, defaultValue, onValueChangedKV, onValueChanged, init)
        {
        }

        protected override (object Key, KeypadSudoku Value) CalNewValue()
        {
            var V = DefaultValue;
            foreach (var item in Controllers)
            {
                V |= item.Value;
            }

            return (null, V);
        }
    }

    /// <summary>
    /// 并集. //TODO,没啥办法判断值改变,索性每次计算都调用回调.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MultipleControlCollection<K, V> : MultipleControlBase<K, ICollection<V>, bool>
    {
        HashSet<V> keys = new HashSet<V>();
        /// <summary>
        /// TODO,控制交集并集
        /// </summary>
        bool func = false;
        public MultipleControlCollection(K defaultKey,
                                         ICollection<V> defaultValue,
                                         OnValueChanged<(K, ICollection<V>)> onValueChangedKV = null,
                                         OnValueChanged<ICollection<V>> onValueChanged = null,
                                         bool init = false) : base(defaultKey, defaultValue, onValueChangedKV, onValueChanged, init)
        {

        }

        protected override void InitInCtor(bool init)
        {
            func = init;
        }

        protected override void ApplyValue()
        {
            var old = Current;
            var oldK = CurrentKey;
            var result = CalNewValue();
            Current = result.Value;
            CurrentKey = result.Key;

            //TODO,没啥办法判断值改变,索性每次计算都调用回调.
            OnValueChanged(Current, old);
            OnValueChangedKV((CurrentKey, Current), (oldK, old));
        }

        /// <summary>
        /// 去并集
        /// </summary>
        /// <returns></returns>
        protected override (K Key, ICollection<V> Value) CalNewValue()
        {
            keys.Clear();
            foreach (var item in Controllers)
            {
                foreach (var ele in item.Value)
                {
                    keys.Add(ele);
                }
            }
            return (default, keys);
        }
    }
}


