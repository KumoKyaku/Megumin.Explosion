using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class DrawExtension_95DA6E62
{
    /// <summary>
    /// 在游戏中绘制一个mesh
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="matri"></param>
    /// <param name="material"></param>
    public static void Draw(this Mesh mesh, Matrix4x4 matri, Material material)
    {
        if (mesh)
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
#if UNITY_EDITOR
            Debug.LogError($"mesh:{mesh} 参数为null");
#else
            Debug.LogWarning($"mesh:{mesh} 参数为null");
#endif
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



    public static void GizmoDraw(this Mesh mesh, Matrix4x4 matri, Color color, bool isWire = false)
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


public partial class DrawExtension_95DA6E62
{
    /// <summary>
    /// 不知道为什么不起作用
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="position"></param>
    public static void GizmoDraw(this Sprite sprite, Vector3 position)
    {
#if UNITY_EDITOR
        position = UnityEditor.SceneView.lastActiveSceneView.camera.WorldToScreenPoint(position);
#endif
        Rect dstRect = new Rect(position.x,
                                 position.y,
                                 500,
                                 500);

        Gizmos.DrawGUITexture(dstRect,
                             sprite.texture);
    }
}

partial class DrawExtension_95DA6E62
{
    //绘制点线
    public static Material DefaultUnlitColor { get; set; }
    public static Dictionary<Color, Material> Cache { get; }
        = new Dictionary<Color, Material>();

    static DrawExtension_95DA6E62()
    {
        try
        {
            DefaultUnlitColor = new Material(Shader.Find("Unlit/Color"));
            DefaultUnlitColor.color = Color.red;
            // 有SRP Batcher  Enable GPU Instancing 可以不用开启. 
            // 保险起见还是开着,反正有没有坏处.
            DefaultUnlitColor.enableInstancing = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static Material GetCacheDebugMat(in Color color)
    {
        if (!Cache.TryGetValue(color, out var mat))
        {
            if (DefaultUnlitColor)
            {
                mat = UnityEngine.Object.Instantiate(DefaultUnlitColor);
                mat.color = color;
                Cache.Add(color, mat);
            }
        }
        return mat;
    }

    public static void DrawPoint(this Transform transform, Material material = default, Vector3 offset = default)
    {
        (transform.position + offset).DrawPoint(material);
    }

    public static void DrawPoint(this Vector3 position, Material material = default)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Sphere);
        var size = 0.15f * Vector3.one;
        var matri = Matrix4x4.TRS(position, Quaternion.identity, size);
        mesh.Draw(matri, material);
    }

    public static void DrawPoint(this Vector3 position, in Color color)
    {
        DrawPoint(position, GetCacheDebugMat(color));
    }

    public static void DrawLine(this Transform from, Transform to, Material material = default)
    {
        if (from)
        {
            from.DrawPoint(material);
        }

        if (to)
        {
            to.DrawPoint(material);
        }

        if (from && to)
        {
            from.position.DrawLineWithoutEndPoint(to.position, material);
        }
    }

    public static void DrawLine(this Vector3 from, Vector3 to, Material material = default)
    {
        from.DrawPoint(material);
        to.DrawPoint(material);
        DrawLineWithoutEndPoint(from, to, material);
    }

    public static void DrawLine(this Vector3 from, Vector3 to, in Color color)
    {
        DrawLine(from, to, GetCacheDebugMat(color));
    }

    public static void DrawLineWithoutEndPoint(this Vector3 from, Vector3 to, Material material = default)
    {
        const float weight = 0.05f;
        var pos = (from + to) / 2;
        var foward = to - from;
        var q = Quaternion.identity;
        if (foward != Vector3.zero)
        {
            q = Quaternion.LookRotation(foward);
        }
        Vector3 size = new Vector3(weight, weight, Vector3.Distance(from, to));
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Cube);
        var matri = Matrix4x4.TRS(pos, q, size);
        mesh.Draw(matri, material);
    }

    public static void DrawLineWithoutEndPoint(this Vector3 from, Vector3 to, in Color color)
    {
        DrawLineWithoutEndPoint(from, to, GetCacheDebugMat(color));
    }
}




