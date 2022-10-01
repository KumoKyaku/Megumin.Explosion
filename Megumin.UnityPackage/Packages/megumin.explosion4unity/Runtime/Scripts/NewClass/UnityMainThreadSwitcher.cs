using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin
{
    [DefaultExecutionOrder(int.MinValue)]
    public class UnityMainThreadSwitcher : MonoBehaviour
    {
        /// <summary>
        /// 一样是每帧一次，不如用Update。
        /// </summary>
        SynchronizationContext context;

        private void Awake()
        {
            context = SynchronizationContext.Current;
            context.Post(MyCallBack, null);
        }

        private void MyCallBack(object state)
        {
            DefaultThreadSwitcher.Tick();
            Debug.Log($"{Time.frameCount} -- {Utility.ToStringReflection<Time>()}");
            context.Post(MyCallBack, null);
        }

        [Button]
        public void ThreadTest()
        {
            this.LogThreadID(1);
            Task.Run(async () =>
            {
                this.LogThreadID(2);
                await Task.Delay(10);
                this.LogThreadID(3);
                await MainThread.Switch();
                this.LogThreadID(4);
            });
        }
    }

    public class MainThread
    {
        /// <summary>
        /// 一样是每帧一次，不如用Update。
        /// </summary>
        static SynchronizationContext context;

        public static SynchronizationContext Context => context;
        public static int ManagedThreadId { get; internal protected set; } = -1;

        [RuntimeInitializeOnLoadMethod]
        static void InitContext()
        {
            context = SynchronizationContext.Current;
            ManagedThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void InitEditorContext()
        {
            context = SynchronizationContext.Current;
            ManagedThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }
#endif

        public static SynchronizationContextExtension_F196DAE75A234F72A3AC998F76A66F1A.Awaitable Switch()
        {
            if (SynchronizationContext.Current == context)
            {
                return SynchronizationContextExtension_F196DAE75A234F72A3AC998F76A66F1A.Awaitable.CompletedTask;
            }
            else
            {
                return context.Switch();
            }
        }

        public static async ValueTask Switch2()
        {
            if (SynchronizationContext.Current == context)
            {
                return;
            }
            else
            {
                await context.Switch();
            }
        }

        public static void Post(SendOrPostCallback callback, object state)
        {
            context.Post(callback, state);
        }

        /// <summary>
        /// 用于编辑器将PlayMode的值保存。
        /// 没有经过测试。
        /// </summary>
        /// <returns></returns>
        public static Task ExitPlayMode()
        {
#if UNITY_EDITOR
            TaskCompletionSource<int> source = new TaskCompletionSource<int>();
            void Test(UnityEditor.PlayModeStateChange obj)
            {
                UnityEditor.EditorApplication.playModeStateChanged -= Test;
                source.TrySetResult(0);
            }
            UnityEditor.EditorApplication.playModeStateChanged += Test;
            return source.Task;
#else
            return Task.CompletedTask;
#endif
        }
    }
}
