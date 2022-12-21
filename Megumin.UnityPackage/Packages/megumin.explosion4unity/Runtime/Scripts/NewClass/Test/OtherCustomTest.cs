using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    [DefaultExecutionOrder(0)]
    public class OtherCustomTest : MonoBehaviour
    {
        void Start()
        {

        }

        public bool Update_Log = false;
        private void Update()
        {
            if (Update_Log)
            {
                this.LogFrameID_Name_CallerMemberName();
            }
        }

        public bool FixedUpdate_Log = false;
        private void FixedUpdate()
        {
            if (FixedUpdate_Log)
            {
                this.LogFrameID_Name_CallerMemberName();
            }
        }

        public bool LateUpdate_Log = false;
        private void LateUpdate()
        {
            if (LateUpdate_Log)
            {
                this.LogFrameID_Name_CallerMemberName();
            }
        }
    }

}
