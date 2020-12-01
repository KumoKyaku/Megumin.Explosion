using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 管道队列，用于处理生产者已经产出，消费者还没有就绪的情况
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPipeQueue<T>
    {
        /// <summary>
        /// 当队列中有元素时返回。
        /// </summary>
        /// <returns></returns>
        ValueTask<T> ReadAsync();
        void Write(T item);
    }
}
