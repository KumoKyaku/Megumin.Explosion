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
                    packagePath = Path.Combine(Application.dataPath, "..\\Packages");
                    packagePath = Path.GetFullPath(packagePath);
                }
                return packagePath;
            }
        }
    }
}
