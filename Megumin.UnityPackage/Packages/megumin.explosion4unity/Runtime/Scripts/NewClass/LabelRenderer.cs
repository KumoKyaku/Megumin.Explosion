using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    [ExecuteAlways]
    public class LabelRenderer : MonoBehaviour
    {
        public bool ShowOnRuntime = true;
        public bool UseStyle = false;

        [field: SerializeField]
        public string Prefix { get; set; } = "≈ˆ◊≤∫–";

        [field: SerializeField]
        public string OverrideLabel { get; set; } = null;

        public int FontSize = 18;
        public Color FontColor = Color.white;

        public GUIStyle LabelStyle;

        void Awake()
        {
            InitStyle();
        }
        void Start()
        {

        }

        [EditorButton]
        private void InitStyle(string styleName = "CN CountBadge", bool force = false)
        {
            if (LabelStyle == null || force)
            {
                LabelStyle = new GUIStyle(styleName);
            }
        }

        void OnGUI()
        {
            if (ShowOnRuntime && Camera.main)
            {
                var pos = Camera.main.WorldToScreenPoint(transform.position);
                string text = CalText();
                var rect = new Rect(pos.x, Screen.height - pos.y, 400, 400);
                if (UseStyle)
                {
                    GUI.Label(rect, text, LabelStyle);
                }
                else
                {
                    using ((GUIColorScopeStruct)FontColor)
                    {
                        using (new GUIFontSizeScopeStruct(FontSize))
                        {
                            GUI.Label(rect, text);
                        }
                    }
                }

                //œ‘ æ¥Û–°¥ÌŒÛ
                //GUILayout.BeginArea(rect);
                //GUILayout.Label(text, LabelStyle);
                //GUILayout.EndArea();
            }
        }

        private string CalText()
        {
            string text = OverrideLabel;
            if (string.IsNullOrEmpty(text))
            {
                text = $"{Prefix}£∫[{name}|{tag}]";
            }

            this.Macro(ref text);
            return text;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (enabled)
            {
                string text = CalText();

                if (UseStyle)
                {
                    UnityEditor.Handles.Label(transform.position, text, LabelStyle);
                }
                else
                {
                    using ((GUIColorScopeStruct)FontColor)
                    {
                        using (new GUIFontSizeScopeStruct(FontSize))
                        {
                            UnityEditor.Handles.Label(transform.position, text);
                        }
                    }
                }

            }
        }
#endif

    }
}





