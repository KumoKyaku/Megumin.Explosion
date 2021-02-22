using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://answers.unity.com/questions/514293/changing-a-gameobjects-primitive-mesh.html
/// </summary>
public static class PrimitiveMesh
{
    /// <summary>
    /// 获取unity内置的mesh
    /// </summary>
    /// <param name="primitiveType"></param>
    /// <returns></returns>
    public static Mesh GetUnityPrimitiveMesh(PrimitiveType primitiveType)
    {
        switch (primitiveType)
        {
            case PrimitiveType.Sphere:
                return GetCachedPrimitiveMesh(ref _unitySphereMesh, primitiveType);
            case PrimitiveType.Capsule:
                return GetCachedPrimitiveMesh(ref _unityCapsuleMesh, primitiveType);
            case PrimitiveType.Cylinder:
                return GetCachedPrimitiveMesh(ref _unityCylinderMesh, primitiveType);
            case PrimitiveType.Cube:
                return GetCachedPrimitiveMesh(ref _unityCubeMesh, primitiveType);
            case PrimitiveType.Plane:
                return GetCachedPrimitiveMesh(ref _unityPlaneMesh, primitiveType);
            case PrimitiveType.Quad:
                return GetCachedPrimitiveMesh(ref _unityQuadMesh, primitiveType);
            default:
                throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null);
        }
    }

    private static Mesh GetCachedPrimitiveMesh(ref Mesh primMesh, PrimitiveType primitiveType)
    {
        if (primMesh == null)
        {
            //Debug.Log("Getting Unity Primitive Mesh: " + primitiveType);
            primMesh = Resources.GetBuiltinResource<Mesh>(GetPrimitiveMeshPath(primitiveType));

            if (primMesh == null)
            {
                Debug.LogError("Couldn't load Unity Primitive Mesh: " + primitiveType);
            }
        }

        return primMesh;
    }

    private static string GetPrimitiveMeshPath(PrimitiveType primitiveType)
    {
        switch (primitiveType)
        {
            case PrimitiveType.Sphere:
                return "New-Sphere.fbx";
            case PrimitiveType.Capsule:
                return "New-Capsule.fbx";
            case PrimitiveType.Cylinder:
                return "New-Cylinder.fbx";
            case PrimitiveType.Cube:
                return "Cube.fbx";
            case PrimitiveType.Plane:
                return "New-Plane.fbx";
            case PrimitiveType.Quad:
                return "Quad.fbx";
            default:
                throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null);
        }
    }

    private static Mesh _unityCapsuleMesh = null;
    private static Mesh _unityCubeMesh = null;
    private static Mesh _unityCylinderMesh = null;
    private static Mesh _unityPlaneMesh = null;
    private static Mesh _unitySphereMesh = null;
    private static Mesh _unityQuadMesh = null;

}
