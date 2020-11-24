using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Megumin
{
    /// <summary>
    /// 失败品，多线程不安全，有逻辑错误
    /// </summary>
    public class ValueTaskSource : ValueTaskSource<bool>, IValueTaskSource
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

    /// <summary>
    /// 失败品，多线程不安全，有逻辑错误
    /// </summary>
    public class ValueTaskSource<TResult> : IValueTaskSource<TResult>
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
}
