using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    public static class MeguminUtility4Unity
    {
        public static string ProjectPath { get; } = GetProjectFolderPath("");
        public static string PackagesPath { get; } = GetProjectFolderPath("Packages");
        public static string LibraryPath { get; } = GetProjectFolderPath("Library");
        public static string LibraryPackageCachePath { get; } = GetProjectFolderPath("Library/PackageCache");
        public static string LogsPath { get; } = GetProjectFolderPath("Logs");
        public static string TempPath { get; } = GetProjectFolderPath("Temp");
        public static string UserSettingsPath { get; } = GetProjectFolderPath("UserSettings");

        public static string BuildPCMonoPath { get; } = GetProjectFolderPath("Build/PCMono");
        public static string BuildPCIL2CPPPath { get; } = GetProjectFolderPath("Build/PCIL2CPP");
        public static string BuildAndroidPath { get; } = GetProjectFolderPath("Build/Android");
        public static string BuildiOSPath { get; } = GetProjectFolderPath("Build/iOS");
        public static string BuildPS4Path { get; } = GetProjectFolderPath("Build/PS4");
        public static string BuildPS5Path { get; } = GetProjectFolderPath("Build/PS5");


        public static string ProjectSettingsPath { get; } = GetProjectFolderPath("ProjectSettings");
        public static string ConsoleLogPath { get; } = Path.GetDirectoryName(Application.consoleLogPath);

        public static string GetProjectFolderPath(string folder)
        {
            var path = Path.Combine(Application.dataPath, $"..\\{folder}");
            return Path.GetFullPath(path);
        }

        public static string MakeUnityProjectRelativePath(this string path)
        {
            //转换为相对路径。
            var p1 = new System.Uri(path);
            var p2 = new System.Uri(MeguminUtility4Unity.ProjectPath);
            var r = p2.MakeRelativeUri(p1);
            return r.ToString();
        }

        //=====================================

        /// <summary>
        /// 打印为实现代替抛出异常
        /// </summary>
        /// <param name="funcName"></param>
        public static void LogNotImplemented([CallerMemberName] string funcName = null)
        {
            UnityEngine.Debug.LogError($"{funcName} NotImplemented.    [Instead throw Exception]");
        }

        ///// <summary>
        ///// 编译需要 Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo
        ///// AOT模式下运行会报错,但是可以编译
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static bool TryGetName<T>(this T value, out string name)
        //{
        //    name = null;
        //    try
        //    {
        //        dynamic d = value;
        //        name = d.Name;
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    if (string.IsNullOrEmpty(name))
        //    {
        //        try
        //        {
        //            dynamic d = value;
        //            name = d.name;
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //    return false;
        //}

        ///// <inheritdoc cref="TryGetName{T}(T, out string)"/>
        //public static bool TryGetDisplayName<T>(this T value, out string name)
        //{
        //    name = null;
        //    try
        //    {
        //        dynamic d = value;
        //        name = d.DisplayName;
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    if (string.IsNullOrEmpty(name))
        //    {
        //        try
        //        {
        //            dynamic d = value;
        //            name = d.displayName;
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //    return false;
        //}
    }
}



