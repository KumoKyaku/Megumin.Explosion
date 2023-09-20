using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
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

        //[MenuItem("Tools/Test")]
        static void Test()
        {
            Client.Embed("com.unity.ugui");
            //没找对对应UnEmbed.
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

                VisualElement buttonsLine2 = new VisualElement();
                ExtentionRoot.Add(buttonsLine2);

                buttonsLine2.style.flexDirection = FlexDirection.Row;
                buttonsLine2.style.flexWrap = Wrap.Wrap;

                move2Local = new Button();
                move2Local.text = "Embed";
                move2Local.clicked += Move2PackagesFolder_clicked;
                move2Local.style.width = width;
                buttonsLine2.Add(move2Local);

                move2Cache = new Button();
                move2Cache.text = "UnEmbed";
                move2Cache.clicked += Move2LibraryFolder_clicked;
                move2Cache.style.width = width;
                buttonsLine2.Add(move2Cache);

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
                    var desPath = Path.Combine(PathUtility.LibraryPackageCachePath, foldername);
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
                    var desPath = Path.Combine(PathUtility.PackagesPath, foldername);
                    Debug.Log(desPath);

                    if (Directory.Exists(desPath))
                    {
                        Directory.Delete(desPath);
                    }

                    try
                    {
                        Directory.Move(current.resolvedPath, desPath);
                    }
                    catch (System.IO.IOException e)
                    {
                        Debug.Log($"Close IDE!!!        ".HtmlColor(HexColor.BarnRed) + e.ToString());
                    }

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
                bool? isInLocal = current?.resolvedPath.StartsWith(PathUtility.PackagesPath);
                bool canMove2Local = !isInLocal ?? false;
                bool? isInLibrary = current?.resolvedPath.StartsWith(PathUtility.LibraryPackageCachePath);
                bool canMove2Cache = !isInLibrary ?? false;

                detail.text = $"[Git : {isGit}]    [InLocalPackage : {isInLocal ?? false}]    [InLiraryCache : {isInLibrary ?? false}]";

                if (current != null)
                {
                    Debug.Log(current.displayName + "    " + Utility.ToStringReflection(current));
                }

                opengit.SetEnabled(isGit);
                move2Local.SetEnabled(canMove2Local);
                move2Cache.SetEnabled(canMove2Cache);
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







