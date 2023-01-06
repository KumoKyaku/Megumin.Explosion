using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static partial class MeguminEditorUtility
{
    /// <summary>
    /// 查找所有内置Icon https://assetstore.unity.com/packages/tools/utilities/unity-internal-icons-70496
    /// </summary>
    public static List<(GUIContent icon, GUIContent name)> FindIcons()
    {
        List<(GUIContent icon, GUIContent name)> _icons
            = new List<(GUIContent icon, GUIContent name)>();
        _icons.Clear();

        Texture2D[] t = Resources.FindObjectsOfTypeAll<Texture2D>();
        foreach (Texture2D x in t)
        {
            if (x.name.Length == 0)
                continue;

            if (x.hideFlags != HideFlags.HideAndDontSave && x.hideFlags != (HideFlags.HideInInspector | HideFlags.HideAndDontSave))
                continue;

            if (!EditorUtility.IsPersistent(x))
                continue;

            /* This is the *only* way I have found to confirm the icons are indeed unity builtin. Unfortunately
             * it uses LogError instead of LogWarning or throwing an Exception I can catch. So make it shut up. */
            Debug.unityLogger.logEnabled = false;
            GUIContent gc = EditorGUIUtility.IconContent(x.name);
            Debug.unityLogger.logEnabled = true;

            if (gc == null)
                continue;
            if (gc.image == null)
                continue;

            _icons.Add((gc, new GUIContent(x.name)));
        }

        _icons.Sort((x, y) => { return x.name.text.CompareTo(y.name.text); });
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        return _icons;
    }

    /// <summary>
    /// https://github.com/halak/unity-editor-icons/blob/master/Assets/Editor/IconMiner.cs#L149
    /// </summary>
    /// <returns></returns>
    public static AssetBundle GetEditorAssetBundle()
    {
        var editorGUIUtility = typeof(EditorGUIUtility);
        var getEditorAssetBundle = editorGUIUtility.GetMethod(
            "GetEditorAssetBundle",
            BindingFlags.NonPublic | BindingFlags.Static);

        return (AssetBundle)getEditorAssetBundle.Invoke(null, null);
    }

    static List<(Texture2D Icon, string Name)> EditorIconCache;
    /// <summary>
    /// 取得所有内置Icon
    /// <para></para>
    /// 通过名字加载 <seealso cref="EditorGUIUtility.FindTexture(string)"/>
    /// <para><seealso cref="EditorGUIUtility.IconContent(string)"/></para>
    /// </summary>
    /// <returns></returns>
    public static List<(Texture2D Icon, string Name)> GetEditorIcon()
    {
        if (EditorIconCache == null)
        {
            EditorIconCache = new List<(Texture2D Icon, string Name)>();
            var editorAssetBundle = GetEditorAssetBundle();
            var iconsPath = GetIconsPath();
            foreach (var assetName in EnumerateIcons(editorAssetBundle, iconsPath))
            {
                var icon = editorAssetBundle.LoadAsset<Texture2D>(assetName);
                if (icon == null)
                    continue;
                EditorIconCache.Add((icon, assetName));
            }
        }
        return EditorIconCache;
    }

    private static IEnumerable<string> EnumerateIcons(AssetBundle editorAssetBundle, string iconsPath)
    {
        foreach (var assetName in editorAssetBundle.GetAllAssetNames())
        {
            if (assetName.StartsWith(iconsPath, StringComparison.OrdinalIgnoreCase) == false)
                continue;
            if (assetName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) == false &&
                assetName.EndsWith(".asset", StringComparison.OrdinalIgnoreCase) == false)
                continue;

            yield return assetName;
        }
    }

    /// <summary>
    /// 加载 unity_builtin_extra 里的资源
    /// <para>unity default resources 里的资源直接用<see cref="Resources.GetBuiltinResource{T}(string)"/></para>
    /// unity editor resources 没有找到对应API，或者路径不对，没有加载成功。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T GetBuiltinExtraResource<T>(string path)
        where T : UnityEngine.Object
    {

#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.GetBuiltinExtraResource<T>(path);
#else
        return Resources.GetBuiltinResource<T>(path);
#endif

    }

    private static string GetIconsPath()
    {
#if UNITY_2018_3_OR_NEWER
        return UnityEditor.Experimental.EditorResources.iconsPath;
#else
            var assembly = typeof(EditorGUIUtility).Assembly;
            var editorResourcesUtility = assembly.GetType("UnityEditorInternal.EditorResourcesUtility");

            var iconsPathProperty = editorResourcesUtility.GetProperty(
                "iconsPath",
                BindingFlags.Static | BindingFlags.Public);

            return (string)iconsPathProperty.GetValue(null, new object[] { });
#endif
    }

    /// <summary>
    /// 反射同步IDE
    /// </summary>
    public static void SyncSolution()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(EditorApplication));
        var type = assembly.GetType("UnityEditor.SyncVS");
        var sync = type.GetMethod("SyncSolution");
        sync.Invoke(null, new object[0]);
    }

    [InitializeOnLoadMethod]
    static void AddRtostring()
    {
        Megumin.Utility.MemberValueToStringCallbackHandle = new UnityToStringCallbackHandle();
    }

    class UnityToStringCallbackHandle
        : Megumin.Utility.IToStringReflectionMemberValueToStringCallbackHandle
    {
        public string ToStringReflection(object value)
        {
            if (value is UnityEngine.Object o && o)
            {
                //检查o，如果null 或者distroy missref调用超链接也没意义
                return o.ToHyperlink();
            }
            return value?.ToString();
        }
    }

    [MenuItem("GameObject/Megumin/ForceShowHideGameObject", priority = 15)]
    static public void ForceShowHideGameObject()
    {
        Scene scene = SceneManager.GetActiveScene();
        var objs = scene.GetRootGameObjects();

        static void ShowHiddin(Transform trans)
        {
            if (trans.hideFlags.HasFlag(HideFlags.HideInHierarchy))
            {
                trans.hideFlags &= ~HideFlags.HideInHierarchy;
            }

            if (trans.hideFlags.HasFlag(HideFlags.HideInInspector))
            {
                trans.hideFlags &= ~HideFlags.HideInInspector;
            }

            if (trans.hideFlags.HasFlag(HideFlags.HideAndDontSave))
            {
                trans.hideFlags &= ~HideFlags.HideAndDontSave;
            }

            foreach (Transform item in trans)
            {
                ShowHiddin(item);
            }
        }

        foreach (var obj in objs)
        {
            ShowHiddin(obj.transform);
        }
    }

    [MenuItem("Assets/Create/⚝Select ScriptableObject", priority = 35)]
    public static void CreateSelectScriptableObjectAsset()
    {
        var res = GetScriptObjectType();
        if (res != null)
        {
            var so = ScriptableObject.CreateInstance(res);

            MethodInfo getActiveFolderPath =
                typeof(ProjectWindowUtil).GetMethod("GetActiveFolderPath",
                                                    BindingFlags.Static | BindingFlags.NonPublic);

            string folderPath = (string)getActiveFolderPath.Invoke(null, null);
            var fn = folderPath.CreateFileName(res.Name, ".asset");
            AssetDatabase.CreateAsset(so, fn);
            AssetDatabase.Refresh();
            Debug.Log($"CreateSelectScriptableObjectAsset : {res.FullName}");
        }
    }

    [MenuItem("Assets/Create/⚝Select ScriptableObject", true, priority = 35)]
    public static bool CreateSelectScriptableObjectAssetValidateFunction()
    {
        var res = GetScriptObjectType();
        return res != null;
    }

    public static Type GetScriptObjectType()
    {
        if (Selection.activeObject is MonoScript mono)
        {
            var type = mono.GetClass();
            if (typeof(ScriptableObject).IsAssignableFrom(type))
            {
                return type;
            }
        }

        return null;
    }

    [MenuItem("Assets/※Open in VSCode", priority = 19)]
    public static void OpeninVSCode()
    {
        ///https://stackoverflow.com/questions/61937342/launch-visual-studio-code-programmatically
        ///https://learn.microsoft.com/zh-cn/dotnet/api/system.diagnostics.processstartinfo?view=net-7.0

        if (Selection.activeObject)
        {
            var path = Selection.activeObject.GetAbsoluteFilePath();

            path = Path.GetFullPath(path);

            //System.Diagnostics.Process.Start("code.exe", path); //NotWork and Crash!!!

            try
            {
                using (System.Diagnostics.Process myProcess = new System.Diagnostics.Process())
                {
                    //var vscodePath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Microsoft VS Code\");
                    //if (Directory.Exists(vscodePath))
                    //{
                    //    myProcess.StartInfo.WorkingDirectory = vscodePath;
                    //    myProcess.StartInfo.Environment.Add("UnityOpeninVSCode_C", vscodePath);
                    //}

                    //var p2 = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Microsoft VS Code\");
                    //if (Directory.Exists(p2))
                    //{
                    //    myProcess.StartInfo.Environment.Add("UnityOpeninVSCode_ProgramFiles", Path.GetFileName(p2));
                    //}

                    //var p3 = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft VS Code\");
                    //if (Directory.Exists(p3))
                    //{
                    //    myProcess.StartInfo.Environment.Add("UnityOpeninVSCode_ProgramFilesX86", Path.GetFileName(p3));
                    //}

                    myProcess.StartInfo.UseShellExecute = true; //must true
                    myProcess.StartInfo.FileName = "code";
                    myProcess.StartInfo.Arguments = path;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.StartInfo.ErrorDialog = true;
                    myProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    myProcess.Start();
                    Debug.Log($"※Open in VSCode : {path}");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}










