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
    }

}

