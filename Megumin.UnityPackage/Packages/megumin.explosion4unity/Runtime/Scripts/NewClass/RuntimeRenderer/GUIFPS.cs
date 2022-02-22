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
        [SerializeField]
        private float INTERVAL = 0.5f;

        void Start()
        {
            FPSCounter.Default.INTERVAL = INTERVAL;
        }


        void Update()
        {
            Content = FPSCounter.Default.Update();
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



