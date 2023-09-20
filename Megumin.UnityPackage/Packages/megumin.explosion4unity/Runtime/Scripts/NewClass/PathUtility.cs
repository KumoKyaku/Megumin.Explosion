#if !MEGUMIN_Common

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Megumin
{
    public static class PathUtility
    {
        public static string ProjectPath { get; } = GetProjectFolderPath("");
        public static string PackagesPath { get; } = GetProjectFolderPath("Packages");
        public static string LibraryPath { get; } = GetProjectFolderPath("Library");
        public static string LibraryPackageCachePath { get; } = GetProjectFolderPath("Library/PackageCache");
        public static string LogsPath { get; } = GetProjectFolderPath("Logs");
        public static string TempPath { get; } = GetProjectFolderPath("Temp");
        public static string UserSettingsPath { get; } = GetProjectFolderPath("UserSettings");

        //BuildPath
        public static string BuildPath { get; } = GetProjectFolderPath("Build");

        public static string BuildPath_StandaloneOSX { get; } = GetProjectFolderPath("Build/StandaloneOSX");
        public static string BuildPath_iOS { get; } = GetProjectFolderPath("Build/iOS");

        public static string BuildPath_Android_Mono { get; } = GetProjectFolderPath("Build/Android_Mono");
        public static string BuildPath_Android_IL2CPP { get; } = GetProjectFolderPath("Build/Android_IL2CPP");

        public static string BuildPath_StandaloneWindows64_Mono { get; } = GetProjectFolderPath("Build/StandaloneWindows64_Mono");
        public static string BuildPath_StandaloneWindows64_IL2CPP { get; } = GetProjectFolderPath("Build/StandaloneWindows64_IL2CPP");

        public static string BuildPath_StandaloneLinux64 { get; } = GetProjectFolderPath("Build/StandaloneLinux64");

        public static string BuildPath_PS4 { get; } = GetProjectFolderPath("Build/PS4");
        public static string BuildPath_PS5 { get; } = GetProjectFolderPath("Build/PS5");

        public static string BuildPath_WebGL { get; } = GetProjectFolderPath("Build/WebGL");

        public static string BuildPath_DedicatedServer { get; } = GetProjectFolderPath("Build/DedicatedServer");

        public static string BuildPath_activeBuildTarget { get; } = GetProjectFolderPath("Build/activeBuildTarget");

        public static string ProjectSettingsPath { get; } = GetProjectFolderPath("ProjectSettings");
        public static string ConsoleLogPath { get; } = Path.GetDirectoryName(Application.consoleLogPath);

        public static string GetProjectFolderPath(string folder)
        {
            var path = Path.Combine(Application.dataPath, $"..\\{folder}");
            return Path.GetFullPath(path);
        }

        /// <summary>
        /// 转换为项目相对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [Obsolete("use GetRelativePathWithProject")]
        public static string MakeUnityProjectRelativePath(this string path)
        {
            var p1 = new System.Uri(path);
            var p2 = new System.Uri(ProjectPath);
            var r = p2.MakeRelativeUri(p1);
            return r.ToString();
        }

        /// <summary>
        /// 转换为项目相对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRelativePathWithProject(this string path)
        {
            var p1 = new System.Uri(path);
            var p2 = new System.Uri(ProjectPath);
            var r = p2.MakeRelativeUri(p1);
            return r.ToString();
        }

        /// <summary>
        /// 转换为项目绝对路径
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string GetFullPathWithProject(this string path)
        {
            return Path.GetFullPath(Path.Combine(ProjectPath, path));
        }
    }

}

#endif


