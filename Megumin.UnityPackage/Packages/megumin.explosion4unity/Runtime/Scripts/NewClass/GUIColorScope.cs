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
#if UNITY_EDITOR
            GUI.color = color;
#endif
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            GUI.color = old;
#endif
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
#if UNITY_EDITOR
            GUI.color = color;
#endif
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            GUI.color = old;
#endif
        }

        public static implicit operator GUIColorScope(in Color color)
        {
            return new GUIColorScope(color);
        }
    }
}

