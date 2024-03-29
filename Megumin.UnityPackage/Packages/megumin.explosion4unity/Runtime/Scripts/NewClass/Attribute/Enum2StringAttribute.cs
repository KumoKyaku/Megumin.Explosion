﻿using System;
using UnityEngine;
using Megumin;

namespace Megumin
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

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(Enum2StringAttribute))]
#endif
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
                this.NotMatch(position, property, label);
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
                    if (string.IsNullOrEmpty(current))
                    {
                        //空串给个初值。
                        current = Enum.GetNames(type)?[0];
                    }

                    if (Enum.TryParse(type, current, true, out var currentEnum))
                    {
                        EditorGUI.BeginProperty(valuePosition, label, property);
                        
                        var result = EditorGUI.EnumPopup(valuePosition, overrideName, (Enum)currentEnum);
                        property.stringValue = result.ToString();
                        
                        EditorGUI.EndProperty();
                    }
                    else
                    {
                        DrawWarning(property, label, valuePosition, overrideName, type, current);
                    }
                }
                catch (Exception)
                {
                    DrawWarning(property, label, valuePosition, overrideName, type, current);
                }
            }
            else
            {
                EditorGUI.HelpBox(valuePosition, $"{overrideName}标记显示类型必须是enum", MessageType.Error);
            }
        }

        static void DrawWarning(SerializedProperty property, GUIContent label, Rect valuePosition, string overrideName, Type type, string current)
        {
            label.tooltip += $"{nameof(Enum2StringAttribute)}失效！\n当前值: {current} 无法解析为枚举类型: {type.Name}";
            label.text = $"??? " + overrideName;
            var old = GUI.color;
            GUI.color = warning;
            EditorGUI.PropertyField(valuePosition, property, label);
            GUI.color = old;
        }
    }
}

#endif
