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
        /// 当队列中有元素时返回。
        /// </summary>
        /// <returns></returns>
        Task<T> ReadAsync();
        void Write(T item);
    }

    /// <summary>
    /// <inheritdoc cref="IPipe{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
