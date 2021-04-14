using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Megumin
{
    internal static class DirectoryUtility
    {
        [MenuItem("Tools/Path/Create Build Folder")]
        static void CreateBuildFolder()
        {
            var dir = MeguminUtility4Unity.BuildPath;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            dir = MeguminUtility4Unity.BuildIL2CPPPath;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            System.Diagnostics.Process.Start(MeguminUtility4Unity.ProjectPath);
        }

        [MenuItem("Tools/Path/Open ConsoleLog Folder")]
        static void OpenLogFolder()
        {
            var dir = MeguminUtility4Unity.ConsoleLogPath;
            Debug.Log($"打开 {dir}");
            System.Diagnostics.Process.Start(dir);
        }
    }
}
