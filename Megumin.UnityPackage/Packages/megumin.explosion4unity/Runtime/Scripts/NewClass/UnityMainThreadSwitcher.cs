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
    }

    public class MainThread
    {
        /// <summary>
        /// 一样是每帧一次，不如用Update。
        /// </summary>
        static SynchronizationContext context;

        public static SynchronizationContext CurrentContext => context;

        [RuntimeInitializeOnLoadMethod]
        static void InitContext()
        {
            context = SynchronizationContext.Current;
        }

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
    }
}
