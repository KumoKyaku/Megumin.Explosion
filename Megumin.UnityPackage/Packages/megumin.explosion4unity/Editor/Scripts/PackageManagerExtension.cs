using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager.UI;
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
                VisualElement visual = new VisualElement();

                openFolder = new Button();
                openFolder.text = "Open Cache Folder";
                openFolder.clicked += Button_onClick;
                visual.Add(openFolder);

                opengit = new Button();
                opengit.text = "Open Git Link";
                opengit.clicked += Opengit_clicked;
                visual.Add(opengit);

                return visual;
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

            public void OnPackageSelectionChange(UnityEditor.PackageManager.PackageInfo packageInfo)
            {
                //packageInfo 永远是null，应该是个Bug。
                current = packageInfo;
                //button.SetEnabled(false);
                opengit.SetEnabled(current?.source == UnityEditor.PackageManager.PackageSource.Git);
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







