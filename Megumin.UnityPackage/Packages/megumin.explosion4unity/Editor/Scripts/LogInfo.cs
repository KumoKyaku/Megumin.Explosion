using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Megumin
{
    internal static class LogInfo
    {
        [MenuItem("Tools/LogInfo/Application")]
        static void LogApplicationDetail()
        {
            Debug.Log(Utility.ToStringReflection<Application>());
        }

        [MenuItem("Tools/LogInfo/EditorApplication")]
        static void LogEditorApplicationDetail()
        {
            Debug.Log(Utility.ToStringReflection<EditorApplication>());
        }

        [MenuItem("Tools/LogInfo/System.Environment")]
        static void LogSystemEnvironment()
        {
            Debug.Log(Utility.ToStringReflection(typeof(System.Environment)));
        }

        [MenuItem("Tools/LogInfo/SystemInfo")]
        static void LogSystemInfo()
        {
            Debug.Log(Utility.ToStringReflection<SystemInfo>());
        }
    }
}



