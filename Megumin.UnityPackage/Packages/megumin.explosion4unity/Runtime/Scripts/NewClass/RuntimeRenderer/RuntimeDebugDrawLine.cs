using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    public class RuntimeDebugDrawLine : MonoBehaviour
    {
        public Transform From;
        public Transform To;
        public Material Material;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (From)
            {
                From.DrawLine(To, Material);
            }
            else
            {
                transform.DrawLine(To, Material);
            }
        }
    }
}

