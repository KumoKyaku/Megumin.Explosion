using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{

    /// <summary>
    /// 对象有多个部分构成
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
        /// 当前值
        /// </summary>
        V Current { get; }

        /// <summary>
        /// 加入一个构成项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        V Add(K key, V value);

        /// <summary>
        /// 移除一个构成项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">因为总是复制粘贴Add,参数个数对不上,这个值没有使用,只是为了对齐参数个数.</param>
        V Remove(K key, V value = default);

        /// <summary>
        /// 取消除默认值以外的所有构成项
        /// </summary>
        V RemoveAll();

        /// <summary>
        /// 值发生改变
        /// </summary>
        event OnValueChanged<V> ValueChanged;
    }
}
