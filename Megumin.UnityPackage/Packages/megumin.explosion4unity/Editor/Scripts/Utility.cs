using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public static Type FindCustomEditorTypeByType(Type type, bool multiEdit, bool log = true)
    {
        #region 注释

        /*
        Q:
        [CustomEditor(typeof(GameObject))]
        public class A1 : Editor
        {
        }

        [CustomEditor(typeof(GameObject))]
        public class A2 : Editor
        {
        }
        A1,A2哪一个会生效？

        A：

        当项目代码重新编译时，返回反射记录所有[CustomEditor]信息。
        保存于CustomEditorAttributes.kSCustomEditors 和CustomEditorAttributes.kSCustomEditors 两个List中。
        当需要绘制Inspector面板时，查找CustomEditorAttributes.kSCustomEditors 和CustomEditorAttributes.kSCustomEditors 。
        查找方式FirstOrDefault，这意味着加载程序集的顺序将决定调用哪一个Typ会被先记录在List中，也就会生效。

        四种文件目录分别对用4个dll。
        [Asset]->Assembly-CSharp.dll
        [Asset Editor]->Assembly-CSharp-Editor.dll
        [Plugins]->Assembly-CSharp-firstpass.dll
        [Plugins Editor]->Assembly-CSharp-Editor-firstpass.dll

        载入顺序为 1.Assembly-CSharp-firstpass 2.Assembly-CSharp 3.Assembly-CSharp-Editor-firstpass 4.Assembly-CSharp-Editor。

        但是，反射记录所有[CustomEditor]信息时 程序集顺序是【倒着】 读取的，
        这意味着[Asset Editor]的[CustomEditor(typeof(GameObject))]优先其他三种文件夹的脚本生效。

        那么，同为[Asset Editor]中的脚本，也就是Assembly-CSharp-Editor.dll中的哪个类型会优先生效呢？
        取决于类型的Token，而Token取决于编译顺序，编译顺序为，
        1.文件名字相同时，例如0.cs 和 0.cs，所处文件夹名字靠前的先编译。
        2.文件名字不同时，文件名字小的先编译，和文件夹名字无关。

        正常情况下：先编译的优先加载，优先生效。
        例如 Asset/0/Editor/0.cs 中的CustomEditor标记的类型总是最优先生效。

        当然，还有少数例外情况，略。
         */

        #endregion

        var t = typeof(ArrayUtility).Assembly.GetTypes().First(t => t.Name == "CustomEditorAttributes");
        var m = t.GetMethod("FindCustomEditorTypeByType", BindingFlags.Static | BindingFlags.NonPublic);
        var res = m.Invoke(null, new object[] { type, multiEdit });
        var restype = res as Type;
        if (restype != null && log)
        {
            Debug.Log($"{type.FullName} : [CustomEditorType:{restype.FullName}--Assembly:{restype.Assembly.FullName}]");
        }

        return restype;
    }
}










