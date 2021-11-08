#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

public class PackageWizard : EditorWindow
{
    const float k_WindowWidth = 600f;
    const float k_MaxWindowHeight = 800f;
    const float k_ScreenSizeWindowBuffer = 50f;

    [MenuItem("Tools/Package Wizard...")]
    static void CreateWindow()
    {
        PackageWizard wizard = GetWindow<PackageWizard>(true, "Package Wizard", true);

        wizard.minSize = new Vector2(k_WindowWidth, 400);

        Vector2 position = Vector2.zero;
        SceneView sceneView = SceneView.lastActiveSceneView;

        if (sceneView != null)
        {
            position = new Vector2(sceneView.position.x, sceneView.position.y);
        }

        wizard.position = new Rect(position.x + k_ScreenSizeWindowBuffer, position.y + k_ScreenSizeWindowBuffer, k_WindowWidth, Mathf.Min(Screen.currentResolution.height - k_ScreenSizeWindowBuffer, k_MaxWindowHeight));
        wizard.Show();
    }

    readonly GUIContent m_NameContent = new GUIContent("PackageName");
    string InputPackageName = "TestPackage";
    bool CreateRuntimeAsmdef = true;
    bool CreateEditorAsmdef = false;
    string NameExtension = "com.megumin";
    bool AutoRootNamespace = true;
    string RootNamespace = null;

    void OnGUI()
    {
        InputPackageName = EditorGUILayout.TextField(m_NameContent, InputPackageName);
        var path = Path.GetFullPath($"{MeguminUtility4Unity.PackagesPath}/{InputPackageName}");

        CreateRuntimeAsmdef = EditorGUILayout.Toggle("CreateRuntimeAsmdef", CreateRuntimeAsmdef);
        CreateEditorAsmdef = EditorGUILayout.Toggle("CreateEditorAsmdef", CreateEditorAsmdef);

        if (EditorGUILayout.LinkButton("Start with <domain-name-extension>.<company-name> (for example, \"com.example\" \"net.example\")"))
        {
            Application.OpenURL("https://docs.unity3d.com/2020.3/Documentation/Manual/cus-naming.html");
        }
        NameExtension = EditorGUILayout.TextField("NameExtension", NameExtension);

        AutoRootNamespace = EditorGUILayout.Toggle("AutoRootNamespace", AutoRootNamespace);
        if (AutoRootNamespace)
        {
            RootNamespace = $"Megumin.GameFramework.{InputPackageName}";
            EditorGUILayout.LabelField("RootNamespace", RootNamespace);
        }
        else
        {
            RootNamespace = EditorGUILayout.TextField("RootNamespace", RootNamespace);
        }

        EditorGUILayout.Separator();
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
$@"
{{
    ""name"": ""{NameExtension.ToLower()}.{InputPackageName.ToLower()}"",
    ""displayName"": ""{InputPackageName}"",
    
    ""version"": ""0.0.1"",
    ""unity"": ""2019.4"",
    ""description"": ""Wizard Fast Created."",
    ""category"": ""PackageWizard"",
    
    ""documentationUrl"": ""https://example.com/"",
    ""changelogUrl"": ""https://example.com/changelog.html"",
    ""licensesUrl"": ""https://example.com/licensing.html"",
    
    ""hideInEditor"" : false,
    
    ""keywords"": [
        ""PackageWizard"",
        ""{InputPackageName}""
    ],

    ""author"": {{
        ""name"": ""Unity"",
        ""email"": ""unity@example.com"",
        ""url"": ""https://www.unity3d.com""
    }}
}}




";

            File.WriteAllText(path + "/package.json", packageInfo);

            if (CreateRuntimeAsmdef)
            {
                string runtimeasmdef =
$@"{{
    ""name"": ""{InputPackageName}"",
    ""rootNamespace"": ""{RootNamespace}"",
    ""references"": [],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": true,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
}}";

                File.WriteAllText(path + "/Runtime" + $"/{InputPackageName}.asmdef", runtimeasmdef);

                //无法找到构造函数
                //AssemblyDefinitionAsset assembly =
                //    System.Activator.CreateInstance(typeof(AssemblyDefinitionAsset),
                //    BindingFlags.NonPublic| BindingFlags.Instance,
                //    runtimeasmdef)
                //    as AssemblyDefinitionAsset;

                //CreateAsset无法创建.asmdef
                //TextAsset asset = new TextAsset(runtimeasmdef);
                //AssetDatabase.CreateAsset(asset, path + "/Runtime" + $"/{InputPackageName}.asmdef");
                //AssetDatabase.Refresh();

                //无法取得guid
                //AssetDatabase.ImportAsset(path + "/Runtime" + $"/{InputPackageName}.asmdef");
                //var guid = AssetDatabase.GUIDFromAssetPath(path + "/Runtime" + $"/{InputPackageName}.asmdef");
                //Debug.Log(guid);
            }

            if (CreateEditorAsmdef)
            {
                var editorNamespace = RootNamespace;
                if (!string.IsNullOrEmpty(editorNamespace))
                {
                    editorNamespace = $"{editorNamespace}.Editor";
                }

                string editorasmdef =
$@"{{
    ""name"": ""{InputPackageName}.Editor"",
    ""rootNamespace"": ""{editorNamespace}"",
    ""references"": [
        ""{(CreateRuntimeAsmdef ? InputPackageName : "")}""
    ],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": true,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
}}";

                File.WriteAllText(path + "/Editor" + $"/{InputPackageName}.Editor.asmdef", editorasmdef);
            }
        }


    }
}

#endif



