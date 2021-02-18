using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public partial class MeguminEditorUtility
{
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

        return (AssetBundle)getEditorAssetBundle.Invoke(null, new object[] { });
    }

    static List<(Texture2D Icon, string Name)> EditorIconCache;
    /// <summary>
    /// 取得所有内置Icon
    /// <para></para>
    /// 通过名字加载 <seealso cref="EditorGUIUtility.FindTexture(string)"/>
    /// <para><seealso cref="EditorGUIUtility.IconContent(string)"/></para>
    /// </summary>
    /// <returns></returns>
    public List<(Texture2D Icon, string Name)> GetEditorIcon()
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


#if InvokeString

    static Action<string> InvokeMethod { get; } = CreateInvokeMethod();
    static Action<string> CreateInvokeMethod()
    {
        ImmediateWindow.ShowPackageManagerWindow();
        dynamic immediate = ImmediateWindow.CurrentWindow;

        object console = typeof(ImmediateWindow).GetProperty("Console", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(immediate);

        object output = (console.GetType()).GetProperty("ConsoleOutput", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(console);
        var clear = (output.GetType()).GetMethod("ClearLog");
        clear.Invoke(output, null);

        UnityEngine.UIElements.TextField textField = (console.GetType()).GetProperty("ConsoleInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(console) as UnityEngine.UIElements.TextField;
        
        var invoke = (console.GetType()).GetMethod("CodeEvaluate");

        Action<string> action = (str) =>
        {
            textField.value = str;
            invoke.Invoke(console, null);
        };

        return action;
    }

    public static void ExcuteString(string cmd)
    {
        InvokeMethod?.Invoke(cmd);
    }

#endif


}













