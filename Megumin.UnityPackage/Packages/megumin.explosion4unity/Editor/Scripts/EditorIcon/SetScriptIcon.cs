using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
//https://gist.github.com/MattRix/c1f7840ae2419d8eb2ec0695448d4321

namespace Megumin
{
    [HelpURL("https://docs.unity3d.com/2022.1/Documentation/ScriptReference/MonoImporter.html")]
    public class SetScriptIcon
    {
        /// <summary>
        /// https://docs.unity3d.com/2022.1/Documentation/ScriptReference/EditorGUIUtility.SetIconForObject.html
        /// </summary>
        [MenuItem("Assets/Set Script Icon", false, 20)]
        public static async void SetIcon()
        {
            //var gc = EditorGUIUtility.IconContent("d_Collab.FileMoved");
            //Texture2D icon = gc.image as Texture2D;
            var objectTarget = Selection.activeObject;

            if (objectTarget && Selection.assetGUIDs.Length > 0)
            {
                var path = AssetDatabase.GetAssetPath(objectTarget);
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objectTarget, out var guid, out long _);
                if (Selection.assetGUIDs[0] != guid)
                {
                    return;
                }

                var imp = AssetImporter.GetAtPath(path);

                if (imp is MonoImporter mono)
                {
                    var icon = await EditorIcons.Select();
                    Undo.RecordObjects(new Object[] { objectTarget }, "Set MonoImporter Icon");

                    mono.SetIcon(icon);
                    mono.SaveAndReimport();
                    EditorApplication.delayCall += () =>
                    {
                        Debug.Log($"{nameof(SetScriptIcon)} Set Script Icon.");
                    };
                }
                else if (imp is PluginImporter plugin)
                {
                    var icon = await EditorIcons.Select();
                    Undo.RecordObjects(new Object[] { objectTarget }, "Set PluginImporter Icon");

                    plugin.SetIcon(System.IO.Path.GetFileName(path), icon);
                    plugin.SaveAndReimport();
                    EditorApplication.delayCall += () =>
                    {
                        Debug.Log($"{nameof(SetScriptIcon)} Set Script Icon.");
                    };
                }
            }
        }

        [MenuItem("Assets/Set Script Icon", true, 20)]
        public static bool SetIconValid()
        {
            var objectTarget = Selection.activeObject;

            if (objectTarget && Selection.assetGUIDs.Length > 0)
            {
                var path = AssetDatabase.GetAssetPath(objectTarget);
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objectTarget, out var guid, out long _);

                if (Selection.assetGUIDs[0] == guid)
                {
                    return true;
                }
            }

            return false;
        }

        [MenuItem("Tools/Style/Export Editor Icons", priority = -1002)]
        public static void ExportIconList()
        {
            EditorUtility.DisplayProgressBar("Export", "Find TotalIcon", 0.1f);
            var list = MeguminEditorUtility.GetEditorIcon();
            var set = new HashSet<string>();
            foreach (var item in list)
            {
                set.Add(item.Name);
            }
            CSCodeGenerator generator = new CSCodeGenerator();
            generator.Push(@"public static class EditorIconListByCodeGenerate");
            using (generator.NewScope)
            {
                generator.PushSummaryNote(@$"AutoGeneric. TotalCount : {set.Count} ");
                generator.Push(@"public static string[] IconList =");
                generator.BeginScope();
                int count = 0;
                foreach (var item in set)
                {
                    var name = System.IO.Path.GetFileNameWithoutExtension(item);
                    generator.Push(@$"""{name}"",");
                    count++;
                }
                generator.EndScopeWithSemicolon();
            }
            generator.Push("");
            generator.Push("");

            //var path = @"W:\Git\Megumin.Explosion\Megumin.UnityPackage\Packages\megumin.explosion4unity\Editor\Scripts\EditorIcon\EditorIconList.cs";
            var path = Path.Combine(MeguminUtility4Unity.PackagesPath,
                @"megumin.explosion4unity\Editor\Scripts\EditorIcon\EditorIconList.cs");
            path = Path.GetFullPath(path);
            EditorUtility.DisplayProgressBar("Export", "WirteCodeToPath", 0.75f);
            generator.Generate(path);
            EditorUtility.ClearProgressBar();
        }
    }

}

