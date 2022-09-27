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
        public float INTERVAL = 0.5f;
        private int lastFrameCount;

        public int FPS { get; protected set; } = 0;
        public string FPSString { get; protected set; } = "FPS : 0";
        public string Format = "FPS : 0";
        public static readonly FPSCounter Default = new FPSCounter();
        private FPSCounter() { }

        /// <summary>
        /// retur FPSString.
        /// </summary>
        /// <returns></returns>
        public string Update()
        {
            if (Application.isPlaying)
            {
                float realtimeSinceStartup = Time.realtimeSinceStartup;
                float deltaTime = realtimeSinceStartup - oldTime;

                if (deltaTime >= INTERVAL)
                {
                    int frameCount = Time.renderedFrameCount;
                    //Debug.Log(frameCount);
                    var frame = frameCount - lastFrameCount;
                    ///小数没有意义
                    FPS = (int)(frame / deltaTime + 0.5f); //四舍五入,免得显示59帧.
                    FPSString = FPS.ToString(Format);   // string.Format(Format, FPS);
                    oldTime = realtimeSinceStartup;
                    lastFrameCount = frameCount;
                }
            }
            return FPSString;
        }
    }

    /// <summary>
    /// 模仿OW显示. 不能合批,dc有点高.
    /// </summary>
    //[ExecuteAlways]
    public class IMGUIDeviceInfo : MonoBehaviour
    {
        public int fontSize = 14;
        public int Space = 1600;
        [Space]
        public bool FPS = true;
        [SerializeField]
        [Indent]
        private float INTERVAL = 0.5f;
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

        private void Start()
        {
            FPSCounter.Default.INTERVAL = INTERVAL;
        }

        private void Update()
        {
            FPSCounter.Default.Update();
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
            GUILayout.Space(Space);
            if (FPS)
            {
                GUILayout.Label(FPSCounter.Default.FPSString, mystyle);
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




