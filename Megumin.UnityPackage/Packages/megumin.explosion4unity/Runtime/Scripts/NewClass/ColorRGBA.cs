using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 用于将一个颜色转成颜色码,或者解析颜色码,当作编辑器工具来使用
    /// </summary>
    [ExecuteAlways]
    public class ColorRGBA : MonoBehaviour
    {
        public Color Orignal;

        public string OrignalString;

        [Space(20)]
        public string ParseColorString = "#00FF00FF";

        public Color ParsedColor = Color.green;
        public bool ParseResult = true;


        void OnValidate()
        {
            OrignalString = "#" + ColorUtility.ToHtmlStringRGBA(Orignal);
            if (!ParseColorString.StartsWith("#"))
            {
                ParseColorString = "#" + ParseColorString;
            }
            ParseResult = ColorUtility.TryParseHtmlString(ParseColorString, out ParsedColor);
        }
    }
}

