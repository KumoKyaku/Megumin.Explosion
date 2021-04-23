using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[HelpURL("https://answers.unity.com/questions/1377941/getassetpath-returning-incomplete-path-for-default.html?_ga=2.137606966.727796312.1613282722-322683566.1604029446")]
public class BuiltInAssetsExtra : MonoBehaviour
{
    public UnityEngine.Object[] unity_builtin_extra;
    public UnityEngine.Object[] unity_default_resources;
    
    public string[] names;

    [ContextMenu("GetAllExtraAssets")]
    void GetAllExtraAssets()
    {
#if UNITY_EDITOR
        unity_builtin_extra = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra");
        unity_default_resources = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Resources/unitydefaultresources");
        unity_editor_resources = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Resources/unity_editor_resources");
        names = UnityEditor.AssetDatabase.FindAssets(".fbx");
#endif
    }


    public UnityEngine.Object[] unity_editor_resources;
    public string[] editorAssetNames;

    [ContextMenu("GetEditorAsset")]
    void GetEditorAsset()
    {
#if UNITY_EDITOR
        
        var editorGUIUtility = typeof(UnityEditor.EditorGUIUtility);
        var getEditorAssetBundle = editorGUIUtility.GetMethod(
            "GetEditorAssetBundle",
            BindingFlags.NonPublic | BindingFlags.Static);

        var ab = (AssetBundle)getEditorAssetBundle.Invoke(null, null);

        unity_editor_resources = ab.LoadAllAssets();
        editorAssetNames = ab.GetAllAssetNames();
#endif
    }
}
