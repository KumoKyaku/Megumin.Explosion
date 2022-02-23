using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 使用异步代替一次性回调，免手动清理注册。
    /// <para/>触发一次，清除所有注册。
    /// <para/>弊端，如果不触发，永久保存引用，无法主动清除，容易内存泄漏。
    /// <para/>适用于短生命周期对象。
    /// <para/>谨慎使用。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OneoffEvent<T>
    {
        public Task<T> Task
        {
            get
            {
                if (Source == null)
                {
                    Source = new TaskCompletionSource<T>();
                }
                return Source.Task;
            }
        }

        public TaskCompletionSource<T> Source { get; private set; }

        public void Invoke(T result)
        {
            var soure = Source;
            Source = null;
            soure?.TrySetResult(result);
        }

        /// <summary>
        /// 会清除所有注册的事件，不能清除指定事件。
        /// </summary>
        public void UnRegistAll()
        {
            Source = null;
            //todo,怎么才能将task中已经等待的指定回调移除？
            //Source.Task.GetAwaiter();
        }
    }
}
