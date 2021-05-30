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

    public static void Draw(this Collider collider, Material material, Vector3 offset = default)
    {
        switch (collider)
        {
            case BoxCollider box:
                box.Draw(material, offset);
                break;
            case CapsuleCollider capsule:
                capsule.Draw(material, offset);
                break;
            case SphereCollider sphere:
                sphere.Draw(material, offset);
                break;
            case MeshCollider meshCollider:
                meshCollider.Draw(material, offset);
                break;
            case CharacterController characterController:
                characterController.Draw(material, offset);
                break;
        }
    }

    public static void Draw(this BoxCollider box, Material material, Vector3 offset = default)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Cube);
        Transform trans = box.transform;
        offset += trans.localToWorldMatrix.MultiplyVector(box.center);
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, box.size);
        mesh.Draw(matri, material);
    }

    public static void Draw(this CapsuleCollider capsule, Material material, Vector3 offset = default)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Capsule);
        Transform trans = capsule.transform;
        offset += trans.localToWorldMatrix.MultiplyVector(capsule.center);
        var size = new Vector3(capsule.radius * 2, capsule.height / 2, capsule.radius * 2);
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, size);
        mesh.Draw(matri, material);
    }

    public static void Draw(this CharacterController characterController, Material material, Vector3 offset = default)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Capsule);
        Transform trans = characterController.transform;
        offset += trans.localToWorldMatrix.MultiplyVector(characterController.center);
        var size = new Vector3(characterController.radius * 2, characterController.height / 2, characterController.radius * 2);
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, size);
        mesh.Draw(matri, material);
    }

    public static void Draw(this SphereCollider sphere, Material material, Vector3 offset = default)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Sphere);
        Transform trans = sphere.transform;
        offset += trans.localToWorldMatrix.MultiplyVector(sphere.center);
        var size = sphere.radius * 2 * Vector3.one;
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, size);
        mesh.Draw(matri, material);
    }

    public static void Draw(this MeshCollider meshCollider, Material material, Vector3 offset)
    {
        var mesh = meshCollider.sharedMesh;
        Transform trans = meshCollider.transform;
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, Vector3.one);
        mesh.Draw(matri, material);
    }





    public static void GizmoDraw(this Mesh mesh, Matrix4x4 matri, Color color,bool isWire = false)
    {
        var oldColor = Gizmos.color;
        var oldMatrix = Gizmos.matrix;
        Gizmos.color = color;
        Gizmos.matrix = matri;

        if (isWire)
        {
            Gizmos.DrawWireMesh(mesh);
        }
        else
        {
            Gizmos.DrawMesh(mesh);
        }

        Gizmos.color = oldColor;
        Gizmos.matrix = oldMatrix;
    }

    public static void GizmoDraw(this Collider collider, Color color, Vector3 offset = default)
    {
        switch (collider)
        {
            case BoxCollider box:
                box.GizmoDraw(color, offset);
                break;
            case CapsuleCollider capsule:
                capsule.GizmoDraw(color, offset);
                break;
            case SphereCollider sphere:
                sphere.GizmoDraw(color, offset);
                break;
            case MeshCollider meshCollider:
                meshCollider.GizmoDraw(color, offset);
                break;
            case CharacterController characterController:
                characterController.GizmoDraw(color, offset);
                break;
        }
    }

    public static void GizmoDraw(this BoxCollider box, Color color, Vector3 offset = default)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Cube);
        Transform trans = box.transform;
        offset += trans.localToWorldMatrix.MultiplyVector(box.center);
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, box.size);
        mesh.GizmoDraw(matri, color);
    }

    public static void GizmoDraw(this CapsuleCollider capsule, Color color, Vector3 offset = default)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Capsule);
        Transform trans = capsule.transform;
        offset += trans.localToWorldMatrix.MultiplyVector(capsule.center);
        var size = new Vector3(capsule.radius * 2, capsule.height / 2, capsule.radius * 2);
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, size);
        mesh.GizmoDraw(matri, color);
    }

    public static void GizmoDraw(this CharacterController characterController, Color color, Vector3 offset = default)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Capsule);
        Transform trans = characterController.transform;
        offset += trans.localToWorldMatrix.MultiplyVector(characterController.center);
        var size = new Vector3(characterController.radius * 2, characterController.height / 2, characterController.radius * 2);
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, size);
        mesh.GizmoDraw(matri, color);
    }

    public static void GizmoDraw(this SphereCollider sphere, Color color, Vector3 offset = default)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Sphere);
        Transform trans = sphere.transform;
        offset += trans.localToWorldMatrix.MultiplyVector(sphere.center);
        var size = sphere.radius * 2 * Vector3.one;
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, size);
        mesh.GizmoDraw(matri, color);
    }

    public static void GizmoDraw(this MeshCollider meshCollider, Color color, Vector3 offset)
    {
        var mesh = meshCollider.sharedMesh;
        Transform trans = meshCollider.transform;
        var matri = Matrix4x4.TRS(trans.position + offset, trans.rotation, Vector3.one);
        mesh.GizmoDraw(matri, color);
    }
}




