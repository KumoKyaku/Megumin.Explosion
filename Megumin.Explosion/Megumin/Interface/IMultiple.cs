using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 对象 或者 抽象概念 由多个部分构成。
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
        /// <param name="raiseEvent">Ignore = -1,Force:101, <seealso cref="RaiseEvent"/></param>
        V Add(K key, V value, int raiseEvent = 0);

        /// <summary>
        /// 移除一个构成项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">因为总是复制粘贴Add,参数个数对不上,这个值没有使用,只是为了对齐参数个数.</param>
        /// <param name="raiseEvent">Ignore = -1,Force:101, <seealso cref="RaiseEvent"/></param>
        V Remove(K key, V value = default, int raiseEvent = 0);

        /// <summary>
        /// 取消除默认值以外的所有构成项
        /// </summary>
        /// <param name="raiseEvent">Ignore = -1,Force:101, <seealso cref="RaiseEvent"/></param>
        /// <returns></returns>
        V RemoveAll(int raiseEvent = 0);

        /// <summary>
        /// 取消除默认值以外的所有符合条件的构成项
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="raiseEvent">Ignore = -1,Force:101, <seealso cref="RaiseEvent"/></param>
        /// <returns></returns>
        V RemoveAll(Func<KeyValuePair<K, V>, bool> predicate, int raiseEvent = 0);

        /// <summary>
        /// 在控制项没有变动的情况下，触发ApplyValue，尝试触发事件。
        /// </summary>
        /// <param name="raiseEvent">Ignore = -1,Force:101, <seealso cref="RaiseEvent"/></param>
        void Refresh(int raiseEvent = 0);

        /// <summary>
        /// 仅当值发生改变时被调用
        /// </summary>
        event OnChanged<V> ValueChanged;
        /// <summary>
        /// 仅当值发生改变时被调用
        /// </summary>
        event OnChanged<(K Key, V Value)> ValueChangedKV;
        /// <summary>
        /// 当前键值任一发生改变时被调用
        /// </summary>
        event OnChanged<(K Key, V Value)> KeyOrValueChanged;
    }
}
