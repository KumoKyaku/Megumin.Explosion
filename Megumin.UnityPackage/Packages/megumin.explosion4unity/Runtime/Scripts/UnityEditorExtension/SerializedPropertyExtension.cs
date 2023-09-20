#if !MEGUMIN_Common && UNITY_EDITOR && !Megumin_Explosion4Unity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Megumin;

/// <summary>
/// 为什么要包装一次，因为在PropertyDrawer里，有一些等同于异步的API没办法使用。
/// 比如EditorUtility.DisplayDialog  EditorUtility.OpenFolderPanel
/// 这些函数返回的时候SerializedProperty的值已经不可以在访问了。
/// </summary>
public static class SerializedPropertyExtension_7A82EFA57627471BB151327BD58CEDA3
{

    #region SelectPath

    static Dictionary<string, string> cacheSelectResult = new Dictionary<string, string>();
    public static bool SelectPath(this SerializedProperty property, bool display, out string selectedPath, bool isFolder = true, string exetension = default)
    {
        return SelectPath(property.propertyPath, property.stringValue, display, out selectedPath, isFolder, exetension);
    }

    public static bool SelectPath(string selectkey, string orignalPath, bool display, out string selectedPath, bool isFolder = true, string exetension = default)
    {
        if (display)
        {
            string dir = Path.Combine(PathUtility.ProjectPath, orignalPath);
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

    #endregion

    #region DisplayDialog

    static Dictionary<string, bool> cacheDisplayDialogResult = new Dictionary<string, bool>();
    public static bool DisplayDialog(this SerializedProperty property, bool display,
        string title, string message, string ok, string cancel = "")
    {
        if (display)
        {
            var res = EditorUtility.DisplayDialog(title, message, ok, cancel);
            cacheDisplayDialogResult[property.propertyPath] = res;
            GUIUtility.ExitGUI();
        }

        if (cacheDisplayDialogResult.TryGetValue(property.propertyPath, out var result))
        {
            cacheDisplayDialogResult.Remove(property.propertyPath);
            return result;
        }

        return false;
    }

    #endregion
}

#endif



