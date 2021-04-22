using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine
{
    /// <summary>
    /// gui颜色区域
    /// </summary>
    public struct GUIColorScopeStruct : IDisposable
    {
        Color old;
        public GUIColorScopeStruct(Color color)
        {
            old = GUI.color;
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = old;
        }

        public static implicit operator GUIColorScopeStruct(in Color color)
        {
            return new GUIColorScopeStruct(color);
        }
    }

    /// <summary>
    /// gui颜色区域
    /// </summary>
    public class GUIColorScope : IDisposable
    {
        Color old;
        public GUIColorScope(Color color)
        {
            old = GUI.color;
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = old;
        }

        public static implicit operator GUIColorScope(in Color color)
        {
            return new GUIColorScope(color);
        }
    }


    public struct GUIFontSizeScopeStruct : IDisposable
    {
        int old;
        public GUIFontSizeScopeStruct(int size)
        {
            old = GUI.skin.label.fontSize;
            GUI.skin.label.fontSize = size;
        }

        public void Dispose()
        {
            GUI.skin.label.fontSize = old;
        }
    }
}

