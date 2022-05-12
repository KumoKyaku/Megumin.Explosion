using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    public class HideFlagsUtility : MonoBehaviour
    {
        public HideFlags HideFlags;
        public bool AlsoChangeChild = false;

        [Button]
        [ContextMenu(nameof(Change))]
        public void Change()
        {
            gameObject.hideFlags = HideFlags;
            if (AlsoChangeChild)
            {
                gameObject.SetHideFlagsOnAll(HideFlags);
            }
        }
    }
}




