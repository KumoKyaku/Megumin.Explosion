using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Megumin
{

    public class OrderofExecutionTest : MonoBehaviour
    {
        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerStepThrough]
        public string LogCallerMemberName([CallerMemberName] string func = default)
        {
            Debug.Log(func);
            return func;
        }

        // 加载脚本实例时调用 Awake
        private void Awake()
        {
            LogCallerMemberName();
        }

        // 当对象已启用并处于活动状态时调用此函数
        private void OnEnable()
        {
            LogCallerMemberName();
        }

        // 重置为默认值
        private void Reset()
        {
            LogCallerMemberName();
        }

        // 仅在首次调用 Update 方法之前调用 Start
        private void Start()
        {
            LogCallerMemberName();
        }

        // 如果 MonoBehaviour 已启用，则在每一帧都调用 Update
        private void Update()
        {
            //LogCallerMemberName();
        }


        // 当行为被禁用或处于非活动状态时调用此函数
        private void OnDisable()
        {
            LogCallerMemberName();
        }



        // 如果启用 MonoBehaviour，则每个固定帧速率的帧都将调用此函数
        private void FixedUpdate()
        {

        }

        // 如果启用 Behaviour，则在每一帧都将调用 LateUpdate
        private void LateUpdate()
        {

        }

        public bool OnApplicationFocus_Log = false;
        // 当玩家获得或失去焦点时发送给所有游戏对象
        private void OnApplicationFocus(bool focus)
        {
            if (OnApplicationFocus_Log)
            {
                LogCallerMemberName();
            }
        }

        public bool OnApplicationPause_Log = false;
        // 当玩家暂停时发送给所有游戏对象
        private void OnApplicationPause(bool pause)
        {
            if (OnApplicationPause_Log)
            {
                LogCallerMemberName();
            }
        }

        // 应用程序退出前发送给所有游戏对象
        private void OnApplicationQuit()
        {
            LogCallerMemberName();
        }

        // 当呈现器在任何照相机上都不可见时调用 OnBecameInvisible
        private void OnBecameInvisible()
        {
            LogCallerMemberName();
        }

        // 当呈现器在任何照相机上可见时调用 OnBecameVisible
        private void OnBecameVisible()
        {
            LogCallerMemberName();
        }

        // 当 MonoBehaviour 将被销毁时调用此函数
        private void OnDestroy()
        {
            LogCallerMemberName();
        }



        // 渲染和处理 GUI 事件时调用 OnGUI
        private void OnGUI()
        {
            //LogCallerMemberName();
        }

        public bool OnValidate_Log = false;
        // 当该脚本被加载或检视面板的值被修改时调用此函数(仅在编辑器中调用)
        private void OnValidate()
        {
            if (OnValidate_Log)
            {
                LogCallerMemberName();
            }
        }

        // 回叫在转换子级发生更改后发送到图形
        private void OnTransformChildrenChanged()
        {
            LogCallerMemberName();
        }

        // 回叫在转换父级发生更改后发送到图形
        private void OnTransformParentChanged()
        {
            LogCallerMemberName();
        }

        // 转换父级更改发生前将回叫发送到图形
        private void OnBeforeTransformParentChanged()
        {
            LogCallerMemberName();
        }

        // 如果控制器在执行移动时击中碰撞器，则调用 OnControllerColliderHit
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            LogCallerMemberName();
        }

        // 当粒子系统中的任意粒子满足触发模块的条件时调用
        private void OnParticleTrigger()
        {
            LogCallerMemberName();
        }




    }

}



