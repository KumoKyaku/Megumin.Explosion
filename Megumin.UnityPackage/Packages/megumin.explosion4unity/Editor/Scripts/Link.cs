using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Megumin
{
    internal class Link
    {
        static void OpenURL(string url)
        {
            Debug.Log($"打开链接：{url}");
            Application.OpenURL(url);
        }


        [MenuItem("Tools/Link/Unity/ImmediateWindow")]
        static void ImmediateWindow()
        {
            OpenURL("https://github.com/Unity-Technologies/com.unity.immediate-window");
        }


        [MenuItem("Tools/Link/Unity-UI-Extensions")]
        static void Unity_UI_Extensions()
        {
            OpenURL("https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/src/release/");
        }


        [MenuItem("Tools/Link/IconList")]
        static void IconList()
        {
            OpenURL("https://github.com/halak/unity-editor-icons/blob/master/README.md");
        }

        [MenuItem("Tools/Link/群友分享链接")]
        static void LinkList()
        {
            OpenURL("https://docs.qq.com/sheet/DUnhtZVRBR05xTXVI?tab=BB08J2");
        }

        [MenuItem("Tools/Link/UniRx")]
        static void UniRx()
        {
            OpenURL("https://github.com/neuecc/UniRx#upm-package");
        }

        [MenuItem("Tools/Link/UniTask")]
        static void UniTask()
        {
            OpenURL("https://github.com/Cysharp/UniTask#upm-package");
        }

        [MenuItem("Tools/Link/LINQ-to-GameObject-for-Unity")]
        static void LINQtoGameObjectforUnity()
        {
            OpenURL("https://github.com/neuecc/LINQ-to-GameObject-for-Unity/#linq-to-gameobject");
        }

        [MenuItem("Tools/Link/Newtonsoft.Json for Unity")]
        static void NewtonsoftJson()
        {
            OpenURL("https://github.com/jilleJr/Newtonsoft.Json-for-Unity#installation-via-git-in-upm");
        }


        [MenuItem("Tools/Link/ClassTypeReference for Unity")]
        static void ClassTypeReference()
        {
            OpenURL("https://github.com/SolidAlloy/ClassTypeReference-for-Unity");
        }

        [MenuItem("Tools/Link/ShaderGraphNode/3D Nosie")]
        static void JimmyCushnie_Noisy_Nodes()
        {
            OpenURL("https://github.com/JimmyCushnie/Noisy-Nodes");
        }
    }
}
