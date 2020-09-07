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

namespace Enum2StringDrawer
{
    using UnityEditor;
    [CustomPropertyDrawer(typeof(Enum2StringAttribute))]
    internal sealed class Enum2StringDrawer : PropertyDrawer
    {
        static readonly Color warning = new Color(1, 0.7568f, 0.0275f, 1);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                Enum2StringAttribute enum2StringAttribute = (Enum2StringAttribute)attribute;
                var overrideName = property.displayName;
                if (!string.IsNullOrEmpty(enum2StringAttribute.OverrideName))
                {
                    overrideName = enum2StringAttribute.OverrideName;
                }

                Type type = enum2StringAttribute.Type;
                if (type.IsEnum)
                {
                    string current = property.stringValue;

                    try
                    {
                        object currentEnum = Enum.Parse(type, current, true);
                        var result = EditorGUI.EnumPopup(position, overrideName, (Enum)currentEnum);
                        property.stringValue = result.ToString();
                    }
                    catch (Exception)
                    {
                        label.tooltip += $"Enum2StringAttribute失效！\n当前值: {current} 无法解析为枚举类型: {type.Name}";
                        label.text = $"??? " + overrideName;
                        var old = GUI.color;
                        GUI.color = warning;
                        EditorGUI.PropertyField(position, property, label);
                        GUI.color = old;
                    }
                }
                else
                {
                    EditorGUI.HelpBox(position, $"{overrideName}标记显示类型必须是enum", MessageType.Error);
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
    }
}

#endif
