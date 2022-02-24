using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Megumin;

namespace UnityEngine
{
    /// <summary>
    /// 暂时没法实现
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class Member2StringAttribute : PropertyAttribute
    {
        public string Path { get; set; } = "MemberOptions";
        public Member2StringAttribute(string path = "MemberOptions")
        {
            Path = path;
        }
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    //[CustomPropertyDrawer(typeof(Member2StringAttribute))]
#endif
    internal sealed class Member2StringAttributeDrawer : PropertyDrawer
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
            else if (property.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            else
            {
                //EditorGUI.HelpBox(position, $"{label.text} 字段类型必须是string", MessageType.Error);
                label.tooltip += $"{attribute.GetType().Name}失效！\n{label.text} 字段类型必须是string";
                label.text = $"??? " + label.text;
                var old = GUI.color;
                GUI.color = warning;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = old;
            }
        }

        public void DrawEnum(SerializedProperty property, GUIContent label, Rect valuePosition)
        {
            Member2StringAttribute enum2StringAttribute = (Member2StringAttribute)attribute;
            EditorGUI.PropertyField(valuePosition, property, label);
            var refProp = property.serializedObject.FindProperty(enum2StringAttribute.Path);
            //能找到属性却取不到值， NaughtyAttributes 是用反射取得值。
            
            return;

            //var pv = refProp.objectReferenceValue;
            //if ((object)pv is List<string> ls)
            //{

            //}
            //else
            //{
            //    EditorGUI.PropertyField(valuePosition, property, label);
            //}
        }
    }
}

#endif








