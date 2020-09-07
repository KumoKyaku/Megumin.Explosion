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
    public struct GUIColorScope : IDisposable
    {
        Color old;
        public GUIColorScope(Color color)
        {
#if UNITY_EDITOR
            old = GUI.color;
            GUI.color = color;
#endif
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            GUI.color = old;
#endif
        }
    }
}
