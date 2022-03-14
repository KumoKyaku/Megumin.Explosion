using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Megumin
{
    public static class PropertyDrawerUtility_011f30b31b027cd438bcb0f0e46a1564
    {
        static readonly Color warning = new Color(1, 0.7568f, 0.0275f, 1);

#if UNITY_EDITOR
        public static void DrawOptions(this PropertyDrawer propertyDrawer,
            SerializedProperty property,
            Rect valuePosition,
            (string[] Show, string[] Value) myOptions,
            string overrideName,
            string defaultValue,
            GUIContent label)
        {
            //防止空指针检查
            if (myOptions.Show == null)
            {
                myOptions = (myOptions.Value, myOptions.Value);
            }

            var current = property.stringValue;
            var index = Array.IndexOf(myOptions.Value, current);

            if (string.IsNullOrEmpty(current))
            {
                if (defaultValue == null)
                {
                    //新添加给个初值
                    index = 0;
                    property.stringValue = myOptions.Value[index];
                }
                else
                {
                    current = defaultValue;
                    index = Array.IndexOf(myOptions.Value, current);
                    property.stringValue = current;
                }
            }

            if (index != -1)
            {
                if (!string.IsNullOrEmpty(overrideName) && overrideName.StartsWith("Element"))
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
                label.tooltip += $"{propertyDrawer.attribute.GetType().Name}失效！\n当前值: {current} 无法解析为目标值。";
                label.text = $"!! " + overrideName;
                EditorGUI.PropertyField(valuePosition, property, label);
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
            GUI.color = warning;
            EditorGUI.PropertyField(position, property, label);
            GUI.color = old;
        }
#endif

    }
}

