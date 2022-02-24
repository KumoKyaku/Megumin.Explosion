using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Megumin
{
    [ExecuteAlways]
    public class LabelRenderer : MonoBehaviour
    {
        [ProtectedInInspector]
        public Camera Camera;

        public bool ShowOnRuntime = true;
        public bool UseStyle = false;

        [field: SerializeField]
        public string Prefix { get; set; } = "碰撞盒";

        [field: SerializeField]
        public string OverrideLabel { get; set; } = null;

        public int FontSize = 18;
        public Color FontColor = Color.white;
        public Vector2 Size = new Vector2(400, 200);
        public Vector3 Offset = Vector3.zero;
        [Range(-1, 1)]
        public float ScreenOffsetX = 0f;
        [Range(-1, 1)]
        public float ScreenOffsetY = 0f;
        public GUIStyle LabelStyle;

        /// <summary>
        /// 全局显示开关
        /// </summary>
        public static Pref<bool> GlobalToggle;
        public readonly static ActiveControl ActiveControl = new ActiveControl(new object(), true, ascending: true);

        [ReadOnlyInInspector]
        public string cacheText;

        void Awake()
        {
            InitStyle();
        }

        void Start()
        {
            if (!Camera)
            {
                Camera = GetComponent<Camera>();
            }

            if (!Camera)
            {
                Camera = Camera.main;
            }

            InitGlobalToggle();
        }

        private void Update()
        {
            if (!Camera)
            {
                Camera = Camera.main;
            }

            cacheText = CalText();
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
            if (ShowOnRuntime && Camera && GlobalToggle && ActiveControl.Current)
            {
                var pos = Camera.WorldToScreenPoint(transform.position + Offset);
                if (pos.z > 0)
                {
                    string text = cacheText;

                    if (!string.IsNullOrEmpty(text))
                    {
                        var rect = new Rect(pos.x + (Screen.width * ScreenOffsetX),
                                            Screen.height - pos.y + (Screen.height * ScreenOffsetY),
                                            400,
                                            200);
                        rect.size = Size;

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
            }
        }

        protected virtual string CalText()
        {
            Profiler.BeginSample(nameof(CalText));
            string text = OverrideLabel;
            if (string.IsNullOrEmpty(text))
            {
                text = $"{Prefix}：[{name}|{tag}]";
            }

            this.Macro(ref text);
            Profiler.EndSample();
            return text;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            InitGlobalToggle();
            if (enabled && GlobalToggle)
            {
                string text = cacheText;

                if (!string.IsNullOrEmpty(text))
                {
                    if (UseStyle)
                    {
                        UnityEditor.Handles.Label(transform.position + Offset, text, LabelStyle);
                    }
                    else
                    {
                        using ((ValueGUIColor)FontColor)
                        {
                            using (new ValueGUIFontSize(FontSize))
                            {
                                UnityEditor.Handles.Label(transform.position + Offset, text);
                            }
                        }
                    }
                }
            }
        }
#endif

    }
}





