using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Megumin;
using System.Reflection;

namespace Megumin
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
    [CustomPropertyDrawer(typeof(Member2StringAttribute))]
#endif
    internal sealed class Member2StringAttributeDrawer : PropertyDrawer
    {
        public bool UseOption = true;
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

                ///在后面画个小勾
                UseOption = GUI.Toggle(togglePosition, UseOption, GUIContent.none);

                if (UseOption)
                {
                    DrawOptions(property, label, valuePosition);
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
                this.NotMatch(position, property, label);
            }
        }


        public object cacheObject = null;
        public string cacheMember = null;
        public string[] cacheOption = null;
        public void DrawOptions(SerializedProperty property, GUIContent label, Rect valuePosition)
        {
            Member2StringAttribute enum2StringAttribute = (Member2StringAttribute)attribute;

            //能找到属性却取不到值， NaughtyAttributes 是用反射取得值。
            //var refProp = property.serializedObject.FindProperty(enum2StringAttribute.Path);

            if (!System.Object.Equals(cacheObject, property.serializedObject.targetObject)
                || cacheMember != enum2StringAttribute.Path)
            {
                var hasValue = ReflectionUtility.TryGetValue(
                    property.serializedObject.targetObject,
                    enum2StringAttribute.Path, out var memberValue);
                string[] options = null;
                if (hasValue)
                {
                    if (memberValue is List<string> list)
                    {
                        options = list.ToArray();
                    }
                    else if (memberValue is string[] array)
                    {
                        options = array;
                    }
                    else if (memberValue is IEnumerable<string> enumerable)
                    {
                        options = enumerable.ToArray();
                    }
                    else if (memberValue is IEnumerable<object> enumerableObject)
                    {
                        options = new string[enumerableObject.Count()];
                        var index = 0;
                        foreach (var item in enumerableObject)
                        {
                            options[index] = item.ToString();
                            index++;
                        }
                    }
                }

                cacheObject = property.serializedObject.targetObject;
                cacheMember = enum2StringAttribute.Path;
                cacheOption = options;

            }

            if (cacheOption != null)
            {
                this.DrawOptions(property,
                    valuePosition,
                    (cacheOption, cacheOption),
                    property.displayName,
                    null,
                    label);
            }
            else
            {
                EditorGUI.PropertyField(valuePosition, property, label);
            }

            return;
        }
    }
}

#endif








