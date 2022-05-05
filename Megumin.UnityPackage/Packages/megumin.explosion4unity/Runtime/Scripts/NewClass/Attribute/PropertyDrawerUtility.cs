using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Megumin
{
    public static class PropertyDrawerUtility_011f30b31b027cd438bcb0f0e46a1564
    {
        static readonly Color opNotMatch = new Color(0.75f, 0.67f, 0.44f, 1);
        static readonly Color typeNotMatch = new Color(1, 0.7568f, 0.0275f, 1);

#if UNITY_EDITOR
        public static void DrawOptions(this PropertyDrawer propertyDrawer,
            SerializedProperty property,
            Rect valuePosition,
            string[] opShow,
            string[] opValue,
            string overrideName,
            string defaultValue,
            GUIContent label)
        {
            //防止空指针检查
            if (opShow == null)
            {
                opShow = opValue;
            }

            var current = property.stringValue;
            var index = Array.IndexOf(opValue, current);

            if (string.IsNullOrEmpty(current))
            {
                if (defaultValue == null)
                {
                    //新添加给个初值
                    index = 0;
                    property.stringValue = opValue[index];
                }
                else
                {
                    current = defaultValue;
                    index = Array.IndexOf(opValue, current);
                    property.stringValue = current;
                }
            }

            if (index != -1)
            {
                EditorGUI.BeginProperty(valuePosition, label, property);
                if (!string.IsNullOrEmpty(overrideName) && overrideName.StartsWith("Element"))
                {
                    //容器内不显名字。
                    index = EditorGUI.Popup(valuePosition, index, opShow);
                }
                else
                {
                    index = EditorGUI.Popup(valuePosition, overrideName, index, opShow);
                }
                property.stringValue = opValue[index];
                EditorGUI.EndProperty();
            }
            else
            {
                label.tooltip += $"{propertyDrawer.attribute.GetType().Name}失效！\n当前值: {current} 无法解析为目标值。";
                label.text = $"!! " + overrideName;
                var old = GUI.color;
                GUI.color = opNotMatch;
                EditorGUI.PropertyField(valuePosition, property, label);
                GUI.color = old;
            }
        }

        public static void NotMatch(this PropertyDrawer propertyDrawer,
            Rect position,
            SerializedProperty property,
            GUIContent label,
            string tip = "字段类型必须是String")
        {
            //EditorGUI.HelpBox(position, $"{label.text} 字段类型必须是string", MessageType.Error);
            label.tooltip += $"{propertyDrawer.attribute.GetType().Name}失效！\n{label.text} {tip}";
            label.text = $"??? " + label.text;
            var old = GUI.color;
            GUI.color = typeNotMatch;
            EditorGUI.PropertyField(position, property, label);
            GUI.color = old;
        }
#endif

    }
}

