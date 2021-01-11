using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.IO;

public class PackageWizard : EditorWindow
{
    const float k_WindowWidth = 500f;
    const float k_MaxWindowHeight = 800f;
    const float k_ScreenSizeWindowBuffer = 50f;

    [MenuItem("Window/Package Wizard...")]
    static void CreateWindow()
    {
        PackageWizard wizard = GetWindow<PackageWizard>(true, "Package Wizard", true);

        Vector2 position = Vector2.zero;
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
            position = new Vector2(sceneView.position.x, sceneView.position.y);
        wizard.position = new Rect(position.x + k_ScreenSizeWindowBuffer, position.y + k_ScreenSizeWindowBuffer, k_WindowWidth, Mathf.Min(Screen.currentResolution.height - k_ScreenSizeWindowBuffer, k_MaxWindowHeight));

        wizard.Show();
    }

    readonly GUIContent m_NameContent = new GUIContent("PackageName");
    string InputPackageName = "";
    void OnGUI()
    {
        InputPackageName = EditorGUILayout .TextField(m_NameContent, InputPackageName);
        var path = Path.GetFullPath($"{MeguminUtility4Unity.PackagesPath}/{InputPackageName}");
        if (GUILayout.Button("Create", GUILayout.Width(60f)))
        {
            CreatePackageFolder(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Close();
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Delete", GUILayout.Width(60f)))
        {
            Directory.Delete(path,true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        GUILayout.Space(5);
        EditorGUILayout.HelpBox("������ɺ�Editor�л�����̨�����л�������" +
            "����package���룬û���ҵ�����Զ����롣", MessageType.Info);
    }

    private void CreatePackageFolder(string path)
    {
        if (string.IsNullOrEmpty(InputPackageName))
        {
            Debug.LogError($"��������Ϊ��");
            return;
        }

        if (Directory.Exists(path))
        {
            Debug.LogError($"���Ѵ���");
            return;
        }
        else
        {
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "/Editor");
            Directory.CreateDirectory(path + "/Runtime");
            Directory.CreateDirectory(path + "/Tests");
            string packageInfo = 
@$"
{{
    ""name"": ""{InputPackageName.ToLower()}"",
    ""displayName"": ""{InputPackageName}"",
    ""version"": ""0.0.1"",
    ""unity"": ""2019.4"",
    ""description"": ""Wizard Fast Created."",
    ""type"": ""library"",
    ""hideInEditor"" : false
}}

";
            File.WriteAllText(path+"/package.json", packageInfo);
        }


    }


}