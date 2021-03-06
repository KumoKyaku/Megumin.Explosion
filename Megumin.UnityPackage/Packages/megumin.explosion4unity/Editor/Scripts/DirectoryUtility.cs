﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static System.Environment;

namespace Megumin
{
    internal static class DirectoryUtility
    {
        [MenuItem("Tools/Path/Log Environment SpecialFolder")]
        static void LogEnvironmentSpecialFolder()
        {
            foreach (SpecialFolder item in System.Enum.GetValues(typeof(SpecialFolder)))
            {
                Debug.Log($"{item.ToString().Html(HexColor.Azure)}:    {GetFolderPath(item)}");
            }
        }

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

        [MenuItem("Tools/Path/Open AssetStoreCache Folder")]
        public static void OpenAssetStoreCacheFolder()
        {
            var dir = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData),
                                   "Unity/Asset Store-5.x");
            dir = Path.GetFullPath(dir);
            Debug.Log($"打开 {dir}");
            System.Diagnostics.Process.Start(dir);
        }


    }
}
