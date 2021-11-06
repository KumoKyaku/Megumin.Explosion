using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    /// <summary>
    /// FPS计数器
    /// </summary>
    public class FPSCounter
    {
        private float oldTime;
        private int frame = 0;
        public float INTERVAL = 0.5f;
        private int lastFrameCount;

        public static int FPS { get; protected set; } = 0;
        public static string FPSString { get; protected set; } = "FPS : 0";

        public static readonly FPSCounter Instance = new FPSCounter();
        private FPSCounter() { }

        public void Update()
        {
            if (Application.isPlaying)
            {
                if (Time.frameCount != lastFrameCount)
                {
                    //一帧计数一次

                    frame++;
                    float time = Time.realtimeSinceStartup - oldTime;
                    if (time >= INTERVAL)
                    {
                        ///小数没有意义
                        FPS = (int)(frame / time + 0.5f); //四舍五入,免得显示59帧.
                        FPSString = $"FPS : {FPS}";
                        oldTime = Time.realtimeSinceStartup;
                        frame = 0;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 模仿OW显示. 不能合批,dc有点高.
    /// </summary>
    //[ExecuteAlways]
    public class IMGUIDeviceInfo : MonoBehaviour
    {
        public int fontSize = 14;

        [Space]
        public bool FPS = true;
        bool TMP = false;
        bool VRM = false;
        public bool DELAY = false;
        /// <summary>
        /// 插值延迟
        /// </summary>
        public bool INDELAY = false;
        public bool Time = false;


        public string TMPStr { get; set; } = $"TMP: 67 C";
        public string VRMStr { get; set; } = $"VRM: 1770 MB";
        public string DELAYStr { get; set; } = $"DELAY: 65 MS";
        public string INDStr { get; set; } = $"IND: 25 MS";

        string now = DateTimeOffset.Now.ToString("t");
        private void Update()
        {
            FPSCounter.Instance.Update();
            now = DateTimeOffset.Now.ToString("t");
        }

        GUIStyle mystyle = null;
        private void OnGUI()
        {
            if (mystyle == null)
            {
                mystyle = new GUIStyle(GUI.skin.box);
            }
            mystyle.fontSize = fontSize;

            GUILayout.BeginHorizontal();
            if (FPS)
            {
                GUILayout.Label(FPSCounter.FPSString, mystyle);
            }

            if (TMP)
            {
                //ManagementObjectSearcher
                GUILayout.Label(TMPStr, mystyle);
            }

            if (VRM)
            {
                GUILayout.Label(VRMStr, mystyle);
            }

            if (DELAY)
            {
                GUILayout.Label(DELAYStr, mystyle);
            }

            if (INDELAY)
            {
                GUILayout.Label(INDStr, mystyle);
            }

            if (Time)
            {
                GUILayout.Label(now, mystyle);
            }

            GUILayout.EndHorizontal();
        }
    }
}




