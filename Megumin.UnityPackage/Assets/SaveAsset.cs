using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Megumin;

public class SaveAsset : ScriptableObject, ISerializationCallbackReceiver
{
    [HideInInspector]
    public List<Mesh> Meshs;
    public List<WarpClass> WarpClasses;
    public void OnBeforeSerialize()
    {
        if (WarpClasses != null)
        {
            foreach (var item in WarpClasses)
            {
                if (Meshs.Contains(item.mesh))
                {

                }
                else
                {
                    if (item.mesh)
                    {
                        Meshs.Add(item.mesh);
#if UNITY_EDITOR
                        UnityEditor.AssetDatabase.AddObjectToAsset(item.mesh, this);
#endif
                    }
                }
            }
        }

        if (Meshs != null)
        {
            Meshs.RemoveAll(mesh =>
            {
                if (WarpClasses?.Any(ele => ele.mesh == mesh) ?? false)
                {
                    return false;
                }
                else
                {
                    if (mesh)
                    {
#if UNITY_EDITOR
                        UnityEditor.AssetDatabase.RemoveObjectFromAsset(mesh);
#endif
                    }
                    return true;
                }
            });
        }
    }

    public void OnAfterDeserialize()
    {

    }

#if UNITY_EDITOR
    [EditorButton]
    public void TestAdd()
    {
        var count = 0;
        if (Meshs != null)
        {
            count = Meshs.Count;
        }
        var mesh = new Mesh() { name = $"test{count}" };
        UnityEditor.AssetDatabase.AddObjectToAsset(mesh, this);
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();

        if (Meshs == null)
        {
            Meshs = new List<Mesh>();
        }
        Meshs.Add(mesh);
        if (WarpClasses == null)
        {
            WarpClasses = new List<WarpClass>();
        }
        WarpClasses.Add(new WarpClass() { mesh = mesh });
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
#endif


}

[Serializable]
public class WarpClass
{
    public Mesh mesh;
}

