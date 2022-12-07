#if UNITY_EDITOR && !Megumin_Explosion4Unity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class SerializedPropertyExtension_7A82EFA57627471BB151327BD58CEDA3
{
    static Dictionary<string, string> cacheSelectResult = new Dictionary<string, string>();
    public static bool SelectPath(this SerializedProperty property, bool selectClick, out string selectedPath, bool isFolder = true, string exetension = default)
    {
        return SelectPath(property.propertyPath, property.stringValue, selectClick, out selectedPath, isFolder, exetension);
    }

    public static bool SelectPath(string selectkey, string orignalPath, bool selectClick, out string selectedPath, bool isFolder = true, string exetension = default)
    {
        if (selectClick)
        {
            string dir = Path.Combine(MeguminUtility4Unity.ProjectPath, orignalPath);
            dir = Path.GetDirectoryName(dir);
            dir = Path.GetFullPath(dir);

            if (isFolder)
            {
                var path = EditorUtility.OpenFolderPanel("选择文件夹", dir, "");
                cacheSelectResult[selectkey] = path;
                //property.stringValue = path;
                GUIUtility.ExitGUI();
            }
            else
            {
                var path = EditorUtility.OpenFilePanel("选择文件", dir, exetension);
                cacheSelectResult[selectkey] = path;
                //property.stringValue = path;
                GUIUtility.ExitGUI();
            }
        }

        if (cacheSelectResult.TryGetValue(selectkey, out var res))
        {
            cacheSelectResult.Remove(selectkey);
            selectedPath = res;
            return true;
        }
        selectedPath = string.Empty;
        return false;
    }
}

#endif



