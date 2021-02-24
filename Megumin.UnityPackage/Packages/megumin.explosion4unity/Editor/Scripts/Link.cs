using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Link
{
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

    [MenuItem("Tools/Link/Unity-UI-Extensions")]
    static void Unity_UI_Extensions()
    {
        Application.OpenURL("https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/src/release/");
    }

    [MenuItem("Tools/Link/UniRx")]
    static void UniRx()
    {
        Application.OpenURL("https://github.com/neuecc/UniRx");
    }

    [MenuItem("Tools/Link/UniTask")]
    static void UniTask()
    {
        Application.OpenURL("https://github.com/Cysharp/UniTask");
    }
}



