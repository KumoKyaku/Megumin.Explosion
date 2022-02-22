using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MyNamespace
{
    public class ColliderTriggerTest : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            this.LogCallerMemberName();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            this.LogCallerMemberName();
        }

        private void OnCollisionExit(Collision collision)
        {
            this.LogCallerMemberName();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            this.LogCallerMemberName();
        }

        private void OnCollisionStay(Collision collision)
        {
            this.LogCallerMemberName();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            this.LogCallerMemberName();
        }

        private void OnTriggerEnter(Collider other)
        {
            this.LogCallerMemberName();
            var c = GetComponent<BoxCollider>();
            const float len = 1f;
            var res = Physics.BoxCast(c.bounds.center, c.bounds.extents, Vector3.up,out var hit, transform.rotation, len);
            var res1 = Physics.BoxCastAll(c.bounds.center, c.bounds.extents, Vector3.up, transform.rotation, len);

            var res2 = Physics.BoxCastAll(c.bounds.center, c.bounds.extents, Vector3.up, transform.rotation, len);
            RaycastHit[] results = new RaycastHit[10];
            Physics.BoxCastNonAlloc(c.bounds.center, c.bounds.extents, Vector3.up, results, transform.rotation, len);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            this.LogCallerMemberName();
        }

        private void OnTriggerExit(Collider other)
        {
            this.LogCallerMemberName();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            this.LogCallerMemberName();
        }

        private void OnTriggerStay(Collider other)
        {
            this.LogCallerMemberName();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            this.LogCallerMemberName();
        }
    }

}
