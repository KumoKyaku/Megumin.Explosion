using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        public static string BuildAndroidIL2CPPPath { get; } = GetProjectFolderPath("Build/AndroidIL2CPP");
        public static string BuildAndroidMonoPath { get; } = GetProjectFolderPath("Build/AndroidMono");
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

        public static List<(T obj, string guid, long localId, string path)>
            CollectAllAsset<T>(List<string> collectFolder = null)
            where T : UnityEngine.Object
        {
            List<(T obj, string guid, long localId, string path)> list = new List<(T obj, string guid, long localId, string path)>();

#if UNITY_EDITOR

            var FindAssetsFolders = new string[] { "Assets", "Packages" };
            if (collectFolder != null)
            {
                FindAssetsFolders = collectFolder.ToArray();
            }

            string[] GUIDs = AssetDatabase.FindAssets($"t:{typeof(T).Name}",
                FindAssetsFolders);

            for (int i = 0; i < GUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(GUIDs[i]);
                var sos = AssetDatabase.LoadAllAssetsAtPath(path);
                foreach (var item in sos)
                {
                    if (item != null && item is T so)
                    {
                        AssetDatabase.TryGetGUIDAndLocalFileIdentifier(so, out var tempGUID, out long localID);
                        list.Add((so, tempGUID, localID, path));
                    }
                }
            }
#endif

            return list;
        }


        /// <summary>
        /// 目前拿到的是第一个方法的行号链接，没找到class声明那一行。
        /// 使用imguidebugger查看Console链接源文本。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetUnityProjectLink(this Type type)
        {
#if MEGUMIN_MONOCECIL
            static (string rpath, int StartLine) GetPath(Mono.Cecil.TypeDefinition t)
            {
                foreach (var method in t.Methods)
                {
                    if (method.HasBody)
                    {
                        var se = method.DebugInformation.GetSequencePointMapping();
                        foreach (var item in se)
                        {
                            var link = item.Value.Document.Url;
                            var rpath = link.MakeUnityProjectRelativePath();
                            return (rpath, item.Value.StartLine);
                        }
                    }
                }
                return default;
            }

            var r = new Mono.Cecil.ReaderParameters() { ReadSymbols = true };
            using (var ad = Mono.Cecil.AssemblyDefinition.ReadAssembly(type.Assembly.Location, r))
            {
                var td = ad.MainModule.GetType(type.FullName);
                var res = GetPath(td);
                return @$"<a href=""{res.rpath}"" line=""{res.StartLine}"">{type.FullName}</a>";
            }
#else
            return $"{type.FullName}    {"[Need MonoCecil]".HtmlColor(HexColor.BrickRed)}";
#endif
        }
    }
}



