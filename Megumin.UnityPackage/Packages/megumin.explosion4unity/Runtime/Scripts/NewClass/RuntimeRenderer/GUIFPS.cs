using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 原生GUI显示帧率
    /// </summary>
    [ExecuteAlways]
    public class GUIFPS : LabelBase
    {
        // FPS
        private float oldTime;
        private int frame = 0;
        private static float frameRate = 0f;
        private static string frameRateStr = "0";
        private const float INTERVAL = 0.5f;

        void Start()
        {
            oldTime = Time.realtimeSinceStartup;
        }


        void Update()
        {
            if (Application.isPlaying)
            {
                frame++;
                float time = Time.realtimeSinceStartup - oldTime;
                if (time >= INTERVAL)
                {
                    frameRate = frame / time;
                    Content = $"FPS : {frameRate:0.0}"; ;
                    frameRateStr = Content;
                    oldTime = Time.realtimeSinceStartup;
                    frame = 0;
                }
            }
        }

        /// <summary>
        /// 获取帧率
        /// </summary>
        /// <returns></returns>
        public static float GetFrameRate()
        {
            return frameRate;
        }

        public override void InitStyle(string styleName = "CN CountBadge", bool force = false)
        {
            if (LabelStyle == null || force)
            {
                LabelStyle = new GUIStyle(GUI.skin.label);
            }
        }
    }
}



