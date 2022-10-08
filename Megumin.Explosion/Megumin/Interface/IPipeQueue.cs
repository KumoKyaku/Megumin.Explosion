using System;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 管道队列，用于处理生产者已经产出，消费者还没有就绪的情况
    /// <para/>也可以通过（bool isEnd,T value）元组，来实现终止信号
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// 线程同步上下文由Task机制保证，无需额外处理
    /// SynchronizationContext callbackContext;
    /// public bool UseSynchronizationContext { get; set; } = true;
    /// </remarks>
    public interface IPipe<T>
    {
        /// <summary>
        /// 入队，但是不触发异步回调
        /// </summary>
        /// <param name="item"></param>
        /// <remarks>
        /// 用例：有时候生产者同时生产多个值，不想逐个触发异步回调，就需要多个值都Enqueue，然后一次Flush。
        /// </remarks>
        void Enqueue(T item);
        /// <summary>
        /// 尝试触发异步回调
        /// </summary>
        void Flush();

        /// <summary>
        /// 当队列中有元素时返回。
        /// </summary>
        /// <returns></returns>
        Task<T> ReadAsync();
        /// <summary>
        /// 当队列中有元素时返回。
        /// </summary>
        /// <returns></returns>
        ValueTask<T> ReadValueTaskAsync();
        /// <summary>
        /// 写入，同时触发异步回调
        /// </summary>
        /// <param name="item"></param>
        void Write(T item);
    }

    /// <summary>
    /// <inheritdoc cref="IPipe{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete]
    public interface IValuePipe<T>
    {
        /// <summary>
        /// 当队列中有元素时返回。
        /// </summary>
        /// <returns></returns>
        ValueTask<T> ReadAsync();
        void Write(T item);
    }
}
