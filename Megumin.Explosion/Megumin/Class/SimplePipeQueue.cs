using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 异步缓存管道
    /// <para/>也可以通过（bool isEnd,T value）元组，来实现终止信号
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimplePipeQueue<T> : Queue<T>, IPipeQueue<T>
    {
        readonly object _innerLock = new object();
        private TaskCompletionSource<T> source;

        //线程同步上下文由Task机制保证，无需额外处理
        //SynchronizationContext callbackContext;
        //public bool UseSynchronizationContext { get; set; } = true;

        public virtual void Write(T item)
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

                    var next = source;
                    source = null;
                    next.TrySetResult(item);
                }
            }
        }

        public virtual ValueTask<T> ReadAsync()
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
                    source = new TaskCompletionSource<T>();
                    return new ValueTask<T>(source.Task);
                }
            }
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimplePipeQueueWithEndSignal<T> : SimplePipeQueue<(T Value, bool IsEnd)>
    {
        //public bool IsEnd { get; private set; }

        public void Write(T item, bool isEnd = false)
        {
            Write((item, isEnd));
        }

        //public override async ValueTask<(T Value, bool IsEnd)> ReadAsync()
        //{
        //    if (IsEnd)
        //    {
        //        throw new IndexOutOfRangeException();
        //    }
        //    var ret = await base.ReadAsync();
        //    IsEnd = ret.IsEnd;
        //    return ret;
        //}
    }

}
