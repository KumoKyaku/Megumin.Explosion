using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Sector : MonoBehaviour
    {
        public enum DrawDirection
        {
            ClockWise,
            CounterClockWise,
        }
        /// <summary>
        /// Todo,太烦了，以后再写
        /// </summary>
        public DrawDirection Direction = DrawDirection.CounterClockWise;

        public float Radius = 6;          //外半径  
        [Range(0, 10)]
        public float innerRadius = 3;     //内半径
        [Range(0, 360)]
        public float angleDegree = 180;   //扇形或扇面的角度
        [Range(0, 360)]
        public int angleOffset = 0;
        [Range(6, 120)]
        public int SegmentsPer360 = 30;
        [ReadOnlyInInspector]
        public int Segments = 60;         //分割数  

        private MeshFilter meshFilter;

        private void Awake()
        {
            ReCreate();
        }

        Mesh CreateMesh(float radius, float innerradius, float angledegree, int segments)
        {
            //vertices(顶点):
            int vertices_count = segments * 2 + 2;              //因为vertices(顶点)的个数与triangles（索引三角形顶点数）必须匹配
            Vector3[] vertices = new Vector3[vertices_count];
            float angleRad = Mathf.Deg2Rad * angledegree;
            var offset = Mathf.Deg2Rad * offsetA;
            float angleCur = angleRad + offset;
            float angledelta = angleRad / segments;
            for (int i = 0; i < vertices_count; i += 2)
            {
                float cosA = Mathf.Cos(angleCur);
                float sinA = Mathf.Sin(angleCur);

                vertices[i] = new Vector3(radius * cosA, 0, radius * sinA);
                vertices[i + 1] = new Vector3(innerradius * cosA, 0, innerradius * sinA);
                if (Direction == DrawDirection.ClockWise)
                {
                    angleCur += angledelta;
                }
                else
                {
                    angleCur -= angledelta;
                }
            }

            //triangles:
            int triangle_count = segments * 6;
            int[] triangles = new int[triangle_count];
            for (int i = 0, vi = 0; i < triangle_count; i += 6, vi += 2)
            {
                triangles[i] = vi;
                triangles[i + 1] = vi + 3;
                triangles[i + 2] = vi + 1;
                triangles[i + 3] = vi + 2;
                triangles[i + 4] = vi + 3;
                triangles[i + 5] = vi;
            }

            //uv:
            Vector2[] uvs = new Vector2[vertices_count];
            for (int i = 0; i < vertices_count; i++)
            {
                uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
            }

            //负载属性与mesh
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            return mesh;
        }

        int offsetA = 90;
        public void ReCreate()
        {
            Segments = (int)(SegmentsPer360 * angleDegree / 360);
            offsetA = 90 + angleOffset - (int)(angleDegree / 2 + 0.5f);
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = CreateMesh(Radius, innerRadius, angleDegree, Segments);
        }

        private void OnValidate()
        {
            ReCreate();
        }
    }
}




