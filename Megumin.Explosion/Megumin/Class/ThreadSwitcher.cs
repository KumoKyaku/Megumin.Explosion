using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Megumin
{
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
        /// <inheritdoc cref="ThreadSwitcher.Switch(int)"/>
        /// </summary>
        /// <param name="safeMillisecondsDelay"></param>
        /// <returns></returns>
        [Obsolete("严重bug,无法实现预定功能. 无法保证先await 后 Tick", false)]
        public static ConfiguredValueTaskAwaitable Switch(int safeMillisecondsDelay = 0)
        {
            return Default.Switch(safeMillisecondsDelay);
        }
    }


    /// <summary>
    /// 通用线程切换器
    /// </summary>
    public partial class ThreadSwitcher
    {

        /// <summary>
        /// 可以合并Source来提高性能,但是会遇到异步后续出现异常的情况,比较麻烦.
        /// 所以每个Switch调用处使用不同的source,安全性更好
        /// </summary>
        readonly ConcurrentQueue<TaskCompletionSource<int>> WaitQueue = new ConcurrentQueue<TaskCompletionSource<int>>();
        readonly ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();

        /// <summary>
        /// 由指定线程调用,回调其他线程需要切换到这个线程的方法
        /// </summary>
        public virtual void Tick()
        {
            while (actions.TryDequeue(out var callback))
            {
                callback?.Invoke();
            }

            while (WaitQueue.TryDequeue(out var res))
            {
                res.TrySetResult(0);
            }
        }

        /// <summary>
        /// 切换执行线程
        /// <seealso cref="Switch(int)"/>
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>线程切换过程中闭包几乎无法避免, 除非明确回调函数参数类型.
        /// <para>对于性能敏感区域,可以写特定代码消除闭包. 参考 megumin.net</para>
        /// </remarks>
        public void Switch(Action action)
        {
            actions.Enqueue(action);
        }

        /// <summary>
        /// 通用性高,但是用到TaskCompletionSource和异步各种中间对象和异步机制.
        /// 性能开销大不如明确的类型和回调接口.
        /// <para>异步后续在<see cref="Tick"/>线程调用</para>
        /// <para>保证返回值先await 后Tick, 不然 await会发现Task同步完成,无法切换线程.</para>
        /// <para><see cref="Task.Status"/>无法指示是否被await </para>
        /// </summary>
        /// <returns></returns>
        /// <remarks>BUG,无法保证先await 后Tick</remarks>
        [Obsolete("严重bug,无法实现预定功能. 无法保证先await 后 Tick", false)]
        public ConfiguredValueTaskAwaitable Switch(int safeMillisecondsDelay = 0)
        {
            TaskCompletionSource<int> source = new TaskCompletionSource<int>();
            var a = new ValueTask(source.Task).ConfigureAwait(false);
            if (safeMillisecondsDelay > 0)
            {
                DelayEnqueue(safeMillisecondsDelay, source);
            }
            else
            {
                WaitQueue.Enqueue(source);
            }

            return a;
        }

        protected async void DelayEnqueue(int safeMillisecondsDelay, TaskCompletionSource<int> source)
        {
            await Task.Delay(safeMillisecondsDelay).ConfigureAwait(false);
            WaitQueue.Enqueue(source);
        }
    }
}


