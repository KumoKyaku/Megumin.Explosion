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
    [Tooltip("网格模式")]
    public bool IsWire = true;
    [Tooltip("网格类型")]
    public PrimitiveType type = PrimitiveType.Capsule;

    public Vector3 scale = Vector3.one;
    Mesh pointMesh;
    public Color targetColor = new Color32(105, 227, 116, 41);

    private void Awake()
    {
        pointMesh = PrimitiveMesh.GetUnityPrimitiveMesh(type);
    }

    void OnValidate()
    {
        pointMesh = PrimitiveMesh.GetUnityPrimitiveMesh(type);
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
                    DrawCylinder();
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

    private void DrawCylinder()
    {
        Gizmos.color = targetColor;
        Vector3 meshCenter = transform.position;
        if (centerDown)
        {
            meshCenter = transform.position + Vector3.up * scale.y;
        }

        DrawMesh(meshCenter);
    }

    private void DrawSphere()
    {
        Gizmos.color = targetColor;
        Vector3 meshCenter = transform.position;
        if (centerDown)
        {
            meshCenter = transform.position + Vector3.up * 0.5f * scale.y;
        }

        DrawMesh(meshCenter);
    }

    private void DrawCube()
    {
        Gizmos.color = targetColor;
        Vector3 meshCenter = transform.position;
        if (centerDown)
        {
            meshCenter = transform.position + Vector3.up * 0.5f * scale.y;
        }

        DrawMesh(meshCenter);
    }

    private void DrawPlayerStart()
    {
        Gizmos.color = targetColor;
        Vector3 meshCenter = transform.position;

        if (centerDown)
        {
            meshCenter = transform.position + Vector3.up * scale.y;
        }

        DrawMesh(meshCenter);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(meshCenter, meshCenter + transform.forward * 1.5f);
    }

    private void DrawMesh(Vector3 meshCenter)
    {
        if (IsWire)
        {
            Gizmos.DrawWireMesh(pointMesh, meshCenter, transform.rotation, scale);
        }
        else
        {
            Gizmos.DrawMesh(pointMesh, meshCenter, transform.rotation, scale);
        }
    }

#endif

}








