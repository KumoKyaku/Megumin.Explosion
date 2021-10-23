using System.IO;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

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
                VisualElement ExtentionRoot = new VisualElement();
                VisualElement label = new VisualElement();
                ExtentionRoot.Add(label);
                detail = new Label();
                detail.text = "test";
                label.Add(detail);


                VisualElement buttons = new VisualElement();
                ExtentionRoot.Add(buttons);

                buttons.style.flexDirection = FlexDirection.Row;
                buttons.style.flexWrap = Wrap.Wrap;

                const int width = 160;

                openFolder = new Button();
                openFolder.text = "Open Cache Folder";
                openFolder.clicked += Button_onClick;
                openFolder.style.width = width;
                buttons.Add(openFolder);

                opengit = new Button();
                opengit.text = "Open Git Link";
                opengit.clicked += Opengit_clicked;
                opengit.style.width = width;
                buttons.Add(opengit);

                move2Local = new Button();
                move2Local.text = "Move To Packages Folder";
                move2Local.clicked += Move2PackagesFolder_clicked;
                move2Local.style.width = width;
                buttons.Add(move2Local);

                move2Cache = new Button();
                move2Cache.text = "Move To Library Folder";
                move2Cache.clicked += Move2LibraryFolder_clicked;
                move2Cache.style.width = width;
                buttons.Add(move2Cache);

                return ExtentionRoot;
            }

            private void Move2LibraryFolder_clicked()
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
                    MeguminEditorUtility.SyncSolution();
                }
            }

            private void Move2PackagesFolder_clicked()
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
                    MeguminEditorUtility.SyncSolution();
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
            private Label detail;

            public void OnPackageSelectionChange(UnityEditor.PackageManager.PackageInfo packageInfo)
            {
                //packageInfo 永远是null，应该是个Bug。
                current = packageInfo;
                //button.SetEnabled(false);
                bool isGit = current?.source == UnityEditor.PackageManager.PackageSource.Git;
                bool isInLocalPackage = !current?.resolvedPath.StartsWith(MeguminUtility4Unity.PackagesPath) ?? false;
                bool isInLiraryCache = !current?.resolvedPath.StartsWith(MeguminUtility4Unity.LibraryPackageCachePath) ?? false;

                detail.text = $"[isGit : {isGit}]    [isInLocalPackage : {isInLocalPackage}]    [isInLiraryCache : {isInLiraryCache}]";

                if (current != null)
                {
                    Debug.Log(current.displayName + "    " + MeguminUtility.Detail(current));
                }

                opengit.SetEnabled(isGit);
                move2Local.SetEnabled(isInLocalPackage);
                move2Cache.SetEnabled(isInLiraryCache);
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







