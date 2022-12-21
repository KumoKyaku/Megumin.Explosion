using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    [DefaultExecutionOrder(0)]
    public class OtherCustomTest : MonoBehaviour
    {
        public bool Awake_Log = true;
        private void Awake()
        {
            if (Awake_Log)
            {
               this.LogFrameID_Name_CallerMemberName();
            }
        }

        public bool OnEnable_Log = true;
        // �����������ò����ڻ״̬ʱ���ô˺���
        private void OnEnable()
        {
            if (OnEnable_Log)
            {
                this.LogFrameID_Name_CallerMemberName();
            }
        }

        public bool Start_Log = true;
        void Start()
        {
            if (Start_Log)
            {
                this.LogFrameID_Name_CallerMemberName();
            }
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
