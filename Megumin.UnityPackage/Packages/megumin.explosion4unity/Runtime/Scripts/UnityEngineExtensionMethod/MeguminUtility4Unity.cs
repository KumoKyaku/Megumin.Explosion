using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    public class MeguminUtility4Unity
    {
        static string packagePath = "";
        public static string PackagesPath
        {
            get
            {
                if (string.IsNullOrEmpty(packagePath))
                {
                    packagePath = GetProjectFolderPath("Packages");
                }
                return packagePath;
            }
        }

        public static string LibraryPath { get; } = GetProjectFolderPath("Library");
        public static string LogsPath { get; } = GetProjectFolderPath("Logs");
        public static string TempPath { get; } = GetProjectFolderPath("Temp");
        public static string UserSettingsPath { get; } = GetProjectFolderPath("UserSettings");
        public static string BuildPath { get; } = GetProjectFolderPath("Build");
        public static string ProjectSettings { get; } = GetProjectFolderPath("ProjectSettings");

        public static string GetProjectFolderPath(string folder)
        {
            var path = Path.Combine(Application.dataPath, $"..\\{folder}");
            return Path.GetFullPath(path);
        }
    }
}



