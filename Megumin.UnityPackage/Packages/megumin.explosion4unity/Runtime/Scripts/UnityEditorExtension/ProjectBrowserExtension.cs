using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProjectBrowserExtension_D0437BD7A7FB41AD9B3FFF6F4DFF7F83
{

#if UNITY_EDITOR
    public static Type ProjectBrowserType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ProjectBrowser");
    public static FieldInfo SearchFieldText = ProjectBrowserType.GetField("m_SearchFieldText", (BindingFlags)(-1));
    public static MethodInfo SearchMethod = ProjectBrowserType.GetMethod("UpdateSearchDelayed", (BindingFlags)(-1));
#endif

    public static void ProjectBrowserSearch(string search)
    {
#if UNITY_EDITOR

        var projectBrowser = ProjectBrowserType.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public).GetValue(null);
        SearchFieldText.SetValue(projectBrowser, search);
        SearchMethod.Invoke(projectBrowser, null);

        //Debug.LogError(projectBrowser);

        //dynamic受程序集可见性影响
        //projectBrowser.m_SearchFieldText = "Test";
        //Debug.LogError(projectBrowser.m_SearchAllAssets);
#endif
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Fast Search By Type", false, 21)]
#endif
    public static void FastSearchByType()
    {
#if UNITY_EDITOR
        var objectTarget = Selection.activeObject;
        if (objectTarget && Selection.assetGUIDs.Length > 0)
        {
            ProjectBrowserSearch($"t:{objectTarget.GetType().Name}");
        }
#endif
    }
}


