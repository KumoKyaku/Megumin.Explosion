using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///https://forum.unity.com/threads/helpattribute-allows-you-to-use-helpbox-in-the-unity-inspector-window.462768/#post-3014998

namespace Megumin
{
    using UnityEngine;

    public enum HelpBoxMessageType { None, Info, Warning, Error }

    /// <summary>
    /// https://learn.microsoft.com/zh-cn/visualstudio/extensibility/walkthrough-displaying-quickinfo-tooltips?view=vs-2022&tabs=csharp
    /// https://developercommunity.visualstudio.com/t/visual-studio-tools-for-unity-use-unityenginetoolt/683906
    /// https://developercommunity.visualstudio.com/t/Visual-Studio-Tools-for-Unity:-Custom-At/10187477?ftype=idea
    /// </summary>
    public class HelpBoxAttribute : PropertyAttribute
    {

#if HELPBOX_DISABLE
        public static bool Enable = false;
#else
        public static bool Enable = true;
#endif

        /// <summary>
        /// TODO:调用webAPI翻译.
        /// </summary>
        public static bool AutoTranlate = false;

        public static Dictionary<string, Dictionary<SystemLanguage, string>> Translation { get; }
            = new Dictionary<string, Dictionary<SystemLanguage, string>>();

        Dictionary<SystemLanguage, string> MyTranslation = new Dictionary<SystemLanguage, string>();
        public HelpBoxMessageType MessageType { get; set; }

        public string Text
        {
            get
            {
                if (Application.systemLanguage == Language)
                {
                    return text;
                }
                else
                {
                    if (MyTranslation.TryGetValue(Application.systemLanguage, out var ret))
                    {
                        return ret;
                    }
                    else if (Translation.TryGetValue(text, out var dic))
                    {
                        if (dic.TryGetValue(Application.systemLanguage, out ret))
                        {
                            return ret;
                        }
                        else
                        {
                            //Todo :调用翻译api.
                            return text;
                        }
                    }
                }
                return text;
            }
            set => text = value;
        }

        protected SystemLanguage Language = SystemLanguage.ChineseSimplified;

        public string EnglishText
        {
            get => englishText;
            set
            {
                englishText = value;
                MyTranslation[SystemLanguage.English] = value;
            }
        }

        private string text;
        private string englishText;

        public HelpBoxAttribute(string text,
                                HelpBoxMessageType messageType = HelpBoxMessageType.None,
                                SystemLanguage textLanguage = SystemLanguage.ChineseSimplified)
        {
            this.Text = text;
            Language = textLanguage;
            MyTranslation[textLanguage] = text;
            this.MessageType = messageType;
        }

        public string QuickInfo => Text;

        //// 不是有效的特性参数类型
        //public HelpBoxAttribute(string text, KV[] tranlate, HelpBoxMessageType messageType = HelpBoxMessageType.None)
        //{
        //    this.text = text;
        //    this.MessageType = messageType;
        //}

        //public struct KV
        //{
        //    public SystemLanguage SystemLanguage { get; set; }
        //    public string text { get; set; }
        //}
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{

    using UnityEngine;
    using UnityEditor;
    using global::Megumin;

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
#endif
    public class HelpBoxAttributeDrawer : DecoratorDrawer
    {

        public override float GetHeight()
        {
            if (HelpBoxAttribute.Enable)
            {
                var helpBoxAttribute = attribute as HelpBoxAttribute;
                if (helpBoxAttribute == null) return base.GetHeight();
                var helpBoxStyle = (GUI.skin != null) ? GUI.skin.GetStyle("helpbox") : null;
                if (helpBoxStyle == null) return base.GetHeight();
                return Mathf.Max(14f, helpBoxStyle.CalcHeight(new GUIContent(helpBoxAttribute.Text), EditorGUIUtility.currentViewWidth) + 4);
            }
            else
            {
                return 0;
            }
        }

        public override void OnGUI(Rect position)
        {
            var helpBoxAttribute = attribute as HelpBoxAttribute;
            if (helpBoxAttribute == null || HelpBoxAttribute.Enable == false)
            {
                return;
            }

            EditorGUI.HelpBox(position, helpBoxAttribute.Text, GetMessageType(helpBoxAttribute.MessageType));
        }

        private MessageType GetMessageType(HelpBoxMessageType helpBoxMessageType)
        {
            switch (helpBoxMessageType)
            {
                default:
                case HelpBoxMessageType.None: return MessageType.None;
                case HelpBoxMessageType.Info: return MessageType.Info;
                case HelpBoxMessageType.Warning: return MessageType.Warning;
                case HelpBoxMessageType.Error: return MessageType.Error;
            }
        }
    }
}

#endif
