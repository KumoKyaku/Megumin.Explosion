﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Megumin
{
    internal partial class Link
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

        [MenuItem("Tools/Link/Unity/Timeline Playables Wizard")]
        static void TimelinePlayablesWizard()
        {
            OpenURL("https://assetstore.unity.com/packages/essentials/default-playables-95266");
        }

        [MenuItem(@"Tools/Link/Unity/NavMesh Components | com.unity.ai.navigation")]
        static void NavMeshComponents()
        {
            OpenURL("https://github.com/Unity-Technologies/NavMeshComponents/tree/package");
            OpenURL("https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.0/manual/index.html");
        }

        [MenuItem("Tools/Link/Unity/Japan/UnityChanSpringBone")]
        static void UnityChanSpringBone()
        {
            OpenURL("https://github.com/unity3d-jp/UnityChanSpringBone/tree/release/1.2");
        }

        //[MenuItem("Tools/Link/Unity/Japan/UnityChanSpringBone/Add")]
        //static void UnityChanSpringBoneAdd()
        //{

        //}

        [MenuItem("Tools/Link/Unity/Japan/Universal Toon Shader for URP")]
        static void UnityChanToonShaderVer2_Project()
        {
            OpenURL("https://github.com/unity3d-jp/UnityChanToonShaderVer2_Project/tree/release/urp%2F2.2.3");
        }

        [MenuItem("Tools/Link/Unity-UI-Extensions")]
        static void Unity_UI_Extensions()
        {
            OpenURL("https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/src/release/");
        }

        [MenuItem("Tools/Link/Unity-UI-UIEffect")]
        static void Unity_UI_UIEffect()
        {
            OpenURL("https://github.com/mob-sakai/UIEffect");
        }

        [MenuItem("Tools/Link/Unity-QianMo-X-PostProcessing-Library")]
        static void Unity_QianMo_X_PostProcessing_Library()
        {
            OpenURL("https://github.com/QianMo/X-PostProcessing-Library");
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

        [MenuItem("Tools/Link/In-game Debug Console")]
        static void In_game_Debug_Console()
        {
            OpenURL("https://github.com/yasirkula/UnityIngameDebugConsole#installation");
        }

        [MenuItem("Tools/Link/Runtime Inspector & Hierarchy for Unity 3D")]
        static void RuntimeInspectorHierarchyforUnity3D()
        {
            OpenURL("https://github.com/yasirkula/UnityRuntimeInspector#c-installation");
        }

        [MenuItem("Tools/Link/Runtime Scene Gizmo for Unity")]
        static void RuntimeSceneGizmoforUnity()
        {
            OpenURL("https://github.com/0x3f3f3f3f/UnityRuntimeSceneGizmo");
        }

        [MenuItem("Tools/Link/NaughtyAttributes")]
        static void NaughtyAttributes()
        {
            OpenURL("https://github.com/dbrizov/NaughtyAttributes#installation");
        }

        [MenuItem("Tools/Link/GUI.Scope")]
        static void GUIScope()
        {
            OpenURL("https://hacchi-man.hatenablog.com/entry/2019/12/20/002444");
        }

        [MenuItem("Tools/Link/SubAssetEditor")]
        static void SubAssetEditor()
        {
            OpenURL("https://github.com/mob-sakai/SubAssetEditor");
        }

        [MenuItem("Tools/Link/Mixture")]
        static void Mixture()
        {
            OpenURL("https://github.com/alelievr/Mixture#installation");
        }

        [MenuItem("Tools/Link/[Graphy] - Ultimate FPS Counter - Stats Monitor & Debugger")]
        static void Graphy()
        {
            OpenURL("https://github.com/Tayx94/graphy#installation");
        }
    }

    partial class Link
    {
        //dotnet source link

        [MenuItem("Tools/Link/Dotnet Source/List")]
        static void DotnetSourceList()
        {
            OpenURL("https://source.dot.net/#System.Private.CoreLib/List.cs,cf7f4095e4de7646");
        }

        [MenuItem("Tools/Link/Dotnet Source/Dictionary")]
        static void DotnetSourceDictionary()
        {
            OpenURL("https://source.dot.net/#System.Private.CoreLib/Dictionary.cs,d3599058f8d79be0");
        }


        [MenuItem("Tools/Link/Dotnet Source/Socket")]
        static void DotnetSourceSocket()
        {
            OpenURL("https://source.dot.net/#q=Socket");
        }

        [MenuItem("Tools/Link/Dotnet Source/Task")]
        static void DotnetSourceTask()
        {
            OpenURL("https://source.dot.net/#System.Private.CoreLib/Task.cs,045a746eb48cbaa9");
        }
    }
}





