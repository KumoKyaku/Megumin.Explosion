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

        [MenuItem("Tools/LogInfo/ActiveCustomEditorType")]
        static void LogActiveCustomEditorType()
        {
            MeguminExtension_2B5D73B2.FindCustomEditorTypeByType(typeof(Object), false);
            MeguminExtension_2B5D73B2.FindCustomEditorTypeByType(typeof(Component), false);
            MeguminExtension_2B5D73B2.FindCustomEditorTypeByType(typeof(Behaviour), false);
            MeguminExtension_2B5D73B2.FindCustomEditorTypeByType(typeof(MonoBehaviour), false);
            MeguminExtension_2B5D73B2.FindCustomEditorTypeByType(typeof(ScriptableObject), false);
        }
    }
}



