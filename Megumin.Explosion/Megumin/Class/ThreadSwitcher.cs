using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// <inheritdoc cref="ThreadSwitcher"/>
    /// </summary>
    public static class DefaultThreadSwitcher
    {
        static readonly ThreadSwitcher Default = new ThreadSwitcher();

        /// <summary>
        /// <inheritdoc cref="ThreadSwitcher.Tick"/>
        /// </summary>
        public static void Tick()
        {
            Default.Tick();
        }

        /// <summary>
        /// <inheritdoc cref=" ThreadSwitcher.Switch(Action)"/>
        /// </summary>
        /// <param name="action"></param>
        public static void Switch(Action action)
        {
            Default.Switch(action);
        }

        /// <summary>
        /// <inheritdoc cref="ThreadSwitcher.Switch()"/>
        /// </summary>
        /// <returns></returns>
        public static ThreadSwitcher.SwitcherSource Switch()
        {
            return Default.Switch();
        }

        /// <inheritdoc cref="ThreadSwitcher.Switch1(int)"/>
        [Obsolete("Use Switch instead. 严重bug,无法实现预定功能. 无法保证先await 后 Tick.", true)]
        public static ConfiguredValueTaskAwaitable Switch1(int safeMillisecondsDelay = 0)
        {
            return Default.Switch1(safeMillisecondsDelay);
        }

        /// <summary>
        /// <inheritdoc cref="ThreadSwitcher.Switch3MaxWait"/>
        /// </summary>
        /// <returns></returns>
        public static ThreadSwitcher.SwitcherSource3 Switch3MaxWait(int maxWaitMilliseconds = 100)
        {
            return Default.Switch3MaxWait(maxWaitMilliseconds);
        }
    }

    /// <summary>
    /// 通用线程切换器
    /// <para/>异步后续在<see cref="Tick"/>线程调用。
    /// <para/>通用性高,但是用到TaskCompletionSource和异步各种中间对象和异步机制.
    /// <para/>性能开销大,不如明确的类型和回调接口<see cref="Switch(Action)"/>.
    /// </summary>
    public partial class ThreadSwitcher
    {

        /// <summary>
        /// 由指定线程调用,回调其他线程需要切换到这个线程的方法
        /// </summary>
        public virtual void Tick()
        {
            while (actions.TryDequeue(out var callback))
            {
                callback?.Invoke();
            }

            TickWaitQueue();

            TickWaitQueue1();

            TickWaitQueue3();
        }

        protected readonly ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();

        /// <summary>
        /// 切换执行线程
        /// <seealso cref="Switch()"/>
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>线程切换过程中闭包几乎无法避免, 除非明确回调函数参数类型.
        /// <para>对于性能敏感区域,可以写特定代码消除闭包. 参考 megumin.net</para>
        /// </remarks>
        public void Switch(Action action)
        {
            actions.Enqueue(action);
        }
    }

    public partial class ThreadSwitcher
    {
        /// <summary>
        /// 切换线程专用Soucre，不建议保留引用，请直接await。
        /// </summary>
        public class SwitcherSource : TaskCompletionSource<int>
        {
            public ConcurrentQueue<SwitcherSource> WaitQueue { get; internal protected set; }

            public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
            {
                private SwitcherSource source;

                public Awaiter(SwitcherSource source)
                {
                    this.source = source;
                }

                public bool IsCompleted
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get
                    {
                        return source.Task.IsCompleted;
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                [System.Diagnostics.DebuggerHidden]
                public void GetResult()
                {
                    source.Task.GetAwaiter().GetResult();
                }

                public void UnsafeOnCompleted(Action continuation)
                {
                    source.Task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(continuation);
                    source.WaitQueue.Enqueue(source);
                }

                public void OnCompleted(Action continuation)
                {
                    throw new NotImplementedException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Awaiter GetAwaiter()
            {
                return new Awaiter(this);
            }
        }

        protected readonly ConcurrentQueue<SwitcherSource> WaitQueue = new ConcurrentQueue<SwitcherSource>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void TickWaitQueue()
        {
            while (WaitQueue.TryDequeue(out var res))
            {
                res.TrySetResult(0);
            }
        }

        /// <summary>
        /// 使用特殊异步source切换线程。
        /// <para/>source只有await 执行后，才会入队<see cref="WaitQueue"/>
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        public SwitcherSource Switch()
        {
            var source = new SwitcherSource();
            source.WaitQueue = WaitQueue;
            return source;
        }
    }

    public partial class ThreadSwitcher
    {

        /// <summary>
        /// 可以合并Source来提高性能,但是会遇到异步后续出现异常的情况,比较麻烦.
        /// 所以每个Switch调用处使用不同的source,安全性更好
        /// </summary>
        protected readonly ConcurrentQueue<TaskCompletionSource<int>> WaitQueue1 = new ConcurrentQueue<TaskCompletionSource<int>>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void TickWaitQueue1()
        {
            while (WaitQueue1.TryDequeue(out var res))
            {
                res.TrySetResult(0);
            }
        }

        /// <summary>
        /// <para>保证返回值先await 后Tick, 不然 await会发现Task同步完成,无法切换线程.</para>
        /// <para><see cref="Task.Status"/>无法指示是否被await </para>
        /// </summary>
        /// <returns></returns>
        /// <remarks>BUG,无法保证先await 后Tick</remarks>
        [Obsolete("Use Switch instead. 严重bug,无法实现预定功能. 无法保证先await 后 Tick.", true)]
        public ConfiguredValueTaskAwaitable Switch1(int safeMillisecondsDelay = 0)
        {
            TaskCompletionSource<int> source = new TaskCompletionSource<int>();
            var a = new ValueTask(source.Task).ConfigureAwait(false);
            if (safeMillisecondsDelay > 0)
            {
                DelayEnqueue1(safeMillisecondsDelay, source);
            }
            else
            {
                WaitQueue1.Enqueue(source);
            }

            return a;
        }

        protected async void DelayEnqueue1(int safeMillisecondsDelay, TaskCompletionSource<int> source)
        {
            await Task.Delay(safeMillisecondsDelay).ConfigureAwait(false);
            WaitQueue1.Enqueue(source);
        }
    }

    public partial class ThreadSwitcher
    {
        /// <summary>
        /// 切换线程专用Soucre，不要保留引用，请直接await。
        /// </summary>
        public class SwitcherSource3 : TaskCompletionSource<int>
        {
            /// <summary>
            /// 表示调用线程已经进入异步await
            /// </summary>
            public bool IsAwaiting { get; internal protected set; } = false;
            /// <summary>
            /// 最大等待轮询时间，超出这个时间将不在同步完成。失去切换线程的作用。
            /// </summary>
            public long MaxWaitMilliseconds { get; internal protected set; } = 100;
            internal protected long? WaitTickTime = null;

            public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
            {
                private SwitcherSource3 source;

                public Awaiter(SwitcherSource3 source)
                {
                    this.source = source;
                }

                public bool IsCompleted
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get
                    {
                        return source.Task.IsCompleted;
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                [System.Diagnostics.DebuggerHidden]
                public void GetResult()
                {
                    source.Task.GetAwaiter().GetResult();
                }

                public void UnsafeOnCompleted(Action continuation)
                {
                    source.Task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(continuation);
                    source.IsAwaiting = true;
                }

                public void OnCompleted(Action continuation)
                {
                    throw new NotImplementedException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Awaiter GetAwaiter()
            {
                return new Awaiter(this);
            }
        }
        protected readonly LinkedList<SwitcherSource3> WaitQueue3 = new LinkedList<SwitcherSource3>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void TickWaitQueue3()
        {
            var current = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var node = WaitQueue3.First;
            while (node != null)
            {
                if (node.Value.IsAwaiting)
                {
                    node.Value.TrySetResult(0);
                }
                else
                {
                    if (node.Value.WaitTickTime.HasValue)
                    {
                        var delta = current - node.Value.WaitTickTime.Value;
                        if (delta > node.Value.MaxWaitMilliseconds)
                        {
                            //保护措施，如果一个线程切换申请一直没有await，就忽略它/提前完成它。
                            node.Value.TrySetResult(-1);
                        }
                    }
                    else
                    {
                        node.Value.WaitTickTime = current;
                    }
                }

                var next = node.Next;
                if (node.Value.Task.IsCompleted)
                {
                    WaitQueue3.Remove(node);
                }
                node = next;
            }

        }
        /// <summary>
        /// 使用特殊异步source切换线程，请直接await。
        /// 如果没有await，经过<paramref name="maxWaitMilliseconds"/>后将会被完成，失去切换线程的作用，
        /// 但不会阻塞线程切换器。
        /// <para/>source会立即入队<see cref="WaitQueue3"/>
        /// </summary>
        /// <returns></returns>
        public SwitcherSource3 Switch3MaxWait(int maxWaitMilliseconds = 100)
        {
            var source = new SwitcherSource3();
            source.MaxWaitMilliseconds = maxWaitMilliseconds;
            WaitQueue3.AddLast(source);
            return source;
        }
    }


}


