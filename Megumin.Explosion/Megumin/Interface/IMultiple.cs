using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 对象有多个部分构成。
    /// <para/> 根据一个或多个值，得到一个结果值。多个部分可能随时添加或者移除
    /// <para/> 运算规则可能为排序，求和，均值等多种算法，用于不同业务场景。
    /// <para/> 例如：是否静音由多个业务模块同时控制。最大血量可能由多个装备求和。
    /// </summary>
    public interface IMultiple<K, V>
    {
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
        /// 当前值的Key,可能为无效值,看计算方式.
        /// </summary>
        K CurrentKey { get; }

        /// <summary>
        /// 当前值
        /// </summary>
        V Current { get; }

        /// <summary>
        /// 添加一个构成项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="forceRaiseEvent"></param>
        V Add(K key, V value, bool forceRaiseEvent = false);

        /// <summary>
        /// 移除一个构成项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">因为总是复制粘贴Add,参数个数对不上,这个值没有使用,只是为了对齐参数个数.</param>
        /// <param name="forceRaiseEvent"></param>
        V Remove(K key, V value = default, bool forceRaiseEvent = false);

        /// <summary>
        /// 取消除默认值以外的所有构成项
        /// </summary>
        V RemoveAll(bool forceRaiseEvent = false);

        /// <summary>
        /// 仅当值发生改变时被调用
        /// </summary>
        event OnValueChanged<V> ValueChanged;
        /// <summary>
        /// 仅当值发生改变时被调用
        /// </summary>
        event OnValueChanged<(K Key, V Value)> ValueChangedKV;
        /// <summary>
        /// 当前键值任一发生改变时被调用
        /// </summary>
        event OnValueChanged<(K Key, V Value)> KeyOrValueChanged;
    }
}
