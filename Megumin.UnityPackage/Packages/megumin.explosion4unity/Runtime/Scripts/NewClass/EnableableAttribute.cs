using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    /// <summary>
    /// 指定一个bool属性为开启关闭Toggle
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public sealed class EnableableAttribute : PropertyAttribute
    {
        public string Path { get; set; } = "Enabled";

        public EnableableAttribute(string path = "Enabled")
        {
            Path = path;
        }
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(EnableableAttribute))]
    public class EnableableAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var name = (attribute as EnableableAttribute).Path;
            SerializedProperty toggle = null;
            if (!string.IsNullOrEmpty(name))
            {
                toggle = property.FindPropertyRelative(name); ;
            }

            if (toggle != null)
            {
                var togglePosition = position;
                togglePosition.x += togglePosition.width - 14;
                //togglePosition.y = 0;
                togglePosition.width = 16;

                var valuePosition = position;
                valuePosition.width -= 20;

                toggle.boolValue = GUI.Toggle(togglePosition, toggle.boolValue, GUIContent.none);
                EditorGUI.BeginDisabledGroup(!toggle.boolValue);
                EditorGUI.PropertyField(valuePosition, property, label, true);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}

#endif

