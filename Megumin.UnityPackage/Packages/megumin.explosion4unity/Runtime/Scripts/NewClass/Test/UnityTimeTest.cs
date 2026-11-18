using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Megumin
{


    //在一帧中的先后顺序
    //Unity一些特殊内置组件不在下面的阶段执行，例如物理，声音组件，他们由自己的独有阶段。
    //如果延续函数错过了当前帧的调用阶段，就放到下一帧执行。

    //- FixedUpdate.ScriptRunBehaviourFixedUpdate
    //  组件MonoBehaviour的FixedUpdate函数在这个阶段执行。一阵内多次调用。

    //- Update.ScriptRunBehaviourUpdate
    //  组件MonoBehaviour的Update函数在这个阶段执行

    //- Update.ScriptRunDelayedDynamicFrameRate
    //  所有的WaitNextFrame()的延续函数在这个阶段执行，包括协程和Awaitable

    //- Update.ScriptRunDelayedTasks
    //  异步函数的延续调用在这里执行。UnitySynchronizationContext.ExecuteTasks()

    //- PreLateUpdate.ScriptRunBehaviourLateUpdate
    //  组件MonoBehaviour的LateUpdate函数在这个阶段执行

    //- PlayerEndOfFrame
    //  当前帧所有等待EndOfFrame的协程和异步的延续，在这里调用。
    //  在这个阶段启动的EndOfFrame，会放到下一帧的PlayerEndOfFrame。



    /// <summary>
    /// 测试Unity 各个time时间的含义。
    /// 在同一帧中取到的值是否相同
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class UnityTimeTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            LogTime("Start");
        }


        void Update()
        {
            var sleep = Time.frameCount % 10 * 10;

            Debug.Log($"[FrameID:{Time.frameCount}]    sleep:{sleep}");
            LogTime("Update1");
            Debug.Log($"MainThread Sleep1:{sleep}");
            Thread.Sleep(sleep);
            LogTime("Update2");

            Debug.Log($"MainThread Sleep2:{sleep}");
            Thread.Sleep(sleep);

            TaskYield("Update");

            StartCoroutine(CoroutineEndOfFrame());
            StartCoroutine(CoroutineEndOfFrame2());
            StartCoroutine(CoroutineNextFrameReturnNull());
            StartCoroutine(CoroutineNextFrameReturn0());
#if UNITY_2023_1_OR_NEWER
            EndOfFrameAsync();
            EndOfFrameAsync2();
            NextFrameAsync();
#endif
        }

        private void LateUpdate()
        {
            LogTime("LateUpdate");
            TaskYield("LateUpdate");
        }

        private void FixedUpdate()
        {
            LogTime("FixedUpdate");
        }

        private async void TaskYield(string callPoint)
        {
            var callFrameID = Time.frameCount;
            Debug.Log($"{callPoint} Call TaskYield [FrameID:{callFrameID}]");
            await Task.Yield();
            LogTime2($"{callPoint} TaskYield", callFrameID);
        }

#if UNITY_2023_1_OR_NEWER
        private async void EndOfFrameAsync()
        {
            var callFrameID = Time.frameCount;
            Debug.Log($"Call EndOfFrameAsync [FrameID:{callFrameID}]");
            await Awaitable.EndOfFrameAsync();
            LogTime2("EndOfFrameAsync", callFrameID);
        }

        private async void EndOfFrameAsync2()
        {
            var callFrameID = Time.frameCount;
            Debug.Log($"Call EndOfFrameAsync2 [FrameID:{callFrameID}]");
            await Awaitable.EndOfFrameAsync();
            LogTime2("EndOfFrameAsync2 First", callFrameID);
            //Q：再EndOfFrame中再此调用EndOfFrame，延续函数会在那一帧执行？
            //会放在下一帧的EndOfFrame阶段执行。
            await Awaitable.EndOfFrameAsync();
            LogTime2("EndOfFrameAsync2 Second", callFrameID);
        }

        private async void NextFrameAsync()
        {
            var callFrameID = Time.frameCount;
            Debug.Log($"Call NextFrameAsync [FrameID:{callFrameID}]");
            await Awaitable.NextFrameAsync();
            LogTime3("NextFrameAsync", callFrameID);
        }
#endif

        private IEnumerator CoroutineEndOfFrame()
        {
            var callFrameID = Time.frameCount;
            Debug.Log($"Call CoroutineEndOfFrame [FrameID:{callFrameID}]");
            yield return new WaitForEndOfFrame();
            //注意，编辑器一定要打开Game窗口，不然相机不会渲染，WaitForEndOfFrame后的代码不会触发。
            //Debug.LogError(111);
            LogTime2("CoroutineEndOfFrame", callFrameID);
        }

        private IEnumerator CoroutineEndOfFrame2()
        {
            var callFrameID = Time.frameCount;
            Debug.Log($"Call CoroutineEndOfFrame2 [FrameID:{callFrameID}]");
            yield return new WaitForEndOfFrame();
            //注意，编辑器一定要打开Game窗口，不然相机不会渲染，WaitForEndOfFrame后的代码不会触发。
            //Debug.LogError(111);
            LogTime2("CoroutineEndOfFrame2 First", callFrameID);
            yield return new WaitForEndOfFrame();
            //注意，编辑器一定要打开Game窗口，不然相机不会渲染，WaitForEndOfFrame后的代码不会触发。
            //Debug.LogError(111);
            //会放在下一帧的EndOfFrame阶段执行。
            LogTime2("CoroutineEndOfFrame2 Second", callFrameID);
        }

        /// <summary>
        /// 延迟一帧执行
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoroutineNextFrameReturnNull()
        {
            var callFrameID = Time.frameCount;
            Debug.Log($"Call CoroutineNextFrameReturnNull [FrameID:{callFrameID}]");
            yield return null;
            //这里居然是下一帧Update之后，下一帧的WaitForEndOfFrame之前 Unity2021.3.30f1 Unity2022.3.45f1 Unity6000.0.41f
            //按照推测应该是下一帧Update之前才合理，不知道为什么不对 
            LogTime3("CoroutineNextFrameReturnNull", callFrameID);
        }

        /// <summary>
        /// 延迟一帧执行
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoroutineNextFrameReturn0()
        {
            var callFrameID = Time.frameCount;
            Debug.Log($"Call CoroutineNextFrameReturn0 [FrameID:{callFrameID}]");
            yield return 0;
            //这里居然是下一帧Update之后，下一帧的WaitForEndOfFrame之前 Unity2021.3.30f1 Unity2022.3.45f1 Unity6000.0.41f
            //按照推测应该是下一帧Update之前才合理，不知道为什么不对 
            LogTime3("CoroutineNextFrameReturn0", callFrameID);
        }

        public void LogTime(string point)
        {
            Debug.Log($"{point}  [FrameID:{Time.frameCount}]  DeltaTime:{Time.deltaTime}  Time:{Time.time}  RealTime:{Time.realtimeSinceStartup}  FixedDeltaTime:{Time.fixedDeltaTime}");
        }

        public void LogTime2(string point, int callFrameID)
        {
            Debug.Log($"{point}  CallFrameID:{callFrameID}  [FrameID:{Time.frameCount}]  DeltaTime:{Time.deltaTime}  Time:{Time.time}  RealTime:{Time.realtimeSinceStartup}  FixedDeltaTime:{Time.fixedDeltaTime}");
        }

        public void LogTime3(string point, int callFrameID)
        {
            Debug.Log($"{point}  CallFrameID:{callFrameID}  [FrameID:{Time.frameCount}]  DeltaTime:{Time.deltaTime}  Time:{Time.time}  RealTime:{Time.realtimeSinceStartup}  FixedDeltaTime:{Time.fixedDeltaTime}");
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/ShowPlayerLoop")]
#endif
        public static void ShowPlayerLoop()
        {
            StringBuilder sb = new();
            ShowPlayerLoop(PlayerLoop.GetCurrentPlayerLoop(), sb, 0);
            UnityEngine.Debug.LogError(sb);
        }

        private static void ShowPlayerLoop(PlayerLoopSystem playerLoopSystem, StringBuilder text, int inline)
        {
            if (playerLoopSystem.type != null)
            {
                for (var i = 0; i < inline; i++)
                {
                    text.Append("\t");
                }
                text.AppendLine(playerLoopSystem.type.Name);
            }

            if (playerLoopSystem.subSystemList != null)
            {
                inline++;
                foreach (var s in playerLoopSystem.subSystemList)
                {
                    ShowPlayerLoop(s, text, inline);
                }
            }
        }
    }
}




