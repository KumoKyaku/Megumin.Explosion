#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Megumin
{
    public class PackageWizard : EditorWindow
    {
        const float k_WindowWidth = 600f;
        const float k_MaxWindowHeight = 480;
        const float k_ScreenSizeWindowBuffer = 50f;

        [MenuItem("Tools/Package Wizard...")]
        static void CreateWindow()
        {
            PackageWizard wizard = GetWindow<PackageWizard>(true, "Package Wizard", true);

            wizard.minSize = new Vector2(k_WindowWidth, k_MaxWindowHeight);

            //Vector2 position = Vector2.zero;
            //SceneView sceneView = SceneView.lastActiveSceneView;

            //if (sceneView != null)
            //{
            //    position = new Vector2(sceneView.position.x, sceneView.position.y);
            //}

            //wizard.position = new Rect(position.x + k_ScreenSizeWindowBuffer, position.y + k_ScreenSizeWindowBuffer, k_WindowWidth, Mathf.Min(Screen.currentResolution.height - k_ScreenSizeWindowBuffer, k_MaxWindowHeight));

            wizard.Show();
        }

        class Prop
        {
            public Prop() { }

            public Prop(Func<string, string> overrideFunc = null)
            {
                GetOverrideValue = overrideFunc;
            }

            public bool IsOverride = false;
            public string Label = "";
            public string OverrideValue = "";

            public string defuaultValue;
            public Func<string, string> GetOverrideValue;

            internal void OnGUI(string input)
            {
                EditorGUILayout.BeginHorizontal();
                IsOverride = EditorGUILayout.Toggle(IsOverride, GUILayout.Width(17));

                EditorGUILayout.LabelField(Label, GUILayout.Width(EditorGUIUtility.labelWidth - 21));
                if (IsOverride)
                {
                    OverrideValue = EditorGUILayout.TextField(OverrideValue);
                }
                else
                {
                    if (GetOverrideValue == null)
                    {
                        defuaultValue = input;
                    }
                    else
                    {
                        defuaultValue = GetOverrideValue?.Invoke(input);
                    }

                    if (string.IsNullOrEmpty(OverrideValue))
                    {
                        OverrideValue = defuaultValue;
                    }
                    EditorGUILayout.LabelField(defuaultValue, new GUIStyle("SelectionRect"));
                }

                EditorGUILayout.EndHorizontal();
            }

            public override string ToString()
            {
                return IsOverride ? OverrideValue : defuaultValue;
            }
        }

        readonly GUIContent m_NameContent = new GUIContent("PackageName");
        string InputPackageName = "TestPackage";

        string Company = "Megumin";

        Prop DisplayName = new Prop() { Label = nameof(DisplayName) };
        Prop NameExtension = new Prop() { Label = nameof(NameExtension) };
        Prop FolderName = new Prop() { Label = nameof(FolderName) };
        Prop AsmdefName = new Prop() { Label = nameof(AsmdefName) };

        bool CreateRuntimeAsmdef = true;
        bool CreateEditorAsmdef = false;
        bool CreateTestsFolder = true;
        bool CreateSamplesFolder = false;
        bool CreateDemosFolder = false;
        bool CreateReadme = true;
        bool CreateChangeLog = true;
        /// <summary>
        /// 不要创建Lisence文件，用户应该从其他地方复制过来。
        /// </summary>
        bool CreateLicense = false;
        bool CreateThirdPartyNotices = true;

        /// <summary>
        /// 打开这个脚本
        /// </summary>
        public void OpenPackageWizardScript()
        {
            var path = AssetDatabase.GUIDToAssetPath("f853b7b701c17ad478a19360697577a9");
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            AssetDatabase.OpenAsset(script);
        }

        void OnGUI()
        {
            if (GUILayout.Button("Edit PackageWizard"))
            {
                OpenPackageWizardScript();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            InputPackageName = EditorGUILayout.TextField(m_NameContent, InputPackageName);

            EditorGUILayout.Separator();
            EditorGUILayout.HelpBox("NameExtension用于包全名前缀,自动变为小写.", MessageType.Info);
            if (EditorGUILayout.LinkButton("Start with <domain-name-extension>.<company-name> (for example, \"com.example\" \"net.example\")"))
            {
                Application.OpenURL("https://docs.unity3d.com/2020.3/Documentation/Manual/cus-naming.html");
            }

            Company = EditorGUILayout.TextField(nameof(Company), Company);

            EditorGUILayout.Separator();
            //显示的名字将. 替换为空格
            DisplayName.OnGUI($"{Company} {InputPackageName.Replace('.', ' ')}");
            NameExtension.OnGUI($"com.{Company.ToLower()}");
            FolderName.OnGUI($"{NameExtension.ToString()?.ToLower()}.{InputPackageName.ToLower()}");

            var path = Path.GetFullPath($"{PathUtility.PackagesPath}/{FolderName}");

            using (new EditorGUI.DisabledScope(!CreateRuntimeAsmdef))
            {
                AsmdefName.OnGUI($"{Company}.{InputPackageName}");
            }

            CreateRuntimeAsmdef = EditorGUILayout.Toggle("CreateRuntimeAsmdef", CreateRuntimeAsmdef);
            CreateEditorAsmdef = EditorGUILayout.Toggle("CreateEditorAsmdef", CreateEditorAsmdef);

            EditorGUILayout.Separator();
            CreateTestsFolder = EditorGUILayout.Toggle(nameof(CreateTestsFolder), CreateTestsFolder);
            CreateSamplesFolder = EditorGUILayout.Toggle(nameof(CreateSamplesFolder), CreateSamplesFolder);
            CreateDemosFolder = EditorGUILayout.Toggle(nameof(CreateDemosFolder), CreateDemosFolder);

            EditorGUILayout.Separator();
            CreateReadme = EditorGUILayout.Toggle(nameof(CreateReadme), CreateReadme);
            CreateChangeLog = EditorGUILayout.Toggle(nameof(CreateChangeLog), CreateChangeLog);
            using (new EditorGUI.DisabledScope(true))
            {
                CreateLicense = EditorGUILayout.Toggle(nameof(CreateLicense), CreateLicense);
            }
            CreateThirdPartyNotices = EditorGUILayout.Toggle(nameof(CreateThirdPartyNotices), CreateThirdPartyNotices);

            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();

            var hasPath = Directory.Exists(path);

            using (new EditorGUI.DisabledScope(!hasPath))
            {
                if (GUILayout.Button("Delete", GUILayout.Width(60f)))
                {
                    if (EditorUtility.DisplayDialog("删除确认",
                                                    "确定要删除本地包么?\n操作不可恢复,请做好备份或使用版本工具.",
                                                    "确定",
                                                    "取消"))
                    {
                        Directory.Delete(path, true);
                        RefreshAsset();
                    }
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.Space(5);

            if (hasPath)
            {
                if (GUILayout.Button("Append", new GUIStyle("flow node 5 on"), GUILayout.Height(30), GUILayout.Width(60f)))
                {
                    CreatePackage(path);
                    RefreshAsset();
                    Close();
                }
            }
            else
            {
                if (GUILayout.Button("Create", new GUIStyle("flow node 1 on"), GUILayout.Height(30), GUILayout.Width(60f)))
                {
                    CreatePackage(path);
                    RefreshAsset();
                    Close();
                }
            }


            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            //通过打开文件夹除非工程刷新
            //EditorGUILayout.HelpBox("创建完成后Editor切换到后台，再切换回来，" +
            //    "触发package导入，没有找到如何自动导入。", MessageType.Info);
        }

        private static void RefreshAsset()
        {
            var process = System.Diagnostics.Process.Start(PathUtility.PackagesPath);
        }

        private void CreatePackage(string path)
        {
            if (string.IsNullOrEmpty(InputPackageName))
            {
                Debug.LogError($"包名不能为空");
                return;
            }

            CreateDirIfNotExist(path);

            CreateDirIfNotExist(path + "/Editor");
            CreateDirIfNotExist(path + "/Runtime");

            if (CreateTestsFolder)
            {
                CreateDirIfNotExist(path + "/Tests");
            }

            if (CreateSamplesFolder)
            {
                CreateDirIfNotExist(path + "/Samples");
            }

            if (CreateDemosFolder)
            {
                CreateDirIfNotExist(path + "/Demos");
            }

            CreatePackageInfoFile(path);

            CreateRuntimeAsmdefFile(path);

            CreateEditorAsmdefFile(path);

            CreateReadmeFile(path);

            CreateChangeLogFile(path);

            CreateCreateThirdPartyNoticesFile(path);
        }

        public void CreateDirIfNotExist(string path)
        {
            if (Directory.Exists(path))
            {

            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        private void CreateCreateThirdPartyNoticesFile(string path)
        {
            if (CreateThirdPartyNotices)
            {
                var filepath = Path.Combine(path, "Third Party Notices.md");
                if (File.Exists(filepath))
                {
                    Debug.LogWarning("Third Party Notices.md exists");
                }
                else
                {
                    string fileStr =
$@"This package contains third-party software components governed by the license(s) indicated below:
---------


";
                    File.WriteAllText(filepath, fileStr);
                }
            }
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
            var filePath = path + "/package.json";

            if (File.Exists(filePath))
            {
                return;
            }

            var version = UnityEditorInternal.InternalEditorUtility.GetFullUnityVersion().Split(".");
            string packageInfo =
$@"{{
    ""name"": ""{NameExtension.ToString().ToLower()}.{InputPackageName.ToLower()}"",
    ""displayName"": ""{DisplayName}"",
    
    ""version"": ""0.0.1"",
    ""unity"": ""{version[0]}.{version[1]}"",
    ""description"": ""Wizard Fast Created."",
    ""category"": ""{Company}"",
    
    ""documentationUrl"": """",
    ""changelogUrl"": """",
    ""licensesUrl"": """",
    
    ""hideInEditor"" : false,
    
    ""keywords"": [
        ""PackageWizard"",
        ""{Company}"",
        ""{InputPackageName}""
    ],

    ""author"": {{
        ""name"": ""KumoKyaku"",
        ""email"": ""479813005@qq.com"",
        ""url"": ""https://github.com/KumoKyaku""
    }}
}}




";

            File.WriteAllText(filePath, packageInfo);
        }

        private void CreateRuntimeAsmdefFile(string path)
        {
            if (CreateRuntimeAsmdef)
            {
                var filePath = path + "/Runtime" + $"/{AsmdefName}.asmdef";

                if (File.Exists(filePath))
                {
                    return;
                }

                string runtimeasmdef =
$@"{{
    ""name"": ""{AsmdefName}"",
    ""rootNamespace"": ""{AsmdefName}"",
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

                File.WriteAllText(filePath, runtimeasmdef);

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
                var dir = path + "/Editor";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(path);
                }

                var editorNamespace = AsmdefName.ToString();
                if (!string.IsNullOrEmpty(editorNamespace))
                {
                    editorNamespace = $"{editorNamespace}.Editor";
                }

                var filePath = path + "/Editor" + $"/{editorNamespace}.asmdef";

                if (File.Exists(filePath))
                {
                    Debug.LogWarning($"{editorNamespace}.asmdef exists");
                }
                else
                {
                    string editorasmdef =
$@"{{
    ""name"": ""{editorNamespace}"",
    ""rootNamespace"": ""{editorNamespace}"",
    ""references"": [
        ""{(CreateRuntimeAsmdef ? AsmdefName : "")}""
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

                    File.WriteAllText(filePath, editorasmdef);
                }
            }
        }
    }

}

#endif



