using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    public class MeguminUtility4Unity
    {
        public static string ProjectPath { get; } = GetProjectFolderPath("");
        public static string PackagesPath { get; } = GetProjectFolderPath("Packages");
        public static string LibraryPath { get; } = GetProjectFolderPath("Library");
        public static string LogsPath { get; } = GetProjectFolderPath("Logs");
        public static string TempPath { get; } = GetProjectFolderPath("Temp");
        public static string UserSettingsPath { get; } = GetProjectFolderPath("UserSettings");
        
        public static string BuildPCMonoPath { get; } = GetProjectFolderPath("Build/PCMomo");
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

        //=====================================

        /// <summary>
        /// 打印为实现代替抛出异常
        /// </summary>
        /// <param name="funcName"></param>
        public static void LogNotImplemented([CallerMemberName] string funcName = null)
        {
            UnityEngine.Debug.LogError($"{funcName} NotImplemented.    [Instead throw Exception]");
        }
    }
}



