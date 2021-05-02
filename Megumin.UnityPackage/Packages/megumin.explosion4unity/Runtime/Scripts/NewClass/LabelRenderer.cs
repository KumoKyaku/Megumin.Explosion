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
        public string Prefix { get; set; } = "碰撞盒";

        [field: SerializeField]
        public string OverrideLabel { get; set; } = null;

        public int FontSize = 18;
        public Color FontColor = Color.white;

        public GUIStyle LabelStyle;

        /// <summary>
        /// 全局显示开关
        /// </summary>
        public static Pref<bool> GlobalToggle;

        void Awake()
        {
            InitStyle();
        }
        void Start()
        {
            InitGlobalToggle();
        }

        protected static void InitGlobalToggle()
        {
            if (GlobalToggle == null)
            {
                GlobalToggle = new Pref<bool>(nameof(LabelRenderer), true);
            }
        }

        [EditorButton]
        public void SwitchGlobalToggle()
        {
            InitGlobalToggle();
            GlobalToggle.Value = !GlobalToggle;
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
            InitGlobalToggle();
            if (ShowOnRuntime && Camera.main && GlobalToggle)
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
                    using ((ValueGUIColor)FontColor)
                    {
                        using (new ValueGUIFontSize(FontSize))
                        {
                            GUI.Label(rect, text);
                        }
                    }
                }

                //显示大小错误
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
                text = $"{Prefix}：[{name}|{tag}]";
            }

            this.Macro(ref text);
            return text;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            InitGlobalToggle();
            if (enabled && GlobalToggle)
            {
                string text = CalText();

                if (UseStyle)
                {
                    UnityEditor.Handles.Label(transform.position, text, LabelStyle);
                }
                else
                {
                    using ((ValueGUIColor)FontColor)
                    {
                        using (new ValueGUIFontSize(FontSize))
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





