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
    [Obsolete("Use QueuePipe instead.", true)]
    public class SimplePipeQueue<T> : Queue<T>, IValuePipe<T>
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
    [Obsolete("Use QueueSignalPipe instead.", true)]
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

    /// <summary>
    /// <inheritdoc cref="IPipe{T}"/>
    /// <para></para>这是个简单的实现,更复杂使用微软官方实现<see cref="System.Threading.Channels.Channel.CreateBounded{T}(int)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueuePipe<T> : Queue<T>, IPipe<T>
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

        public new void Enqueue(T item)
        {
            lock (_innerLock)
            {
                base.Enqueue(item);
            }
        }

        public void Flush()
        {
            lock (_innerLock)
            {
                if (Count > 0)
                {
                    var res = Dequeue();
                    var next = source;
                    source = null;
                    next?.TrySetResult(res);
                }
            }
        }

        public virtual Task<T> ReadAsync()
        {
            lock (_innerLock)
            {
                if (this.Count > 0)
                {
                    var next = Dequeue();
                    return Task.FromResult(next);
                }
                else
                {
                    source = new TaskCompletionSource<T>();
                    return source.Task;
                }
            }
        }

        public ValueTask<T> ReadValueTaskAsync()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueSignalPipe<T> : QueuePipe<(T Value, bool IsEnd)>
    {
        public void Write(T item, bool isEnd = false)
        {
            Write((item, isEnd));
        }
    }

    /// <summary>
    /// <inheritdoc cref="IPipe{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete("没有存在意义,后续版本删除")]
    public class StackPipe<T> : Stack<T>, IPipe<T>
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
                    Push(item);
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

        public void Enqueue(T item)
        {
            lock (_innerLock)
            {
                base.Push(item);
            }
        }

        public void Flush()
        {
            lock (_innerLock)
            {
                if (Count > 0)
                {
                    var res = Pop();
                    var next = source;
                    source = null;
                    next?.TrySetResult(res);
                }
            }
        }

        public virtual Task<T> ReadAsync()
        {
            lock (_innerLock)
            {
                if (this.Count > 0)
                {
                    var next = Pop();
                    return Task.FromResult(next);
                }
                else
                {
                    source = new TaskCompletionSource<T>();
                    return source.Task;
                }
            }
        }

        public ValueTask<T> ReadValueTaskAsync()
        {
            throw new NotImplementedException();
        }
    }
}
