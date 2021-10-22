using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 运行时模拟单步执行
    /// </summary>
    public class RuntimePauseDebug : MonoBehaviour
    {
        public KeyCode Pause = KeyCode.F9;
        public KeyCode NextFrame = KeyCode.F10;
        public KeyCode AutoNextFrame = KeyCode.F11;

        bool isPause = false;
        bool isNextFrame = false;
        private void Start()
        {
            StartCoroutine(Test());
        }

        private void Update()
        {
            if (Input.GetKeyDown(Pause))
            {
                isPause = !isPause;
                Time.timeScale = isPause ? 0 : 1;
            }

            if (Input.GetKeyDown(NextFrame))
            {
                isNextFrame = true;
            }
            if (Input.GetKey(AutoNextFrame))
            {
                isNextFrame = true;
            }
        }


        IEnumerator Test()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                if (isPause)
                {
                    Time.timeScale = 0;
                }

                if (isNextFrame)
                {
                    Time.timeScale = 1;
                    isNextFrame = false;
                }

                //Debug.Log($"--------------{Time.frameCount} -- {Time.time}");
            }
        }
    }

}

