using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    public class CursorSwitcher : MonoBehaviour
    {
        public KeyCode SwitchKey = KeyCode.L;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(SwitchKey))
            {
                Cursor.lockState =
                    Cursor.lockState != CursorLockMode.Locked ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
            }
        }
    }
}

