using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 标记点脚本
/// <para>用来在Scene标记一个空的GameObject</para>
/// </summary>
[ExecuteInEditMode]
public class TargetPoint : MonoBehaviour
{
#if UNITY_EDITOR

    [Tooltip("是否中心调整到模型底部")]
    public bool centerDown = true;
    [Tooltip("网格类型")]
    public PrimitiveType type = PrimitiveType.Capsule;
    public Vector3 scale = Vector3.one;
    Mesh pointMesh;
    public Color targetColor  = new Color32(105,227,116,41);

    PrimitiveType old = PrimitiveType.Capsule;

    private void Awake()
    {
        CheckMesh();
    }

    private void Update()
    {
        CheckMesh();
    }


    /// <summary>
    /// 检查mesh变动
    /// </summary>
    private void CheckMesh()
    {
        if (!pointMesh || type != old)
        {
            GameObject temp = GameObject.CreatePrimitive(type);
            Mesh capsule = temp.GetComponent<MeshFilter>().sharedMesh;
            pointMesh = capsule;
#if UNITY_EDITOR
            DestroyImmediate(temp);
#else  
            Destroy(temp);
#endif

            old = type;
        }
    }

    private void OnDrawGizmos()
    {
        if (enabled)
        {
            switch (type)
            {
                case PrimitiveType.Sphere:
                    DrawSphere();
                    break;
                case PrimitiveType.Capsule:
                    DrawPlayerStart();
                    break;
                case PrimitiveType.Cylinder:
                    break;
                case PrimitiveType.Cube:
                    DrawCube();
                    break;
                case PrimitiveType.Plane:
                    break;
                case PrimitiveType.Quad:
                    break;
                default:
                    break;
            }
        }
    }

    private void DrawSphere()
    {
        Gizmos.color = targetColor;
        Vector3 meshCenter = transform.position;
        if (centerDown)
        {
            meshCenter = transform.position + Vector3.up * 0.5f * scale.y;
        }


        Gizmos.DrawMesh(pointMesh, meshCenter, transform.rotation, scale);
    }

    private void DrawCube()
    {
        Gizmos.color = targetColor;
        Vector3 meshCenter = transform.position;
        if (centerDown)
        {
            meshCenter = transform.position + Vector3.up * 0.5f * scale.y;
        }


        Gizmos.DrawMesh(pointMesh, meshCenter,transform.rotation, scale);
    }

    private void DrawPlayerStart()
    {
        Gizmos.color = targetColor;
        Vector3 meshCenter = transform.position;

        if (centerDown)
        {
            meshCenter = transform.position + Vector3.up * scale.y;
        }

        Gizmos.DrawWireMesh(pointMesh, meshCenter, transform.rotation,scale);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(meshCenter,meshCenter + transform.forward * 1.5f);
    }

#endif
}
