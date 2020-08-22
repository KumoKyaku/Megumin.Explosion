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
    public class GUIFPS:MonoBehaviour
    {
        // FPS
        private float oldTime;
        private int frame = 0;
        private static float frameRate = 0f;
        private const float INTERVAL = 0.5f;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool isOn = true;
        public Rect position = new Rect(50, 50, 220, 60);

        void Start()
        {
            oldTime = Time.realtimeSinceStartup;
        }


        void Update()
        {
            frame++;
            float time = Time.realtimeSinceStartup - oldTime;
            if (time >= INTERVAL)
            {
                frameRate = frame / time;
                oldTime = Time.realtimeSinceStartup;
                frame = 0;
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


        void OnGUI()
        {
            if (isOn)
            {
                GUI.Label(position, "FPS : " + frameRate.ToString(), GUI.skin.label);
            }
        }
    }
}
