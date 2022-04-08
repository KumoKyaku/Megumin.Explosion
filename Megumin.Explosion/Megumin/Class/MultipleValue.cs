using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 复合值，一个值的某些构成是有限时间的。
    /// 和IMultipleControlable很像，但是不排序，是求和。
    /// <para></para>
    /// 需求一：最初需求是被弹反给自己增加一个被弹反计数，这一个有效期5秒，如果计数大于3，进入被弹反硬直。
    /// 这导致弹反计数中，每个弹反点有自己的生命周期，计算总和十分麻烦。
    /// <para></para>
    /// 需求二：护盾由各种护盾构成，蓝盾黄甲，算是生命值，每个构成项可以被消耗。
    /// 如果HP500，蓝盾200，黄甲300，遇到 350点伤害，消耗掉蓝盾100，黄甲100，HP150。
    /// <para></para>
    /// 需求三：仇恨与威胁值系统，WOW中一个节能产生的仇恨是有时间的，过了时间会消退，和需求一同理。
    /// 如果通过buff实现，buff数量暴增。
    /// 计时器是可以实现的，但是容易出bug，对象已经被销毁等。中途整个威胁值已经被重置等，先加后减问题非常多。
    /// 当前思路是给每个构成项一个key，减去移除的时候识别这个key是不是有效。如果已经移除过或整体重置，不会重复减去。
    /// </summary>
    public abstract class MultipleValue<K, V> : Multiple<K, V>
    {
        protected readonly K defaultKey;
        protected readonly V defaultValue;
        public override K DefaultKey => defaultKey;
        public override V DefaultValue => defaultValue;

        public MultipleValue() { }
        public MultipleValue(K defaultKey,
                             V defaultValue = default,
                             OnValueChanged<V> onValueChanged = null)
        {
            this.defaultKey = defaultKey;
            this.defaultValue = defaultValue;
            if (onValueChanged != null)
            {
                this.ValueChanged += onValueChanged;
            }
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class IntMultipleValue<K> : MultipleValue<K, int>
    {
        public IntMultipleValue() { }

        public IntMultipleValue(K defaultKey, int defaultValue = default)
            : base(defaultKey, defaultValue)
        {
        }

        protected override (K Key, int Value) CalNewValue()
        {
            K key = default;
            int value = 0;
            foreach (var item in ElementDic)
            {
                key = item.Key;
                value += item.Value;
            }
            return (key, value);
        }
    }

    /// <summary>
    /// 用于FlagEnum,最好用枚举类型实现一次.
    /// 性能有些额外损失,至少每次计算都要Tostring,甚至装箱一次,不确定.<see cref="CalNewValue"/>.
    /// </summary>
    /// <inheritdoc/>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MultipleValueEnum<K, V> : MultipleValue<K, V>
        where V : struct, Enum, IConvertible
    {
        public MultipleValueEnum(K defaultKey,
                                   V defaultValue,
                                   OnValueChanged<V> onValueChanged = null)
            : base(defaultKey, defaultValue, onValueChanged)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override (K Key, V Value) CalNewValue()
        {
            int temp = DefaultValue.ToInt32(null);

            foreach (var item in ElementDic)
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
    public sealed class MultipleValueEnumKeypadSudoku : MultipleValueEnum<object, KeypadSudoku>
    {
        public MultipleValueEnumKeypadSudoku(object defaultKey,
                                           KeypadSudoku defaultValue,
                                           OnValueChanged<KeypadSudoku> onValueChanged = null)
            : base(defaultKey, defaultValue, onValueChanged)
        {
        }

        protected override (object Key, KeypadSudoku Value) CalNewValue()
        {
            var V = DefaultValue;
            foreach (var item in ElementDic)
            {
                V |= item.Value;
            }

            return (null, V);
        }
    }

    /// <summary>
    /// 并集.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MultipleValueCollection<K, V> : MultipleValue<K, ICollection<V>>
    {
        public MultipleValueCollection(K defaultKey,
                                         ICollection<V> defaultValue,
                                         OnValueChanged<ICollection<V>> onValueChanged = null)
            : base(defaultKey, defaultValue, onValueChanged)
        {

        }

        public HashSet<V> AllElement { get; set; } = new HashSet<V>();

        /// <summary>
        /// 去并集
        /// </summary>
        /// <returns></returns>
        protected override (K Key, ICollection<V> Value) CalNewValue()
        {
            AllElement.Clear();
            foreach (var item in ElementDic)
            {
                foreach (var ele in item.Value)
                {
                    AllElement.Add(ele);
                }
            }
            return (default, AllElement);
        }
    }
}


