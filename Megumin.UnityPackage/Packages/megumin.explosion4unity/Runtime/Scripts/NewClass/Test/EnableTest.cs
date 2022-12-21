using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    /// TODO 有时间找下C++源码验证一下。
    /// <summary>
    /// Awake 和 OnEnable 在同一个批次，多个gameobject 同时激活，初始化场景
    ///     Behaviour1.Awake
    ///     Behaviour1.OnEnable
    ///     Behaviour2.Awake
    ///     Behaviour2.OnEnable
    ///     Behaviour3.Awake
    ///     Behaviour3.OnEnable
    /// 
    /// 简单的说，OnEnable/OnDisable 在 Behaviour.enabled = true/false时立即执行。
    /// Start 一定在 FixedUpdate/Update/LateUpdate前执行一次。具体执行时间不确定。
    /// 当在一个Behaviour1中的Awake/OnEnable中将另一个Behaviour2.enabled = true时，
    /// Behaviour2能在当前帧执行Start FixedUpdate/Update/LateUpdate。
    /// 
    /// 当在Behaviour1中的Update中将Behaviour2.enabled = true时，Behaviour2.OnEnable被立即调用。
    /// Behaviour2 不能在当前帧执行Update，即使Behaviour2 ExecutionOrder 在Behaviour1之后也不行。
    /// 猜测时无在执行Update批次期间，动态将其他对象加入Update队列。
    /// 但是Behaviour2可以在当前帧执行LateUpdate，且Start函数在所有LateUpdate之前执行。
    ///     BehaviourX.Update
    ///     Behaviour1.Update（Behaviour2.enabled = true） 
    ///         Behaviour2.OnEnable
    ///     Behaviour3.Update
    ///     Behaviour2.Start
    ///     BehaviourX.LateUpdate
    ///     Behaviour1.LateUpdate
    ///     Behaviour2.LateUpdate
    ///     Behaviour3.LateUpdate
    /// 
    /// 当在Behaviour7中的Update中将Behaviour8.enabled = false时，Behaviour8.OnDisable被立即调用。
    /// 如果 Behaviour8 ExecutionOrder 在Behaviour7之前，Behaviour8.Update在当前帧必定已经被调用过了。
    /// 如果 Behaviour8 ExecutionOrder 在Behaviour7之后，Behaviour8.Update在当前帧不会被执行。
    /// 猜测 可能1：即使在执行Update批次期间，enabled = false时也会被立刻移除队列。
    ///      可能2：    在执行Update批次期间，会检测enabled值，false时不会调用Update。可以避免在迭代期间修改集合。
    /// 
    /// 在FixedUpdate 中 enabled = true 时同理，当前帧不会执行FixedUpdate，但会执行Update/LateUpdate。
    /// </summary>
    [DefaultExecutionOrder(-20)]
    public class EnableTest : MonoBehaviour
    {
        public OrderofExecutionTest OrderofExecution;
        public bool needenable = false;

        public bool logUpdate = false;
        private void Awake()
        {
            this.LogFrameID_Name_CallerMemberName();
        }

        // Start is called before the first frame update
        void Start()
        {
            this.LogFrameID_Name_CallerMemberName(1);
            OrderofExecution.enabled = needenable;
            this.LogFrameID_Name_CallerMemberName(2);
        }

        // Update is called once per frame
        void Update()
        {
            if (logUpdate)
            {
                this.LogFrameID_Name_CallerMemberName(1);
                OrderofExecution.enabled = needenable;
                this.LogFrameID_Name_CallerMemberName(2);
                logUpdate = false;
            }
        }

        private void OnValidate()
        {
            this.LogFrameID_Name_CallerMemberName();
            logUpdate = true;
        }
    }

}
