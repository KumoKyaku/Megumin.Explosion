using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteAlways]
[DefaultExecutionOrder(9000)]
[SelectionBase]
public class ColliderRenderer : MonoBehaviour
{
    const string OverrideName = "[Override]";
    /// <summary>
    /// 全局显示开关
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

    public bool ShowOnRuntime = true;
    /// <summary>
    /// 即使碰撞盒没有开启也要强制显示
    /// </summary>
    public bool ForceShowOnDisable = false;

    [ReadOnlyInInspector]
    public List<Collider> Colliders = new List<Collider>();
    [ReadOnlyInInspector]
    public List<ColliderRenderer> SubShowers = new List<ColliderRenderer>();
    [ReadOnlyInInspector]
    public ColliderRenderer Parent;
    void Start()
    {
        if (GlobalToggle == null)
        {
            GlobalToggle = new Pref<bool>(nameof(ColliderRenderer), true);
        }

        ReCollect();
        //#if UNITY_EDITOR
        //        UnityEditor.SceneView.duringSceneGui += SceneView_duringSceneGui;
        //#endif
    }

    //#if UNITY_EDITOR

    //    //这里得OnSceneGUI 在Game中仍然会显示
    //    private void SceneView_duringSceneGui(SceneView obj)
    //    {
    //        if (GlobalToggle == null)
    //        {
    //            return;
    //        }

    //        if (GlobalToggle && !ShowOnRuntime)
    //        {
    //            DrawCollider();
    //        }
    //    }

    //#endif

    void OnDestroy()
    {
        //#if UNITY_EDITOR
        //        UnityEditor.SceneView.duringSceneGui -= SceneView_duringSceneGui;
        //#endif
    }


    [EditorButton]
    public void SwitchGlobalToggle()
    {
        if (GlobalToggle == null)
        {
            GlobalToggle = new Pref<bool>(nameof(ColliderRenderer), true);
        }
        GlobalToggle.Value = !GlobalToggle;
    }

    [EditorButton]
    void ReCollect()
    {
        SubShowers.Clear();
        GetComponentsInChildren<ColliderRenderer>(SubShowers);
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
            Parent = transform.parent.GetComponentInParent<ColliderRenderer>();
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

    static readonly Vector3 MeshDrawOffset = new Vector3(0, 0.025f, 0);

    void LateUpdate()
    {
        bool needReCollect = false;

        if (GlobalToggle == null)
        {
            GlobalToggle = new Pref<bool>(nameof(ColliderRenderer), true);
        }

        if (GlobalToggle && ShowOnRuntime)
        {
            needReCollect = DrawCollider();
        }

        if (needReCollect)
        {
            ReCollect();
        }
    }

    internal bool DrawCollider()
    {
        bool needReCollect = false;
        foreach (var collider in Colliders)
        {
            if (collider)
            {
                if (ForceShowOnDisable || collider.enabled)
                {
                    if (collider is BoxCollider box)
                    {
                        box.Draw(Material);
                    }
                    else if (collider is CapsuleCollider capsule)
                    {
                        capsule.Draw(Material);
                    }
                    else if (collider is SphereCollider sphere)
                    {
                        sphere.Draw(Material);
                    }
                    else if (collider is MeshCollider meshCollider)
                    {
                        meshCollider.Draw(Material, MeshDrawOffset);
                    }
                }
            }
            else
            {
                needReCollect = true;
            }
        }

        return needReCollect;
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

    public void OverrideColor(Color? color, bool force = false)
    {
        if (color.HasValue)
        {
            OverrideColor((Color)color, force);
        }
        else
        {
            ResetMat();
        }
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
        CacheOverrideColor = null;
        overrideMat = null;
        this.AssetDataSetDirty();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!ShowOnRuntime)
        {
            foreach (var collider in Colliders)
            {
                if (collider)
                {
                    if (ForceShowOnDisable || collider.enabled)
                    {
                        if (collider is BoxCollider box)
                        {
                            box.GizmoDraw(Material.color);
                        }
                        else if (collider is CapsuleCollider capsule)
                        {
                            capsule.GizmoDraw(Material.color);
                        }
                        else if (collider is SphereCollider sphere)
                        {
                            sphere.GizmoDraw(Material.color);
                        }
                        else if (collider is MeshCollider meshCollider)
                        {
                            meshCollider.GizmoDraw(Material.color, MeshDrawOffset);
                        }
                    }
                }
            }
        }
    }
#endif

}

//#if UNITY_EDITOR

// 这里得OnSceneGUI 在Game中仍然会显示

//[CustomEditor(typeof(ColliderShower), true)]
//public class ColliderShowerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        this.DrawInspectorMethods();
//    }

//    void OnSceneGUI()
//    {
//        ColliderShower shower = (ColliderShower)target;
//        if (ColliderShower.GlobalToggle == null)
//        {
//            return;
//        }

//        if (ColliderShower.GlobalToggle && !shower.ShowOnRuntime)
//        {
//            shower.DrawCollider();
//        }
//    }
//}

//#endif


