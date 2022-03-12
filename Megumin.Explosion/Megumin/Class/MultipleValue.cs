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
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class IntMultipleValue<K> : MultipleValue<K, int>
    {
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
}


