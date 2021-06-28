using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

namespace Megumin
{
    [ExecuteAlways]
    public class DebugMeshRenderer : MonoBehaviour
    {
        public bool ShowOnRuntime = false;
        [Header("Body")]
        public bool RenderBody = true;
        [Tooltip("是否中心调整到模型底部")]
        public bool centerDown = true;
        [Tooltip("网格模式")]
        public bool IsWire = true;
        [Tooltip("网格类型")]
        public PrimitiveType type = PrimitiveType.Sphere;
        public Mesh mesh;
        public Overridable<float> Scale = new Overridable<float>(1);
        public Vector3 center = Vector3.zero;

        public Material DefaultMat;

        public Material Material
        {
            get
            {
                return DefaultMat;
            }
        }

        [Header("Arrow")]
        public bool RenderArrow = true;
        [Range(0, 3f)]
        public float ArrowLength = 0.5f;
        [Range(0, 0.1f)]
        public float ArrowRidus = 0.02f;
        public Color ArrowColor = Color.red;
        Material ArrowMaterial;

        private void Start()
        {
            InitArrow();
        }

        private void InitArrow()
        {
            ArrowMaterial = Instantiate(Material);
            ArrowMaterial.color = ArrowColor;
        }

        void LateUpdate()
        {
            if (ShowOnRuntime)
            {
                DrawMesh();
            }
        }

        private void OnValidate()
        {
            InitArrow();
        }

        internal void DrawMesh()
        {
            if (enabled)
            {
                var scale = transform.localScale;
                var pos = transform.position + center;
                scale *= Scale;
                if (centerDown)
                {
                    if (type == PrimitiveType.Capsule || type == PrimitiveType.Cylinder)
                    {
                        pos.y += scale.y;
                    }
                    else
                    {
                        pos.y += scale.y / 2;
                    }
                }

                if (RenderBody)
                {
                    Mesh body = default;
                    if (mesh)
                    {
                        body = mesh;
                    }
                    else
                    {
                        body = PrimitiveMesh.GetUnityPrimitiveMesh(type);
                    }

                    var matri = Matrix4x4.TRS(pos, transform.rotation, scale);
                    body.Draw(matri, Material);
                }

                if (RenderArrow)
                {
                    //绘制方向
                    var arrow = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Cube);
                    var len = scale.z * ArrowLength;
                    var arrowPos = pos + transform.forward * ((scale.z + len) / 2);
                    var r = Mathf.Min(ArrowRidus * scale.x, ArrowRidus * scale.y);
                    r = Mathf.Max(r, 0.02f);
                    var arrowscale = new Vector3(r, r, len);
                    var arrowMatri = Matrix4x4.TRS(arrowPos, transform.rotation, arrowscale);
                    arrow.Draw(arrowMatri, ArrowMaterial);
                }

            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!ShowOnRuntime)
            {
                var scale = transform.localScale;
                var pos = transform.position + center;
                scale *= Scale;
                if (centerDown)
                {
                    if (type == PrimitiveType.Capsule || type == PrimitiveType.Cylinder)
                    {
                        pos.y += scale.y;
                    }
                    else
                    {
                        pos.y += scale.y / 2;
                    }
                }

                if (RenderBody)
                {
                    Mesh body = default;
                    if (mesh)
                    {
                        body = mesh;
                    }
                    else
                    {
                        body = PrimitiveMesh.GetUnityPrimitiveMesh(type);
                    }

                    var matri = Matrix4x4.TRS(pos, transform.rotation, scale);
                    body.GizmoDraw(matri, Material.color, IsWire);
                }

                if (RenderArrow)
                {
                    //绘制方向
                    var arrow = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Cube);
                    var len = scale.z * ArrowLength;
                    var arrowPos = pos + transform.forward * ((scale.z + len) / 2);
                    var r = Mathf.Min(ArrowRidus * scale.x, ArrowRidus * scale.y);
                    r = Mathf.Max(r, 0.02f);
                    var arrowscale = new Vector3(r, r, len);
                    var arrowMatri = Matrix4x4.TRS(arrowPos, transform.rotation, arrowscale);
                    arrow.Draw(arrowMatri, ArrowMaterial);
                }

            }
        }
#endif

    }

}



