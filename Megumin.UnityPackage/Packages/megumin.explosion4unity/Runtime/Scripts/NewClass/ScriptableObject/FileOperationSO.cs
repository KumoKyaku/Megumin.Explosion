using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Megumin
{
    public class FileOperationSO : ScriptableObject
    {
        [Serializable]
        public class CopyTask
        {
            [Path]
            public string OutputFolder;
            [Space(20)]
            [FormerlySerializedAs("Files")]
            public List<TextAsset> TextAsset = new List<TextAsset>();
            public List<UnityEngine.Object> ObjectFiles = new List<UnityEngine.Object>();
        }

        public List<CopyTask> CopyTasks = new List<CopyTask>();

        [Tooltip("拷贝文件到目标目录")]
        [HelpBox("执行操作前请使用版本管理！！！防止文件和改动丢失！！！", HelpBoxMessageType.Error)]
        [Editor]
        public void Copy()
        {
            foreach (var task in CopyTasks)
            {
                foreach (var item in task.TextAsset)
                {
                    CopyFile(task.OutputFolder, item);
                }

                foreach (var item in task.ObjectFiles)
                {
                    CopyFile(task.OutputFolder, item);
                }
            }

        }

        private static void CopyFile(string outputFolder, UnityEngine.Object file)
        {
            try
            {
                var path = file.GetAbsoluteFilePath();
                path = Path.GetFullPath(path);
                var des = Path.Combine(MeguminUtility4Unity.ProjectPath, outputFolder);
                var fileName = Path.GetFileName(path);
                des = Path.Combine(des, fileName);
                des = Path.GetFullPath(des);
                if (path == des)
                {
                    Debug.LogWarning($"Ignore [{fileName}]  from {path} to {des}");
                }
                else
                {
                    Debug.Log($"Cpoy [{fileName}]  from {path} to {des}");
                    File.Copy(path, des, true);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
