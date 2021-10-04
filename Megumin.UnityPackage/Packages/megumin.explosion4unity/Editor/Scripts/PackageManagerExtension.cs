using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager.UI;
using UnityEngine.UIElements;
using System.IO;

namespace Megumin
{
    class PackageManagerExtension
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            PackageManagerExtensions.RegisterExtension(new Ex());
        }

        class Ex : IPackageManagerExtension
        {
            public VisualElement CreateExtensionUI()
            {
                VisualElement visual = new VisualElement();
                visual.style.flexDirection = FlexDirection.Row;
                visual.style.flexWrap = Wrap.Wrap;

                const int width = 160;

                openFolder = new Button();
                openFolder.text = "Open Cache Folder";
                openFolder.clicked += Button_onClick;
                openFolder.style.width = width;
                visual.Add(openFolder);

                opengit = new Button();
                opengit.text = "Open Git Link";
                opengit.clicked += Opengit_clicked;
                opengit.style.width = width;
                visual.Add(opengit);

                move2Local = new Button();
                move2Local.text = "Move Cache To Local";
                move2Local.clicked += Move2Local_clicked;
                move2Local.style.width = width;
                visual.Add(move2Local);

                move2Cache = new Button();
                move2Cache.text = "Move Local To Cache";
                move2Cache.clicked += Move2Cache_clicked;
                move2Cache.style.width = width;
                visual.Add(move2Cache);

                return visual;
            }

            private void Move2Cache_clicked()
            {
                if (current == null)
                {

                }
                else
                {
                    DirectoryInfo info = new DirectoryInfo(current.resolvedPath);
                    var foldername = info.Name;
                    var desPath = Path.Combine(MeguminUtility4Unity.LibraryPackageCachePath, foldername);
                    Debug.Log(desPath);

                    if (Directory.Exists(desPath))
                    {
                        Directory.Delete(desPath);
                    }

                    Directory.Move(current.resolvedPath, desPath);
                    System.Diagnostics.Process.Start(desPath);
                }
            }

            private void Move2Local_clicked()
            {
                if (current == null)
                {

                }
                else
                {
                    DirectoryInfo info = new DirectoryInfo(current.resolvedPath);
                    var foldername = info.Name;
                    var desPath = Path.Combine(MeguminUtility4Unity.PackagesPath, foldername);
                    Debug.Log(desPath);

                    if (Directory.Exists(desPath))
                    {
                        Directory.Delete(desPath);
                    }

                    Directory.Move(current.resolvedPath, desPath);
                    System.Diagnostics.Process.Start(desPath);
                }
            }

            private void Opengit_clicked()
            {
                if (current?.source == UnityEditor.PackageManager.PackageSource.Git)
                {
                    var url = current.GetType().GetField("m_ProjectDependenciesEntry",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        .GetValue(current) as string;
                    Debug.Log($"打开链接：{url}");
                    Application.OpenURL(url);
                }
            }

            private void Button_onClick()
            {
                Debug.Log(current);
                if (current == null)
                {
                    DirectoryUtility.OpenAssetStoreCacheFolder();
                }
                else
                {
                    System.Diagnostics.Process.Start(current.resolvedPath);
                }
            }

            public UnityEditor.PackageManager.PackageInfo current = null;
            private Button openFolder;
            private Button opengit;
            private Button move2Local;
            private Button move2Cache;

            public void OnPackageSelectionChange(UnityEditor.PackageManager.PackageInfo packageInfo)
            {
                //packageInfo 永远是null，应该是个Bug。
                current = packageInfo;
                //button.SetEnabled(false);
                opengit.SetEnabled(current?.source == UnityEditor.PackageManager.PackageSource.Git);
                move2Local.SetEnabled(!current?.resolvedPath.StartsWith(MeguminUtility4Unity.PackagesPath) ?? false);
                move2Cache.SetEnabled(!current?.resolvedPath.StartsWith(MeguminUtility4Unity.LibraryPackageCachePath) ?? false);
            }

            public void OnPackageAddedOrUpdated(UnityEditor.PackageManager.PackageInfo packageInfo)
            {
                //MeguminUtility4Unity.LogNotImplemented();
            }

            public void OnPackageRemoved(UnityEditor.PackageManager.PackageInfo packageInfo)
            {
                //MeguminUtility4Unity.LogNotImplemented();
            }
        }
    }
}







