using UnityEngine;
using System.Collections;

namespace UnityEngine
{
    /// <summary>
    /// 隐藏鼠标
    /// </summary>
    public class HideLockMouse : MonoBehaviour
    {
        [SerializeField]
        private bool hide = false;
        [SerializeField]
        private bool locked = false;

        void Awake()
        {
            ApplyCursorState();
        }

        /// <summary>
        /// 应用游标状态
        /// </summary>
        public void ApplyCursorState()
        {
            if (hide)
            {
                if (Cursor.visible)
                    Cursor.visible = false;
            }
            else
            {
                if (!Cursor.visible)
                {
                    Cursor.visible = true;
                }
            }

            ///锁定
            if (locked)
            {
                if (Cursor.lockState != CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
            else
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
    }
}