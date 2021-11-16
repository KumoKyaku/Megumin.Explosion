using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    /// <summary>
    /// 指定一个bool属性为开启关闭Toggle
    /// </summary>
    /// <remarks>标记在class上并没有作用</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public sealed class EnableableAttribute : PropertyAttribute
    {
        public string Path { get; set; } = "Enabled";
        public bool Value { get; set; } = true;
        public EnableableAttribute(string path = "Enabled", bool value = true)
        {
            Path = path;
            Value = value;
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
            SerializedObject objectReferenceValueSerializedObject = null;
            if (!string.IsNullOrEmpty(name))
            {
                toggle = property.FindPropertyRelative(name);
                if (toggle == null)
                {
                    var obj = property.objectReferenceValue;
                    if (obj)
                    {
                        objectReferenceValueSerializedObject = new SerializedObject(obj);
                        toggle = objectReferenceValueSerializedObject.FindProperty(name);
                    }
                }
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
                objectReferenceValueSerializedObject?.ApplyModifiedProperties();
                var isGray = toggle.boolValue != (attribute as EnableableAttribute).Value;
                EditorGUI.BeginDisabledGroup(isGray);
                EditorGUI.PropertyField(valuePosition, property, label, true);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
                Debug.LogWarning($"EnableableAttribute 没有找到{name}属性,没有生效");
            }
        }
    }
}

#endif

