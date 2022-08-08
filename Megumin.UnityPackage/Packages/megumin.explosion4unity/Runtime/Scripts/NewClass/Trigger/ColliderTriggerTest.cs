using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MyNamespace
{
    /// <summary>
    /// 测试结论:
    /// <para></para>OnEnter OnStay可能在同一帧被调用。
    /// OnStay 是随着fixedupdate走的，文档不准，一帧内可能被调用多次。
    /// </summary>
    public class ColliderTriggerTest : MonoBehaviour
    {
        public bool DetailTime = false;
        public bool Trigger = true;
        public bool Trigger2D = true;
        public bool Collision = true;
        public bool Collision2D = true;

        public bool BusyFrame = false;
        public int BusyTestCount = 3000;
        public void Start()
        {

        }

        public void Update()
        {
            if (BusyFrame)
            {
                string test = "";
                for (int i = 0; i < BusyTestCount; i++)
                {
                    test += i.ToString();
                }
            }
        }


        public string TimeString()
        {
            if (DetailTime)
            {
                return $"Frame:{Time.frameCount}--RenderedFrameCount:{Time.renderedFrameCount}--Time:{Time.time}--RealtimeSinceStartup:{Time.realtimeSinceStartup}  ||  ";
            }
            else
            {
                return $"Frame:{Time.frameCount}  ";
            }
        }

        public string LogCallerMemberName(Collision collision, [CallerMemberName] string func = default)
        {
            Debug.Log($"{TimeString()}{func}  {collision.collider.name}");
            return func;
        }

        public string LogCallerMemberName(Collision2D collision, [CallerMemberName] string func = default)
        {
            Debug.Log($"{TimeString()}{func}  {collision.collider.name}");
            return func;
        }

        public string LogCallerMemberName(Collider other, [CallerMemberName] string func = default)
        {
            Debug.Log($"{TimeString()}{func}  {other.name}");
            return func;
        }

        public string LogCallerMemberName(Collider2D other, [CallerMemberName] string func = default)
        {
            Debug.Log($"{TimeString()}{func}  {other.name}");
            return func;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Collision)
            {
                this.LogCallerMemberName(collision);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Collision2D)
            {
                this.LogCallerMemberName(collision);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (Collision)
            {
                this.LogCallerMemberName(collision);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (Collision2D)
            {
                this.LogCallerMemberName(collision);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (Collision)
            {
                this.LogCallerMemberName(collision);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (Collision2D)
            {
                this.LogCallerMemberName(collision);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Trigger)
            {
                this.LogCallerMemberName(other);
            }

            //var c = GetComponent<BoxCollider>();
            //const float len = 1f;
            //var res = Physics.BoxCast(c.bounds.center, c.bounds.extents, Vector3.up,out var hit, transform.rotation, len);
            //var res1 = Physics.BoxCastAll(c.bounds.center, c.bounds.extents, Vector3.up, transform.rotation, len);

            //var res2 = Physics.BoxCastAll(c.bounds.center, c.bounds.extents, Vector3.up, transform.rotation, len);
            //RaycastHit[] results = new RaycastHit[10];
            //Physics.BoxCastNonAlloc(c.bounds.center, c.bounds.extents, Vector3.up, results, transform.rotation, len);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Trigger2D)
            {
                this.LogCallerMemberName(collision);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Trigger)
            {
                this.LogCallerMemberName(other);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (Trigger2D)
            {
                this.LogCallerMemberName(collision);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Trigger)
            {
                this.LogCallerMemberName(other);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (Trigger2D)
            {
                this.LogCallerMemberName(collision);
            }
        }
    }

}
