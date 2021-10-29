using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

public class SaveAsset : ScriptableObject,ISerializationCallbackReceiver
{
    public List<WarpClass> WarpClasses;
    [HideInInspector]
    public List<Mesh> Meshs;
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
                        AssetDatabase.AddObjectToAsset(item.mesh, this);
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
                        AssetDatabase.RemoveObjectFromAsset(mesh);
                    }
                    return true;
                }
            });
        }
    }

    public void OnAfterDeserialize()
    {
        
    }

    [EditorButton]
    public void TestAdd()
    {
        WarpClasses.Add(new WarpClass() { mesh = new Mesh() { name = $"test{WarpClasses.Count}"} });
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

[Serializable]
public class WarpClass 
{
    public Mesh mesh;
}

