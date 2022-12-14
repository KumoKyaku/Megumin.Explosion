using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace Megumin
{
    public class CopyFileSO : ScriptableObject
    {

        [Path]
        public string DesDir;
        public List<TextAsset> CSFile = new List<TextAsset>();

        [Tooltip("拷贝文件到目标目录")]
        [HelpBox("执行操作前请使用版本管理！！！", HelpBoxMessageType.Warning)]
        [Editor]
        public void Copy()
        {
            foreach (var item in CSFile)
            {
                var path = item.GetAbsoluteFilePath();
                path = Path.GetFullPath(path);
                var des = Path.Combine(MeguminUtility4Unity.ProjectPath, DesDir);
                var fileName = Path.GetFileName(path);
                des = Path.Combine(des, fileName);
                des = Path.GetFullPath(des);
                if (path != des)
                {
                    Debug.Log($"Cpoy [{fileName}]  from {path} to {des}");
                    File.Copy(path, des, true);
                }
            }
        }
    }
}
