using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

}
