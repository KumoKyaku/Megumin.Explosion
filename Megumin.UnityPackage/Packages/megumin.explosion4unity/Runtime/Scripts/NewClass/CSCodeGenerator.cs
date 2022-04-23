using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System;

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
            if (!string.IsNullOrEmpty(code))
            {
                code = GetIndentStr(Indent) + code;
            }

            Lines.Add(code);
        }

        public void Push(CSCodeGenerator generator)
        {
            foreach (var item in generator.Lines)
            {
                Push(item);
            }
        }

        public void PushSummaryNote(params string[] notes)
        {
            if (notes == null || notes.Length == 0)
            {
                return;
            }

            if (notes.Length == 1 && string.IsNullOrEmpty(notes[0]))
            {
                return;
            }

            //增加注释
            Push("");
            Push(@$"/// <summary>");
            foreach (var item in notes)
            {
                StringReader sr = new StringReader(item);
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    Push(@$"/// <para/> {line}");
                }
            }
            Push(@$"/// </summary>");
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

        class Scope : IDisposable
        {
            CSCodeGenerator g;
            public Scope(CSCodeGenerator g)
            {
                this.g = g;
                g.BeginScope();
            }
            public void Dispose()
            {
                g.EndScope();
            }
        }

        /// <summary>
        /// 使用using
        /// </summary>
        /// <returns></returns>
        public IDisposable EnterScope()
        {
            return new Scope(this);
        }

        public IDisposable NewScope
        {
            get
            {
                return new Scope(this);
            }
        }
    }
}


