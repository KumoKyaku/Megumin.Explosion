using System;
using UnityEngine;

namespace UnityEngine
{
    /// <summary>
    /// 将枚举保存为字符串
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class Enum2StringAttribute : PropertyAttribute
    {
        public Type Type { get; set; }
        public string OverrideName { get; set; }

        public Enum2StringAttribute(Type type, string overrideName = "")
        {
            Type = type;
            OverrideName = overrideName;
        }
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    [CustomPropertyDrawer(typeof(Enum2StringAttribute))]
    internal sealed class Enum2StringDrawer : PropertyDrawer
    {
        public bool UseEnum = true;
        static readonly Color warning = new Color(1, 0.7568f, 0.0275f, 1);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var togglePosition = position;
                togglePosition.x += togglePosition.width - 14;
                togglePosition.width = 16;

                var valuePosition = position;
                valuePosition.width -= 20;
                //valuePosition.x += 20;

                ///在后面画个小勾，切换字符串还是Enum
                UseEnum = GUI.Toggle(togglePosition, UseEnum, GUIContent.none);

                if (UseEnum)
                {
                    DrawEnum(property, label, valuePosition);
                }
                else
                {
                    EditorGUI.PropertyField(valuePosition, property, label);
                }
            }
            else
            {
                //EditorGUI.HelpBox(position, $"{label.text} 字段类型必须是string", MessageType.Error);
                label.tooltip += $"Enum2StringAttribute失效！\n{label.text} 字段类型必须是string";
                label.text = $"??? " + label.text;
                var old = GUI.color;
                GUI.color = warning;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = old;
            }
        }

        public void DrawEnum(SerializedProperty property, GUIContent label, Rect valuePosition)
        {
            Enum2StringAttribute enum2StringAttribute = (Enum2StringAttribute)attribute;
            var overrideName = property.displayName;
            if (!string.IsNullOrEmpty(enum2StringAttribute.OverrideName))
            {
                overrideName = enum2StringAttribute.OverrideName;
            }

            Type type = enum2StringAttribute.Type;
            DrawEnum(property, label, valuePosition, overrideName, type);
        }

        public static void DrawEnum(SerializedProperty property,
                                    GUIContent label,
                                    Rect valuePosition,
                                    string overrideName,
                                    Type type)
        {
            if (type.IsEnum)
            {
                string current = property.stringValue;

                try
                {
                    object currentEnum = Enum.Parse(type, current, true);
                    var result = EditorGUI.EnumPopup(valuePosition, overrideName, (Enum)currentEnum);
                    property.stringValue = result.ToString();
                }
                catch (Exception)
                {
                    label.tooltip += $"Enum2StringAttribute失效！\n当前值: {current} 无法解析为枚举类型: {type.Name}";
                    label.text = $"??? " + overrideName;
                    var old = GUI.color;
                    GUI.color = warning;
                    EditorGUI.PropertyField(valuePosition, property, label);
                    GUI.color = old;
                }
            }
            else
            {
                EditorGUI.HelpBox(valuePosition, $"{overrideName}标记显示类型必须是enum", MessageType.Error);
            }
        }
    }
}

#endif
