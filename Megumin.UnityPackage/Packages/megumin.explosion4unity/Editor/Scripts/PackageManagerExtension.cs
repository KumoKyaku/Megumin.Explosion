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
                button = new Button();
                button.text = "Open Cache Folder";
                button.clicked += Button_onClick;
                return button;
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
            private Button button;

            public void OnPackageSelectionChange(UnityEditor.PackageManager.PackageInfo packageInfo)
            {
                //packageInfo 永远是null，应该是个Bug。
                current = packageInfo;
                //button.SetEnabled(false);
            }

            public void OnPackageAddedOrUpdated(UnityEditor.PackageManager.PackageInfo packageInfo)
            {
                MeguminUtility4Unity.LogNotImplemented();
            }

            public void OnPackageRemoved(UnityEditor.PackageManager.PackageInfo packageInfo)
            {
                MeguminUtility4Unity.LogNotImplemented();
            }
        }
    }
}







