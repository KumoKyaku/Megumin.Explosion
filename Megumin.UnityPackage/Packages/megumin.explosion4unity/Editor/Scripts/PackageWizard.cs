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
    bool AutoFullName = true;
    string FullName = null;
    bool CreateReadme = true;
    bool CreateChangeLog = true;

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

        EditorGUILayout.HelpBox("FullName用于创建asmdef程序集文件名,name和rootNamespace。", MessageType.Info);
        AutoFullName = EditorGUILayout.Toggle("AutoFullName", AutoFullName);
        if (AutoFullName)
        {
            FullName = $"Megumin.GameFramework.{InputPackageName}";
            EditorGUILayout.LabelField("FullName", FullName);
        }
        else
        {
            FullName = EditorGUILayout.TextField("FullName", FullName);
        }

        EditorGUILayout.Separator();
        CreateReadme = EditorGUILayout.Toggle(nameof(CreateReadme), CreateReadme);
        CreateChangeLog = EditorGUILayout.Toggle(nameof(CreateChangeLog), CreateChangeLog);

        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Delete", GUILayout.Width(60f)))
        {
            Directory.Delete(path, true);
            RefreshAsset();
        }

        GUILayout.FlexibleSpace();
        GUILayout.Space(5);

        if (GUILayout.Button("Create", GUILayout.Width(60f)))
        {
            CreatePackageFolder(path);
            RefreshAsset();
            Close();
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);
        //通过打开文件夹除非工程刷新
        //EditorGUILayout.HelpBox("创建完成后Editor切换到后台，再切换回来，" +
        //    "触发package导入，没有找到如何自动导入。", MessageType.Info);
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
        }
        else
        {
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "/Editor");
            Directory.CreateDirectory(path + "/Runtime");
            Directory.CreateDirectory(path + "/Tests");

            CreatePackageInfoFile(path);

            CreateRuntimeAsmdefFile(path);

            CreateEditorAsmdefFile(path);
        }

        CreateReadmeFile(path);

        CreateChangeLogFile(path);
    }

    private void CreateChangeLogFile(string path)
    {
        if (CreateChangeLog)
        {
            var filepath = Path.Combine(path, "CHANGELOG.md");
            if (File.Exists(filepath))
            {
                Debug.LogWarning("CHANGELOG.md exists");
            }
            else
            {
                var dataStr = System.DateTimeOffset.Now.ToString("yyyy-MM-dd");
                string fileStr =
$@"# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

<!--
## [Unreleased] - YYYY-MM-NN

### Added   
### Changed  
### Deprecated  
### Removed  
### Fixed  
### Security  
-->

---

## [Unreleased] - YYYY-MM-NN

## [0.0.1] - {dataStr}
PackageWizard Fast Created.

";
                File.WriteAllText(filepath, fileStr);
            }
        }
    }

    private void CreateReadmeFile(string path)
    {
        if (CreateReadme)
        {
            var filepath = Path.Combine(path, "README.md");
            if (File.Exists(filepath))
            {
                Debug.LogWarning("README.md exists");
            }
            else
            {
                string readmeStr =
$@"# {InputPackageName}
PackageWizard Fast Created.

";
                File.WriteAllText(filepath, readmeStr);
            }
        }
    }

    private void CreatePackageInfoFile(string path)
    {
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
    }

    private void CreateRuntimeAsmdefFile(string path)
    {
        if (CreateRuntimeAsmdef)
        {
            string runtimeasmdef =
$@"{{
    ""name"": ""{FullName}"",
    ""rootNamespace"": ""{FullName}"",
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

            File.WriteAllText(path + "/Runtime" + $"/{FullName}.asmdef", runtimeasmdef);

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
    }

    private void CreateEditorAsmdefFile(string path)
    {
        if (CreateEditorAsmdef)
        {
            var editorNamespace = FullName;
            if (!string.IsNullOrEmpty(editorNamespace))
            {
                editorNamespace = $"{editorNamespace}.Editor";
            }

            string editorasmdef =
$@"{{
    ""name"": ""{editorNamespace}"",
    ""rootNamespace"": ""{editorNamespace}"",
    ""references"": [
        ""{(CreateRuntimeAsmdef ? FullName : "")}""
    ],
    ""includePlatforms"": [
        ""Editor""
    ],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": true,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
}}";

            File.WriteAllText(path + "/Editor" + $"/{editorNamespace}.asmdef", editorasmdef);
        }
    }
}

#endif



