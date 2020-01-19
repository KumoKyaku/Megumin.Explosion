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
    /// 路径
    /// </summary>
    public interface IPath
    {

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

    public interface IVisible
    {
        bool Visible { get; set; }
    }

    public interface IOpenable
    {
        void Open();
    }

    public interface ICloseable
    {
        void Close();
    }

    public interface IRefreshable
    {
        void Refresh();
    }

    public interface IDirtyable
    {
        bool Dirty { get; set; }
    }

    public interface IPauseable
    {
        bool Pause { get; set; }
    }

    public interface ITickable
    {
        void Tick(double delta);
    }
}

namespace Megumin
{
    public interface IAwakeable
    {
        void Awake();
    }

    public interface IInitable
    {
        void Init();
    }

    public interface IEnable
    {
        bool Enable { get; set; }
    }

    public interface IResetable
    {
        void Reset();
    }

    public interface IStartable
    {
        void Start();
    }

    public interface IFixedUpdateable
    {
        void FixedUpdate();
    }

    public interface IUpdateable
    {
        void Update();
    }

    public interface ILateUpdateable
    {
        void LateUpdate();
    }

    public interface IDestroyable
    {
        void Destroy();
    }

    public interface IQuitable
    {
        void Quit();
    }

    public interface IExitable
    {
        void Exit();
    }
}
