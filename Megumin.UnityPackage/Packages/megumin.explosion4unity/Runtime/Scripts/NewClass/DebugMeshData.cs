using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

namespace Megumin
{
    /// <summary>
    /// 调试Mesh工具,快速显示mesh数据.
    /// </summary>
    public class DebugMeshData : MonoBehaviour
    {
        public enum DataType
        {
            Index,
            UV,
            Color,
        }

        public List<Mesh> Meshes;
        public int MeshIndex = 0;
        public DataType Type = DebugMeshData.DataType.Index;
        public int DataTypeParam = 0;
        public float LabelOffset;
        public bool Draw01Line = true;

        public Material Material;
        public Texture Texture;
        public Shader Shader;

        [EditorButton]
        void CollectMesh()
        {
            var g = gameObject.GetComponentsInChildren<TMP_Text>().ToList();
            Meshes.Clear();
            foreach (var item in g)
            {
                Meshes.Add(item.mesh);
            }
        }

        [EditorButton]
        void ParseMat()
        {
            if (Material)
            {
                Texture = Material.mainTexture;
                Shader = Material.shader;
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            var mesh = Meshes[0];

            if (mesh)
            {
                switch (Type)
                {
                    case DataType.Index:
                        {
                            for (int i = 0; i < mesh.vertices.Length; i++)
                            {
                                UnityEditor.Handles.Label(transform.position + mesh.vertices[i]
                                    + new Vector3(0, LabelOffset, 0), i.ToString());

                            }
                        }
                        break;
                    case DataType.UV:
                        {
                            LabelUV(mesh);
                        }
                        break;
                    case DataType.Color:
                        {
                            for (int i = 0; i < mesh.vertices.Length; i++)
                            {
                                UnityEditor.Handles.Label(transform.position + mesh.vertices[i]
                                    + new Vector3(0, LabelOffset, 0), mesh.colors[i].ToString());


                            }
                        }
                        break;
                    default:
                        break;
                }

                if (Draw01Line)
                {
                    var point1 = transform.position + mesh.vertices[0];
                    var point2 = transform.position + mesh.vertices[1];
                    Debug.DrawLine(point1, point2, HexColor.BarnRed);
                }
            }

        }

        private void LabelUV(Mesh mesh)
        {
            var uv = mesh.uv;
            switch (DataTypeParam)
            {
                case 2:
                    uv = mesh.uv2;
                    break;
                case 3:
                    uv = mesh.uv3;
                    break;
                case 4:
                    uv = mesh.uv4;
                    break;
                case 5:
                    uv = mesh.uv5;
                    break;
                case 6:
                    uv = mesh.uv6;
                    break;
                case 7:
                    uv = mesh.uv7;
                    break;
                case 8:
                    uv = mesh.uv8;
                    break;
                default:
                    break;
            }



            for (int i = 0; i < uv.Length; i++)
            {
                UnityEditor.Handles.Label(transform.position + mesh.vertices[i]
                    + new Vector3(0, LabelOffset, 0), uv[i].ToString());

            }
        }
#endif

    }
}





