using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Megumin;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Options2StringAttribute : PropertyAttribute
    {
        public Type Type { get; set; }
        public string OverrideName { get; set; }

         static readonly Dictionary<Type, (string[] Show, string[] Value)> Cache 
            = new Dictionary<Type, (string[] Show, string[] Value)>();
        public Options2StringAttribute(Type type, string overrideName = "")
        {
            Type = type;
            OverrideName = overrideName;

            if (!Cache.TryGetValue(type,out Options))
            {
                var fields = (from field in type.GetFields()
                              where field.FieldType == typeof(string)
                              && field.IsPublic && field.IsStatic
                              select field).ToArray();

                var optionShow = new string[fields.Length];
                var optionValue = new string[fields.Length];
                Options = (optionShow, optionValue);
                Cache[type] = Options;

                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];
                    var value = field.GetValue(null) as string;
                    var show = value;
                    foreach (var attr in field.GetCustomAttributes(typeof(AliasAttribute), true))
                    {
                        if (attr is AliasAttribute alias)
                        {
                            show += $" [{alias.Alias}]";
                        }
                    }

                    optionShow[i] = show;
                    optionValue[i] = value;
                }
            }
        }

        public (string[] Show, string[] Value) Options;
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    [CustomPropertyDrawer(typeof(Options2StringAttribute))]
    internal sealed class Options2StringDrawer : PropertyDrawer
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
                label.tooltip += $"{nameof(Options2StringAttribute)}失效！\n{label.text} 字段类型必须是string";
                label.text = $"??? " + label.text;
                var old = GUI.color;
                GUI.color = warning;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = old;
            }
        }

        public void DrawEnum(SerializedProperty property, GUIContent label, Rect valuePosition)
        {
            Options2StringAttribute enum2StringAttribute = (Options2StringAttribute)attribute;
            var overrideName = property.displayName;
            if (!string.IsNullOrEmpty(enum2StringAttribute.OverrideName))
            {
                overrideName = enum2StringAttribute.OverrideName;
            }

            var myOptions = enum2StringAttribute.Options;
            if (myOptions.Show?.Length > 0)
            {
                string current = property.stringValue;

                var index = Array.IndexOf(myOptions.Value, current);

                if (string.IsNullOrEmpty(current))
                {
                    //新添加给个初值
                    index = 0;
                    property.stringValue = myOptions.Value[index];
                }

                if (index != -1)
                {
                    if (overrideName.StartsWith("Element"))
                    {
                        //容器内不显名字。
                        index = EditorGUI.Popup(valuePosition, index, myOptions.Show);
                    }
                    else
                    {
                        index = EditorGUI.Popup(valuePosition, overrideName, index, myOptions.Show);
                    }
                    property.stringValue = myOptions.Value[index];
                }
                else
                {
                    label.tooltip += $"{nameof(Options2StringAttribute)}失效！\n当前值: {current} 无法解析为{enum2StringAttribute.Type.Name}的常量。";
                    label.text = $"!! " + overrideName;
                    EditorGUI.PropertyField(valuePosition, property, label);
                }
            }
            else
            {
                label.tooltip += $"{nameof(Options2StringAttribute)}失效！\n 没有常量string！";
                label.text = $"!! " + overrideName;
                EditorGUI.PropertyField(valuePosition, property, label);
            }
        }
    }
}

#endif








