using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Megumin
{
    public class SimplePipeQueue<T> : Queue<T>, IPipeQueue<T>
    {
        readonly object _innerLock = new object();
        private TaskCompletionSource<T> source;
        SynchronizationContext callbackContext;
        public bool UseSynchronizationContext { get; set; } = true;

        /// <summary>
        /// 当消费者已经等待时，如果不使用<see cref="IPipeQueue{T}.UseSynchronizationContext"/>,或者当前线程和等待线程相同，立刻调用消费者。
        /// </summary>
        /// <param name="item"></param>
        public void Write(T item)
        {
            lock (_innerLock)
            {
                if (source == null)
                {
                    Enqueue(item);
                }
                else
                {
                    if (Count > 0)
                    {
                        throw new Exception("内部顺序错误，不应该出现，请联系作者");
                    }

                    if (callbackContext == null || callbackContext == System.Threading.SynchronizationContext.Current)
                    {
                        var next = source;
                        source = null;
                        next.TrySetResult(item);
                    }
                    else
                    {
                        var next = source;
                        source = null;
                        callbackContext.Post(
                            state =>
                            {
                                if (state is TaskCompletionSource<T> callback)
                                {
                                    callback.TrySetResult(item);
                                }
                            }, next);
                    }
                }
            }
        }

        public ValueTask<T> ReadAsync()
        {
            lock (_innerLock)
            {
                if (this.Count > 0)
                {
                    var next = Dequeue();
                    return new ValueTask<T>(next);
                }
                else
                {
                    if (UseSynchronizationContext)
                    {
                        callbackContext = SynchronizationContext.Current;
                    }
                    else
                    {
                        callbackContext = null;
                    }

                    source = new TaskCompletionSource<T>();
                    return new ValueTask<T>(source.Task);
                }
            }
        }
    }
}
