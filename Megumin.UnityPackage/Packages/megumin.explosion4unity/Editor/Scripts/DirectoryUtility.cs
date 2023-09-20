using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static System.Environment;

namespace Megumin
{
    internal static class DirectoryUtility
    {
        [MenuItem("Tools/Path/Log Selection Folders")]
        public static void LogSelectionFolders()
        {
            var fs = GetSelectionFolders();
            var dirs = "";
            foreach (var item in fs)
            {
                dirs += $"{item}  |  ";
            }
            Debug.Log($"{dirs}");
        }

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
            List<string> list = new List<string>()
            {
                PathUtility.BuildPath_StandaloneOSX,
                PathUtility.BuildPath_iOS,
                PathUtility.BuildPath_Android_Mono,
                PathUtility.BuildPath_Android_IL2CPP,
                PathUtility.BuildPath_StandaloneWindows64_Mono,
                PathUtility.BuildPath_StandaloneWindows64_IL2CPP,
                PathUtility.BuildPath_StandaloneLinux64,
                PathUtility.BuildPath_PS4,
                PathUtility.BuildPath_PS5,
                PathUtility.BuildPath_WebGL,
                PathUtility.BuildPath_DedicatedServer,
                PathUtility.BuildPath_activeBuildTarget,
            };

            foreach (var dir in list) 
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }

            Debug.Log($"创建打包目录");
            System.Diagnostics.Process.Start(PathUtility.BuildPath);
        }

        [MenuItem("Tools/Path/Open ConsoleLog Folder")]
        static void OpenLogFolder()
        {
            var dir = PathUtility.ConsoleLogPath;
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

        public static List<string> GetSelectionFolders()
        {
            List<string> list = new List<string>();
            Object[] items = Selection.GetFiltered<Object>(SelectionMode.Assets);
            foreach (var item in items)
            {
                var path = AssetDatabase.GetAssetPath(item);
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }

                if (System.IO.Directory.Exists(path))
                {
                    list.Add(path);
                }
            }
            return list;
        }
    }
}
