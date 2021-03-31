using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Options2StringAttribute : PropertyAttribute
    {
        public Type Type { get; set; }
        public string OverrideName { get; set; }

        static readonly Dictionary<Type, string[]> Cache = new Dictionary<Type, string[]>();
        public Options2StringAttribute(Type type, string overrideName = "")
        {
            Type = type;
            OverrideName = overrideName;

            if (!Cache.TryGetValue(type, out allConst))
            {
                allConst = (from field in type.GetFields()
                            where field.FieldType == typeof(string)
                            && field.IsPublic && field.IsStatic
                            select field.GetValue(null) as string).ToArray();
                Cache[type] = allConst;
            }
        }

        public string[] allConst = null;
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

                ///�ں��滭��С�����л��ַ�������Enum
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
                //EditorGUI.HelpBox(position, $"{label.text} �ֶ����ͱ�����string", MessageType.Error);
                label.tooltip += $"{nameof(Options2StringAttribute)}ʧЧ��\n{label.text} �ֶ����ͱ�����string";
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

            string[] options = enum2StringAttribute.allConst;
            if (options?.Length > 0)
            {
                string current = property.stringValue;

                var index = Array.IndexOf(options, current);

                if (string.IsNullOrEmpty(current))
                {
                    //����Ӹ�����ֵ
                    index = 0;
                    property.stringValue = options[index];
                }

                if (index != -1)
                {
                    if (overrideName.StartsWith("Element"))
                    {
                        //�����ڲ������֡�
                        index = EditorGUI.Popup(valuePosition, index, options);
                    }
                    else
                    {
                        index = EditorGUI.Popup(valuePosition, overrideName, index, options);
                    }
                    property.stringValue = options[index];
                }
                else
                {
                    label.tooltip += $"{nameof(Options2StringAttribute)}ʧЧ��\n��ǰֵ: {current} �޷�����Ϊ{enum2StringAttribute.Type.Name}�ĳ�����";
                    label.text = $"!! " + overrideName;
                    EditorGUI.PropertyField(valuePosition, property, label);
                }
            }
            else
            {
                label.tooltip += $"{nameof(Options2StringAttribute)}ʧЧ��\n û�г���string��";
                label.text = $"!! " + overrideName;
                EditorGUI.PropertyField(valuePosition, property, label);
            }
        }
    }
}

#endif








