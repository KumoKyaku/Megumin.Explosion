using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 匹配器
    /// </summary>
    /// <typeparam name="T"><peparam>
    /// <typeparam name="K"><peparam>
    public interface IMatcher<in T, in K>
    {
        bool Match(T input, K target);
    }

    /// <summary>
    /// 节点
    /// </summary>
    public interface INode
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public interface INode<V>:INode
    {
        V NodeValue { get; set; }
    }

    /// <summary>
    /// 插槽
    /// </summary>
    public interface ISlot
    {

    }

    public interface ISlot<V>
    {
        V SlotValue { get; set; }
    }

    /// <summary>
    /// 插槽
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public interface ISlot<K, V>:IEnumerable<KeyValuePair<K,V>>
    {
        V this[K key] { get;set; }
    }
}
