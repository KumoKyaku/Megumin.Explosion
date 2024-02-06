using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    public class DebugLineUtility
    {
        public static readonly Shader UnlitColor = Shader.Find("Unlit/Color");
        public static readonly Shader URPUnlit = Shader.Find("Universal Render Pipeline/Unlit");
        public static readonly Material Material = new Material(UnlitColor);
        public static readonly Material MeshMaterial = new Material(URPUnlit ?? UnlitColor);

        static DebugLineUtility()
        {
            Material.color = Color.red;

            MeshMaterial.SetFloat("_Surface", 1);
            MeshMaterial.SetFloat("_Cull", 0);
            MeshMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            MeshMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            MeshMaterial.SetInt("_ZWrite", 0);
            MeshMaterial.DisableKeyword("_ALPHATEST_ON");
            MeshMaterial.DisableKeyword("_ALPHABLEND_ON");
            MeshMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            MeshMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            MeshMaterial.SetOverrideTag("RenderType", "Transparent");
            MeshMaterial.renderQueue = 3000;

            Color debugColor = new Color(1, 0, 0, 0.5f);
            MeshMaterial.color = debugColor;
            MeshMaterial.SetColor("_BaseColor", debugColor);
            //无法从包加载
            //var mat = Resources.Load<Material>("New Material.mat");
        }

        public static DebuglineHandle GetLineHandle()
        {
            DebuglineHandle handle = new DebuglineHandle();
            handle.lineMat = Material;
            return handle;
        }

        public static DebugMeshHandle GetMeshHandle()
        {
            DebugMeshHandle handle = new DebugMeshHandle();
            handle.Mat = MeshMaterial;
            return handle;
        }
    }

    public class DebuglineHandle
    {
        GameObject line;
        private LineRenderer l;
        public Material lineMat;
        private const float runtimeDebugLineWidth = 0.02f;

        public void Push(Vector3 position)
        {
            if (!line)
            {
                line = new GameObject("Debugline");
                line.transform.position = position;
                l = line.AddComponent<LineRenderer>();
                l.sharedMaterial = lineMat;
                l.startWidth = runtimeDebugLineWidth;
                l.endWidth = runtimeDebugLineWidth;
                l.positionCount = 0;
            }

            l.positionCount++;
            l.SetPosition(l.positionCount - 1, position);
        }

        public void Destory(float t)
        {
            if (line)
            {
                GameObject.Destroy(line, t);
            }
        }
    }

    public class DebugMeshHandle
    {
        internal Material Mat;
        private GameObject line;
        private MeshFilter mf;
        private Mesh mesh;
        private MeshRenderer mr;

        public void Push(Vector3 position1, Vector3 position2)
        {
            if (!line)
            {
                line = new GameObject("DebugMesh");
                mf = line.AddComponent<MeshFilter>();
                mesh = new Mesh();
                mf.mesh = mesh;
                mr = line.AddComponent<MeshRenderer>();
                mr.sharedMaterial = Mat;
            }

            int newCount = mesh.vertexCount + 2;
            Vector3[] vertices = new Vector3[newCount];
            Array.Copy(mesh.vertices, vertices, mesh.vertices.Length);
            vertices[newCount - 2] = position1;
            vertices[newCount - 1] = position2;

            mesh.vertices = vertices;

            Vector2[] uvs = new Vector2[newCount];
            Array.Copy(mesh.uv, uvs, mesh.uv.Length);
            uvs[newCount - 2] = new Vector2(0, 0);
            uvs[newCount - 1] = new Vector2(0, 1);

            mesh.uv = uvs;
            List<int> triangles = new List<int>();
            for (int i = 2; i < newCount; i++)
            {
                triangles.Add(i - 2);
                triangles.Add(i - 1);
                triangles.Add(i);
            }

            mesh.triangles = triangles.ToArray();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }

        public void Destory(float t)
        {
            if (line)
            {
                GameObject.Destroy(line, t);
            }
        }
    }
}
