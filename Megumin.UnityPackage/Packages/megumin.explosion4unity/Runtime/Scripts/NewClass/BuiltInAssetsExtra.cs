using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://answers.unity.com/questions/1377941/getassetpath-returning-incomplete-path-for-default.html?_ga=2.137606966.727796312.1613282722-322683566.1604029446")]
public class BuiltInAssetsExtra : MonoBehaviour
{
    public UnityEngine.Object[] objs;
    [ContextMenu("GetAllExtraAssets")]
    void GetAllExtraAssets()
    {
#if UNITY_EDITOR
        objs = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra");
#endif
    }
}
