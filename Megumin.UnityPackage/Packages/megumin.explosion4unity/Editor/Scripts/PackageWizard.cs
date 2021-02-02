#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

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
        InputPackageName = EditorGUILayout.TextField(m_NameContent, InputPackageName);
        var path = Path.GetFullPath($"{MeguminUtility4Unity.PackagesPath}/{InputPackageName}");
        if (GUILayout.Button("Create", GUILayout.Width(60f)))
        {
            CreatePackageFolder(path);
            RefreshAsset();
            Close();
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Delete", GUILayout.Width(60f)))
        {
            Directory.Delete(path, true);
            RefreshAsset();
        }

        GUILayout.Space(5);
        EditorGUILayout.HelpBox("创建完成后Editor切换到后台，再切换回来，" +
            "触发package导入，没有找到如何自动导入。", MessageType.Info);
    }

    private static void RefreshAsset()
    {
        var process = System.Diagnostics.Process.Start(MeguminUtility4Unity.PackagesPath);
    }

    private void CreatePackageFolder(string path)
    {
        if (string.IsNullOrEmpty(InputPackageName))
        {
            Debug.LogError($"包名不能为空");
            return;
        }

        if (Directory.Exists(path))
        {
            Debug.LogError($"包已存在");
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
    ""author"": ""PackageWizard"",
    ""version"": ""0.0.1"",
    ""unity"": ""2019.4"",
    ""description"": ""Wizard Fast Created."",
    ""category"": ""PackageWizard"",
    ""hideInEditor"" : false
}}

";
            File.WriteAllText(path + "/package.json", packageInfo);
        }


    }
}

#endif
