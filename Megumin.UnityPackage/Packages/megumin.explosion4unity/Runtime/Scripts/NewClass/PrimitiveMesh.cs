using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://answers.unity.com/questions/514293/changing-a-gameobjects-primitive-mesh.html
/// </summary>
public static class PrimitiveMesh
{
    public static Mesh GetUnityPrimitiveMesh(PrimitiveType primitiveType)
    {
        switch (primitiveType)
        {
            case PrimitiveType.Sphere:
                return GetCachedPrimitiveMesh(ref _unitySphereMesh, primitiveType);
                break;
            case PrimitiveType.Capsule:
                return GetCachedPrimitiveMesh(ref _unityCapsuleMesh, primitiveType);
                break;
            case PrimitiveType.Cylinder:
                return GetCachedPrimitiveMesh(ref _unityCylinderMesh, primitiveType);
                break;
            case PrimitiveType.Cube:
                return GetCachedPrimitiveMesh(ref _unityCubeMesh, primitiveType);
                break;
            case PrimitiveType.Plane:
                return GetCachedPrimitiveMesh(ref _unityPlaneMesh, primitiveType);
                break;
            case PrimitiveType.Quad:
                return GetCachedPrimitiveMesh(ref _unityQuadMesh, primitiveType);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null);
        }
    }

    private static Mesh GetCachedPrimitiveMesh(ref Mesh primMesh, PrimitiveType primitiveType)
    {
        if (primMesh == null)
        {
            Debug.Log("Getting Unity Primitive Mesh: " + primitiveType);
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
                break;
            case PrimitiveType.Capsule:
                return "New-Capsule.fbx";
                break;
            case PrimitiveType.Cylinder:
                return "New-Cylinder.fbx";
                break;
            case PrimitiveType.Cube:
                return "Cube.fbx";
                break;
            case PrimitiveType.Plane:
                return "New-Plane.fbx";
                break;
            case PrimitiveType.Quad:
                return "Quad.fbx";
                break;
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
