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
        /// <summary>
        /// 检测两个对象是否可以匹配
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        /// <remarks>匹配是单向的，交换参数位置可能导致结果不同</remarks>
        bool Match(T lhs, K rhs);
    }

    /// <summary>
    /// 可匹配的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMatchable<in T>
    {
        /// <summary>
        /// 目标对象是否与自身匹配
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <remarks>匹配是单向的，即使目标与自身匹配，自身不一定与目标匹配</remarks>
        bool Match(T target);
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
    public interface INode<V> : INode
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
    public interface ISlot<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        V this[K key] { get; set; }
    }

    /// <summary>
    /// 含有可见性的
    /// </summary>
    public interface IVisibleable
    {
        bool Visible { get; set; }
    }

    /// <summary>
    /// 含有可见性等级的
    /// </summary>
    public interface IVisibleLevelable
    {
        /// <summary>
        /// 可见等级
        /// </summary>
        int VisibleLevel { get; set; }
    }

    /// <summary>
    /// 含有版本的
    /// </summary>
    public interface IVersionable
    {
        /// <summary>
        /// 当前版本
        /// </summary>
        Version Version { get; }
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
    //兼容Unity设计的一组接口
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

    /// <summary>
    /// F1帮助相关
    /// </summary>
    public interface IF1able
    {
        void OnF1(object option = null);
    }

    /// <summary>
    /// todo 调试相关
    /// </summary>
    public interface IDebugable
    {
        void OnDebugKey(ConsoleKey key = ConsoleKey.F1);
    }
}
