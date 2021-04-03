using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
[DefaultExecutionOrder(9000)]
[SelectionBase]
public class ColliderShower : MonoBehaviour
{
    const string OverrideName = "[Override]";
    /// <summary>
    /// ȫ����ʾ����
    /// </summary>
    public static Pref<bool> GlobalToggle;
    public Material DefaultMat;
    [ReadOnlyInInspector]
    public Material overrideMat;
    public Material Material
    {
        get
        {
            if (overrideMat)
            {
                return overrideMat;
            }
            return DefaultMat;
        }
    }
    public bool ShowLable = false;
    /// <summary>
    /// ��ʹ��ײ��û�п���ҲҪǿ����ʾ
    /// </summary>
    public bool ForceShowOnDisable = false;

    [ReadOnlyInInspector]
    public List<Collider> Colliders = new List<Collider>();
    [ReadOnlyInInspector]
    public List<ColliderShower> SubShowers = new List<ColliderShower>();
    [ReadOnlyInInspector]
    public ColliderShower Parent;
    void Start()
    {
        if (GlobalToggle == null)
        {
            GlobalToggle = new Pref<bool>(nameof(ColliderShower), true);
        }

        ReCollect();
    }


    [EditorButton]
    public void SwitchGlobalToggle()
    {
        if (GlobalToggle == null)
        {
            GlobalToggle = new Pref<bool>(nameof(ColliderShower), true);
        }
        GlobalToggle.Value = !GlobalToggle;
    }

    [EditorButton]
    void ReCollect()
    {
        SubShowers.Clear();
        GetComponentsInChildren<ColliderShower>(SubShowers);
        SubShowers.RemoveAll(ele => ele == this);

        Colliders.Clear();
        GetComponentsInChildren(Colliders);
        Colliders.RemoveAll(ele =>
        {
            foreach (var item in SubShowers)
            {
                if (item.Colliders.Contains(ele))
                {
                    return true;
                }
            }
            return false;
        });

        if (transform.parent)
        {
            Parent = transform.parent.GetComponentInParent<ColliderShower>();
        }
    }

    [EditorButton]
    void ParentReCollect()
    {
        if (Parent)
        {
            Parent.ReCollect();
        }
    }

    [EditorButton]
    void RemoveSubShowers()
    {
        if (SubShowers != null)
        {
            foreach (var item in SubShowers)
            {
                if (item)
                {
#if UNITY_EDITOR
                    DestroyImmediate(item);
#else
                    Destroy(item);
#endif
                }
            }

            ReCollect();
        }
    }

    void LateUpdate()
    {
        bool needReCollect = false;

        if (GlobalToggle == null)
        {
            GlobalToggle = new Pref<bool>(nameof(ColliderShower), true);
        }

        if (GlobalToggle)
        {
            foreach (var collider in Colliders)
            {
                if (collider)
                {
                    if (ForceShowOnDisable || collider.enabled)
                    {
                        if (collider is BoxCollider box)
                        {
                            DrawBox(box);
                        }
                        else if (collider is CapsuleCollider capsule)
                        {
                            DrawCapsule(capsule);
                        }
                        else if (collider is SphereCollider sphere)
                        {
                            DrawSphere(sphere);
                        }
                        else if (collider is MeshCollider meshCollider)
                        {
                            DrawMesh(meshCollider);
                        }
                    }
                }
                else
                {
                    needReCollect = true;
                }
            }
        }

        if (needReCollect)
        {
            ReCollect();
        }
    }

    private void DrawMesh(MeshCollider meshCollider)
    {
        var mesh = meshCollider.sharedMesh;
        Transform trans = meshCollider.transform;
        //̧��һ���
        var offset = new Vector3(0, 0.025f, 0);
        var matri = Matrix4x4.identity;
        var size = Vector3.one;
        matri.SetTRS(trans.position + offset, trans.rotation, size);
        DrawMesh(mesh, matri);
    }

    private void DrawSphere(SphereCollider sphere)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Sphere);
        Transform trans = sphere.transform;
        var offset = trans.localToWorldMatrix.MultiplyVector(sphere.center);
        var matri = Matrix4x4.identity;
        var size = sphere.radius * 2 * Vector3.one;
        matri.SetTRS(trans.position + offset, trans.rotation, size);
        DrawMesh(mesh, matri);
    }

    private void DrawCapsule(CapsuleCollider capsule)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Capsule);
        Transform trans = capsule.transform;
        var offset = trans.localToWorldMatrix.MultiplyVector(capsule.center);
        var matri = Matrix4x4.identity;
        var size = new Vector3(capsule.radius * 2, capsule.height / 2, capsule.radius * 2);
        matri.SetTRS(trans.position + offset, trans.rotation, size);
        DrawMesh(mesh, matri);
    }

    private void DrawBox(BoxCollider box)
    {
        var mesh = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Cube);
        Transform trans = box.transform;
        var offset = trans.localToWorldMatrix.MultiplyVector(box.center);
        var matri = Matrix4x4.identity;
        matri.SetTRS(trans.position + offset, trans.rotation, box.size);
        DrawMesh(mesh, matri);
    }

    private void DrawMesh(Mesh mesh, Matrix4x4 matri)
    {
        Graphics.DrawMesh(mesh,
                          matri,
                          Material,
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

    [NonSerialized]
    Color? CacheOverrideColor = null;
    [EditorButton]
    public void OverrideColor(Color color, bool force = false)
    {
        if (CacheOverrideColor == color && force == false)
        {
            return;
        }

        overrideMat = Instantiate(DefaultMat);
        overrideMat.name = OverrideName;
        overrideMat.color = color;
        CacheOverrideColor = color;
        this.AssetDataSetDirty();
    }

    [EditorButton]
    public void CopyChildColor(int index = 0, bool force = false)
    {
        if (SubShowers?.Count > index)
        {
            var mat = SubShowers[index]?.overrideMat;
            if (mat)
            {
                if (force || !overrideMat)
                {
                    overrideMat = Instantiate(mat);
                    overrideMat.name = OverrideName;
                    this.AssetDataSetDirty();
                }
            }
        }
    }

    [EditorButton]
    public void InhertParentColor(bool force = false)
    {
        if (Parent)
        {
            if (Parent.overrideMat)
            {
                if (force || !overrideMat)
                {
                    overrideMat = Instantiate(Parent.overrideMat);
                    overrideMat.name = OverrideName;
                    this.AssetDataSetDirty();
                }
            }
        }
    }

    [EditorButton]
    public void ResetMat()
    {
        overrideMat = null;
        this.AssetDataSetDirty();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (Colliders != null && ShowLable && enabled)
        {
            foreach (var collider in Colliders)
            {
                if (collider)
                {
                    UnityEditor.Handles.Label(collider.transform.position,
                        $"��ײ��:[{collider.name}|{collider.tag}]");
                }
            }
        }
    }
#endif

}



