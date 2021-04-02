using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static partial class MeguminEditorUtility
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

        return (AssetBundle)getEditorAssetBundle.Invoke(null, null);
    }

    static List<(Texture2D Icon, string Name)> EditorIconCache;
    /// <summary>
    /// ȡ����������Icon
    /// <para></para>
    /// ͨ�����ּ��� <seealso cref="EditorGUIUtility.FindTexture(string)"/>
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
    /// ���� unity_builtin_extra �����Դ
    /// <para>unity default resources �����Դֱ����<see cref="Resources.GetBuiltinResource{T}(string)"/></para>
    /// unity editor resources û���ҵ���ӦAPI������·�����ԣ�û�м��سɹ���
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
}













