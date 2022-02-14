using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 通用线程切换器
    /// </summary>
    [Obsolete("严重bug,无法实现预定功能. 无法保证先await 后 Tick", true)]
    public partial class ThreadSwitcher
    {
        public static readonly ThreadSwitcher Default = new ThreadSwitcher();

        /// <summary>
        /// 可以合并Source来提高性能,但是会遇到异步后续出现异常的情况,比较麻烦.
        /// 所以每个Switch调用处使用不同的source,安全性更好
        /// </summary>
        readonly ConcurrentQueue<TaskCompletionSource<int>> WaitQueue = new ConcurrentQueue<TaskCompletionSource<int>>();

        /// <summary>
        /// 由指定线程调用,回调其他线程需要切换到这个线程的方法
        /// <para>保证先await 后Tick, 不然 await会发现Task同步完成,无法切换线程.</para>
        /// <para><see cref="Task.Status"/>无法指示是否被await </para>
        /// </summary>
        public void Tick()
        {
            while (WaitQueue.TryDequeue(out var res))
            {
                res.TrySetResult(0);
            }
        }

        /// <summary>
        /// 通用性高,但是用到TaskCompletionSource和异步各种中间对象和异步机制.
        /// 性能开销大不如明确的类型和回调接口.
        /// <para>异步后续在<see cref="Tick"/>线程调用</para>
        /// </summary>
        /// <returns></returns>
        /// <remarks>BUG,无法保证先await 后Tick</remarks>
        public ConfiguredValueTaskAwaitable Switch()
        {
            TaskCompletionSource<int> source = new TaskCompletionSource<int>();
            var a = new ValueTask(source.Task).ConfigureAwait(false);
            WaitQueue.Enqueue(source);
            return a;
        }
    }
}


