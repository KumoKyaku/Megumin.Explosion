using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    [ExecuteAlways]
    public class SystemInfoLabel : LabelBase
    {
        private void Update()
        {
            Content = SystemInfo.deviceModel;
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

