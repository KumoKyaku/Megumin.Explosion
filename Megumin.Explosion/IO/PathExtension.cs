using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO
{
    public static class PathExtension_0728441477D842BE8399395F75CA8BB9
    {
        public static string CreateFileName(this DirectoryInfo directory, string fileName, string ex)
        {
            return directory.FullName.CreateFileName(fileName, ex);
        }

        public static string AddOne(this FileInfo file)
        {
            return file.Name.FileNameAddOne();
        }

        public static string AutoFileName(this FileInfo file, DirectoryInfo directory)
        {
            return file.Name.AutoFileName(directory.FullName, file.Extension);
        }
    }
}


