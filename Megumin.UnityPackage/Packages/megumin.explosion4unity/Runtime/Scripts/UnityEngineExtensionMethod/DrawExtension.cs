using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DrawExtension_95DA6E62
{
    /// <summary>
    /// 在游戏中绘制一个mesh
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="matri"></param>
    /// <param name="material"></param>
    public static void Draw(this Mesh mesh, Matrix4x4 matri, Material material)
    {
        if (mesh && material)
        {
            Graphics.DrawMesh(mesh,
                          matri,
                          material,
                          0,
                          default,
                          default,
                          default,
                          UnityEngine.Rendering.ShadowCastingMode.Off,
                          false,
                          null,
                          UnityEngine.Rendering.LightProbeUsage.Off,
                          null);
        }
        else
        {
            Debug.LogWarning($"mesh:{mesh} material:{material} 有参数为null");
        }
    }

    public static void Draw(this BoxCollider box, Material material)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Cube);
        Transform trans = box.transform;
        var offset = trans.localToWorldMatrix.MultiplyVector(box.center);
        var matri = Matrix4x4.identity;
        matri.SetTRS(trans.position + offset, trans.rotation, box.size);
        mesh.Draw(matri, material);
    }

    public static void Draw(this CapsuleCollider capsule, Material material)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Capsule);
        Transform trans = capsule.transform;
        var offset = trans.localToWorldMatrix.MultiplyVector(capsule.center);
        var matri = Matrix4x4.identity;
        var size = new Vector3(capsule.radius * 2, capsule.height / 2, capsule.radius * 2);
        matri.SetTRS(trans.position + offset, trans.rotation, size);
        mesh.Draw(matri, material);
    }

    public static void Draw(this SphereCollider sphere, Material material)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Sphere);
        Transform trans = sphere.transform;
        var offset = trans.localToWorldMatrix.MultiplyVector(sphere.center);
        var matri = Matrix4x4.identity;
        var size = sphere.radius * 2 * Vector3.one;
        matri.SetTRS(trans.position + offset, trans.rotation, size);
        mesh.Draw(matri, material);
    }

    public static void Draw(this MeshCollider meshCollider, Material material, Vector3 offset)
    {
        var mesh = meshCollider.sharedMesh;
        Transform trans = meshCollider.transform;
        var matri = Matrix4x4.identity;
        var size = Vector3.one;
        matri.SetTRS(trans.position + offset, trans.rotation, size);
        mesh.Draw(matri, material);
    }
}




