using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 代码生成器
    /// </summary>
    public class CSCodeGenerator
    {
        public static string GetIndentStr(int level = 1)
        {
            string res = "";
            for (int i = 0; i < level; i++)
            {
                res += "    ";
            }
            return res;
        }

        public int Indent { get; internal set; } = 0;
        public List<string> Lines { get; set; } = new List<string>();

        public void Push(string code)
        {
            code = GetIndentStr(Indent) + code;
            Lines.Add(code);
        }

        public void Push(CSCodeGenerator generator)
        {
            foreach (var item in generator.Lines)
            {
                Push(item);
            }
        }

        public void BeginScope()
        {
            Push(@$"{{");
            Indent++;
        }

        public void EndScope()
        {
            Indent--;
            Push(@$"}}");
        }

        public void Generate(string path, Encoding encoding = null)
        {
            string txt = "";
            foreach (var item in Lines)
            {
                txt += item + "\n";
            }

            if (encoding == null)
            {
                File.WriteAllText(path, txt);
            }
            else
            {
                File.WriteAllText(path, txt, encoding);
            }
        }
    }
}


