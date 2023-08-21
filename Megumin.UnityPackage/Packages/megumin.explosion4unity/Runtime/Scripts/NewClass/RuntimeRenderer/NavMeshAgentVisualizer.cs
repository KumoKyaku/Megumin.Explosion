using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.ComponentModel;

#if DrawXXL

namespace DrawXXL
{
    [DefaultExecutionOrder(8000)]
    public class NavMeshAgentVisualizer : MonoBehaviour
    {
        NavMeshAgent MeshAgent;
        public bool velocity = false;
        // Start is called before the first frame update
        void Awake()
        {
            MeshAgent = GetComponent<NavMeshAgent>();
        }

        //private void FixedUpdate()
        //{
        //    var oldSpeed = MeshAgent.speed;
        //    for (int i = 0; i < 10; i++)
        //    {
        //        MeshAgent.speed = i;
        //        DrawBasics.Point(MeshAgent.nextPosition, text: $"SpeedTest Speed:{MeshAgent.speed}",
        //                       Color.cyan);
        //    }
        //    MeshAgent.speed = oldSpeed;
        //}

        // Update is called once per frame
        void Update()
        {
            if (!MeshAgent)
            {
                return;
            }

            if (velocity)
            {
                DrawBasics.VectorFrom(MeshAgent.transform.position, MeshAgent.velocity, Color.blue, text: $"NavMeshAgent.velocity {MeshAgent.velocity}");
            }

            DrawBasics.PointTag(MeshAgent.destination,
                                $"{MeshAgent.name}.destination",
                                Color.red);

            DrawBasics.PointTag(MeshAgent.nextPosition,
                                $"NavMeshAgent.NextPosition. Speed:{MeshAgent.speed}",
                                Color.green);

            if (MeshAgent.path != null && MeshAgent.hasPath)
            {
                var c = MeshAgent.path.corners;
                int length = c.Length;
                if (length > 0)
                {
                    //DrawBasics.Line(MeshAgent.transform.position, c[0], Color.green);

                    for (int i = 0; i < length; i++)
                    {
                        DrawBasics.Point(c[i], Color.yellow, text: $"{i}");
                        if (i < length - 1)
                        {
                            DrawBasics.Line(c[i], c[i + 1], Color.red);
                        }
                    }

                    if (c[length - 1] != MeshAgent.destination)
                    {
                        DrawBasics.PointTag(c[length - 1], text: $"{MeshAgent.name}.path", Color.red);
                    }
                }

            }
        }

        //private void LateUpdate()
        //{
        //    //MeshAgent.Warp(transform.position);
        //    MeshAgent.nextPosition = transform.position;
        //}

        [Editor]
        public void SetUpdatePosition(bool value)
        {
            MeshAgent.updatePosition = value;
            MeshAgent.updateRotation = value;
            //MeshAgent.updateUpAxis = value;
        }

        [Editor]
        public void SetDestination(Vector3 offset)
        {
            MeshAgent.SetDestination(transform.position + offset);
        }
    }

    public static class Extension_DrawXXL_35AA76C28B2E48F4A9058A191EF29144
    {
        public static void DrawXXL(this Vector3[] vector3s)
        {
            if (vector3s.Length < 2)
            {
                return;
            }
            for (int i = 0; i < vector3s.Length; i++)
            {
                DrawBasics.Point(vector3s[i], Color.yellow, text: $"{i}");
                if (i < vector3s.Length - 1)
                {
                    DrawBasics.Line(vector3s[i], vector3s[i + 1], Color.red);
                }
            }
        }
    }
}

#endif
