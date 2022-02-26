using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        internal protected Task<T> Task
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

        internal protected TaskCompletionSource<T> Source { get; private set; }

        readonly object _innerLock = new object();

        public void Invoke(T result)
        {
            lock (_innerLock)
            {
                var soure = Source;
                soure?.TrySetResult(result);
                Source = null;
            }
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter GetAwaiter()
        {
            lock (_innerLock)
            {
                return Task.ConfigureAwait(false).GetAwaiter();
            }
        }
    }

    /// <summary>
    /// 弱引用一次性事件. TODO,弱引用没起作用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OneoffEventWeak<T>
    {
        internal protected TaskCompletionSource<T> Source { get; private set; }

        public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            public bool IsCompleted
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return Source.Task.IsCompleted;
                }
            }

            internal TaskCompletionSource<T> Source { get; set; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.DebuggerHidden]
            public T GetResult()
            {
                return Source.Task.GetAwaiter().GetResult();
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                WeakReference weak = new WeakReference(continuation);
                Source.Task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(() =>
                {
                    if (weak.IsAlive)
                    {
                        if (weak.Target is Action action && action.Target != null)
                        {
                            action?.Invoke();
                        }
                    }
                });
            }

            public void OnCompleted(Action continuation)
            {
                throw new NotImplementedException();
            }
        }

        readonly object _innerLock = new object();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Awaiter GetAwaiter()
        {
            lock (_innerLock)
            {
                if (Source == null)
                {
                    Source = new TaskCompletionSource<T>();
                }

                var a = new Awaiter
                {
                    Source = Source
                };
                return a;
            }
        }

        public void Invoke(T result)
        {
            lock (_innerLock)
            {
                var soure = Source;
                soure?.TrySetResult(result);
                Source = null;
            }
        }

        async void Test()
        {
            await this;
        }

    }

}
