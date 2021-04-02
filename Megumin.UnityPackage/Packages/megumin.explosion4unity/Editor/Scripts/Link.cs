using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Link
{
    [MenuItem("Tools/Link/Unity/ImmediateWindow")]
    static void ImmediateWindow()
    {
        Application.OpenURL("https://github.com/Unity-Technologies/com.unity.immediate-window");
    }


    [MenuItem("Tools/Link/Unity-UI-Extensions")]
    static void Unity_UI_Extensions()
    {
        Application.OpenURL("https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/src/release/");
    }


    [MenuItem("Tools/Link/IconList")]
    static void IconList()
    {
        Application.OpenURL("https://github.com/halak/unity-editor-icons/blob/master/README.md");
    }

    [MenuItem("Tools/Link/»∫”—∑÷œÌ¡¥Ω”")]
    static void LinkList()
    {
        Application.OpenURL("https://docs.qq.com/sheet/DUnhtZVRBR05xTXVI?tab=BB08J2");
    }

    [MenuItem("Tools/Link/UniRx")]
    static void UniRx()
    {
        Application.OpenURL("https://github.com/neuecc/UniRx#upm-package");
    }

    [MenuItem("Tools/Link/UniTask")]
    static void UniTask()
    {
        Application.OpenURL("https://github.com/Cysharp/UniTask#upm-package");
    }

    [MenuItem("Tools/Link/LINQ-to-GameObject-for-Unity")]
    static void LINQtoGameObjectforUnity()
    {
        Application.OpenURL("https://github.com/neuecc/LINQ-to-GameObject-for-Unity/#linq-to-gameobject");
    }

    [MenuItem("Tools/Link/Newtonsoft.Json for Unity")]
    static void NewtonsoftJson()
    {
        Application.OpenURL("https://github.com/jilleJr/Newtonsoft.Json-for-Unity#installation-via-git-in-upm");
    }


    [MenuItem("Tools/Link/ClassTypeReference for Unity")]
    static void ClassTypeReference()
    {
        Application.OpenURL("https://github.com/SolidAlloy/ClassTypeReference-for-Unity");
    }
}



