using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Megumin
{
    [Obsolete("失败品，多线程不安全，有逻辑错误", true)]
    public class ObsoleteValueTaskSource : ObsoleteValueTaskSource<bool>, IValueTaskSource
    {
        public new void GetResult(short token)
        {
            base.GetResult(token);
        }

        public void SetResult(short token)
        {
            SetResult(token, false);
        }
    }

    [Obsolete("失败品，多线程不安全，有逻辑错误", true)]
    public class ObsoleteValueTaskSource<TResult> : IValueTaskSource<TResult>
    {
        Dictionary<short, Cache> caches
             = new Dictionary<short, Cache>();

        public class Cache
        {
            public ValueTaskSourceStatus ValueTaskSourceStatus { get; internal set; }
            public short Token { get; internal set; }
            public Exception Exception { get; internal set; }
            public ManualResetEvent ManualResetEvent { get; internal set; }
            public TResult Result { get; internal set; }

            public List<(Action<object> Continuation, object State)> Continuations
                = new List<(Action<object> Continuation, object State)>();
            internal void AddContinuation(Action<object> continuation, object state)
            {
                Continuations.Add((continuation, state));
            }
        }

        public ValueTaskSourceStatus GetStatus(short token)
        {
            return Get(token).ValueTaskSourceStatus;
        }

        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            lock (caches)
            {
                var cache = Get(token);
                cache.AddContinuation(continuation, state);
            }
        }

        Cache Get(short token)
        {
            if (caches.ContainsKey(token))
            {
                return caches[token];
            }
            else
            {
                Cache cache = new Cache();
                cache.ValueTaskSourceStatus = ValueTaskSourceStatus.Pending;
                cache.Token = token;
                cache.ManualResetEvent = new ManualResetEvent(false);
                caches.Add(token, cache);
                return cache;
            }
        }

        void Next(Cache cache)
        {
            cache.ManualResetEvent.Set();

            foreach (var item in cache.Continuations)
            {
                item.Continuation?.Invoke(item.State);
            }
            cache.Continuations.Clear();
        }

        public TResult GetResult(short token)
        {
            var cache = Get(token);
            cache.ManualResetEvent.WaitOne();
            var ret = cache.ValueTaskSourceStatus;
            if (ret == ValueTaskSourceStatus.Canceled)
            {
                throw new TaskCanceledException();
            }
            else if (ret == ValueTaskSourceStatus.Faulted)
            {
                throw cache.Exception;
            }

            return cache.Result;
        }

        public void SetResult(short token, TResult result)
        {
            var cache = Get(token);
            cache.ValueTaskSourceStatus = ValueTaskSourceStatus.Succeeded;
            cache.Result = result;
            Next(cache);
        }

        public void SetCancel(short token)
        {
            var cache = Get(token);
            cache.ValueTaskSourceStatus = ValueTaskSourceStatus.Canceled;
            Next(cache);
        }

        public void SetException(short token, Exception exception)
        {
            var cache = Get(token);
            cache.ValueTaskSourceStatus = ValueTaskSourceStatus.Faulted;
            cache.Exception = exception;
            Next(cache);
        }

        /// <summary>
        /// 吃掉异步后续，释放关联引用，什么也不会发生。
        /// </summary>
        /// <param name="token"></param>
        public void EatContinuation(short token)
        {
            var cache = Get(token);
            cache.ValueTaskSourceStatus = ValueTaskSourceStatus.Succeeded;
            cache.ManualResetEvent.Set();
            cache.Continuations.Clear();
        }

        public void Reset(short token)
        {
            caches.Remove(token);
        }
    }


    /// <summary>
    /// 临时使用的IValueTaskSource，线程安全，但是没有性能优化。当作包装类使用。
    /// <para/> 不支持executioncontext 和 synchronizationcontext
    /// <para/> https://devblogs.microsoft.com/pfxteam/executioncontext-vs-synchronizationcontext/
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class TempValueTaskSource<TResult> : IValueTaskSource<TResult>
    {
        public Exception Ex { get; private set; }

        volatile ValueTaskSourceStatus status = ValueTaskSourceStatus.Pending;

        /// <summary>
        /// 为了实现挂起时访问<see cref="ValueTask{TResult}.Result"/>阻塞线程。
        /// </summary>
        protected ManualResetEvent resetEvent = new ManualResetEvent(false);
        readonly object locker = new object();
        public List<(Action<object> Continuation, object State)> Continuations
                = new List<(Action<object> Continuation, object State)>();
        public short Token { get; private set; } = 0;
        public ValueTask<TResult> ValueTask => new ValueTask<TResult>(this, Token);

        public TempValueTaskSource()
        {

        }

        public TempValueTaskSource(short token)
        {
            this.Token = token;
        }

        /// <summary>
        /// 清理所有状态，准备重用
        /// </summary>
        public void Reset()
        {
            lock (locker)
            {
                Continuations.Clear();
                resetEvent.Reset();
                status = ValueTaskSourceStatus.Pending;
                Value = default;
                Ex = null;
                Token++;
            }
        }

        /// <summary>
        /// 释放已等待异步后续，释放关联引用，什么也不触发。
        /// </summary>
        public void FreeContinuation()
        {
            lock (locker)
            {
                Continuations.Clear();
                TrySetCanceled();
                Reset();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CheckToken(short token)
        {
            if (token != Token)
            {
                throw new InvalidOperationException($"IValueTaskSource.Token 与 ValueTask.Token不一致。");
            }
        }

        public TResult Value { get; private set; }

        public ValueTaskSourceStatus GetStatus(short token)
        {
            lock (locker)
            {
                CheckToken(token);
                return status;
            }
        }

        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            lock (locker)
            {
                CheckToken(token);
                if (status == ValueTaskSourceStatus.Pending)
                {
                    Continuations.Add((continuation, state));
                }
                else
                {
                    throw new TaskSchedulerException();
                }
            }
        }

        public TResult GetResult(short token)
        {
            lock (locker)
            {
                CheckToken(token);
            }

            switch (status)
            {
                case ValueTaskSourceStatus.Pending:
                    resetEvent.WaitOne();
                    return GetResult(token);
                case ValueTaskSourceStatus.Succeeded:
                    return Value;
                case ValueTaskSourceStatus.Faulted:
                    if (Ex != null)
                    {
                        throw Ex;
                    }
                    else
                    {
                        throw new Exception();
                    }
                case ValueTaskSourceStatus.Canceled:
                    throw new TaskCanceledException();
                default:
                    return default;
            }
        }

        protected virtual void CompleteTask()
        {
            resetEvent?.Set();
            try
            {
                foreach (var (Continuation, State) in Continuations)
                {
                    Continuation?.Invoke(State);
                }
            }
            finally
            {
                Continuations.Clear();
            }
        }

        public bool TrySetResult(TResult result)
        {
            lock (locker)
            {
                if (status == ValueTaskSourceStatus.Pending)
                {
                    Value = result;
                    status = ValueTaskSourceStatus.Succeeded;
                    CompleteTask();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool TrySetCanceled()
        {
            lock (locker)
            {
                if (status == ValueTaskSourceStatus.Pending)
                {
                    status = ValueTaskSourceStatus.Canceled;
                    CompleteTask();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool TrySetException(Exception exception)
        {
            lock (locker)
            {
                if (status == ValueTaskSourceStatus.Pending)
                {
                    Ex = exception;
                    status = ValueTaskSourceStatus.Faulted;
                    CompleteTask();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    /// <inheritdoc/>
    public class TempValueTaskSource : TempValueTaskSource<bool>, IValueTaskSource
    {
        public TempValueTaskSource() : base()
        {

        }

        public TempValueTaskSource(short token) : base(token)
        {
        }

        public new ValueTask ValueTask => new ValueTask(this, Token);

        void IValueTaskSource.GetResult(short token)
        {
            base.GetResult(token);
        }

        public bool TrySetResult()
        {
            return TrySetResult(true);
        }
    }


    /// <summary>
    /// 舍弃一个异步，永远不能触发
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ForgetValueTaskSource<T> : IValueTaskSource<T>
    {
        private ForgetValueTaskSource() { }
        public ValueTaskSourceStatus GetStatus(short token)
        {
            return ValueTaskSourceStatus.Pending;
        }

        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
        {

        }

        public T GetResult(short token)
        {
            return default(T);
        }

        /// <summary>
        /// 返回一个永远不会完成的Task
        /// </summary>
        public static ValueTask<T> ForgetIt
        {
            get
            {
                return new ValueTask<T>(new ForgetValueTaskSource<T>(), 0);
            }
        }
    }

    public class ForgetValueTaskSource : IValueTaskSource
    {
        private ForgetValueTaskSource() { }

        public ValueTaskSourceStatus GetStatus(short token)
        {
            return ValueTaskSourceStatus.Pending;
        }

        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
        {

        }

        public void GetResult(short token)
        {

        }

        /// <summary>
        /// 返回一个永远不会完成的Task
        /// </summary>
        public static ValueTask ForgetIt
        {
            get
            {
                return new ValueTask(new ForgetValueTaskSource(), 0);
            }
        }
    }

    /// <summary>
    /// 没有意义。直接用TaskCompletionSource 就行。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueTaskPool<T>
    {
        public ValueTask<T> Regist()
        {
            return default;
        }

        void Test()
        {
            var v = Regist();
            var r = v.Result;
        }
    }
}













