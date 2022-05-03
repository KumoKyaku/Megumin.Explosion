using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    public abstract class LabelBase : MonoBehaviour
    {
        public string Content = "Fast Label";
        public Rect Position = new Rect(50, 50, 220, 60);
        public GUIStyle LabelStyle;

        private void Reset()
        {
            LabelStyle = null;
        }

        protected virtual void OnGUI()
        {
            if (LabelStyle == null)
            {
                InitStyle(Color.white, 18);
            }

            GUI.Label(Position, Content, LabelStyle);
        }

        [Button]
        public virtual void InitStyle(string styleName = "CN CountBadge", bool force = false)
        {
            if (LabelStyle == null || force)
            {
                LabelStyle = new GUIStyle(styleName);
            }
        }

        [Button]
        public void InitStyle(Color textColor, int fontSize = 20)
        {
            if (LabelStyle == null)
            {
                InitStyle();
            }

            LabelStyle.fontSize = fontSize;
            LabelStyle.normal.textColor = textColor;
        }
    }

    [ExecuteAlways]
    public class GlobalLabel : LabelBase
    {
        public static Object MacroTarget { get; set; }

        public static GlobalLabel Instance { get; protected set; }
        void Awake()
        {
            Instance = this;
            InitStyle(Color.white);
        }

        [Button]
        public static void Show(string text = "Fast Label")
        {
            if (Instance)
            {
                Instance.Content = text;
                Instance.InspectorForceUpdate();
            }
        }

        [Button]
        public static void Clear()
        {
            Show(null);
        }

        protected override void OnGUI()
        {
            if (LabelStyle == null)
            {
                InitStyle(Color.white);
            }

            if (string.IsNullOrEmpty(Content))
            {

            }
            else
            {
                MacroTarget.Macro(ref Content);
                GUI.Label(Position, Content, LabelStyle);
            }
        }
    }
}









