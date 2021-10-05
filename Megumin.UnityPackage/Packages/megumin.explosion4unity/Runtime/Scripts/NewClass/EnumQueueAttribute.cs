using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumQueueAttribute : PropertyAttribute
    {
        public Type Type { get; set; }

        public EnumQueueAttribute(Type type)
        {
            Type = type;
        }
    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    [CustomPropertyDrawer(typeof(EnumQueueAttribute))]
    internal sealed class EnumQueueDrawer : PropertyDrawer
    {
        string[] names = null;
        int[] values = null;
        string[] names2 = null;
        int[] values2 = null;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                var attri = attribute as EnumQueueAttribute;
                var enumType = attri.Type;

                if (!enumType.IsEnum)
                {
                    EditorGUI.PropertyField(position, property, label);
                    return;
                }

                if (names == null)
                {
                    //初始化枚举列表
                    names = Enum.GetNames(enumType);
                    values = Enum.GetValues(enumType).Cast<int>().ToArray();
                    names2 = new string[values.Length + 1];
                    values2 = new int[values.Length + 1];

                    for (int i = 0; i < names.Length; i++)
                    {
                        names2[i] = names[i];
                        values2[i] = values[i];
                    }
                }

                float popWidth = 80 + (int)(position.width / 5);
                popWidth = Math.Clamp(popWidth, 150, 400);
                Rect propRect = position;
                propRect.width -= popWidth + 10;
                Rect popRect = position;
                popRect.xMin = popRect.xMax - popWidth;

                var currentQueue = property.intValue;
                EditorGUI.PropertyField(propRect, property, label);

                var count = names.Length;
                var index = -1;
                var nearIndex = -1;
                var min = int.MaxValue;
                for (int i = 0; i < count + 1; i++)
                {
                    var v = values2[i];
                    if (v == currentQueue)
                    {
                        index = i;
                        break;
                    }
                    else
                    {
                        //找到最近枚举
                        var delta = currentQueue - v;
                        if (Math.Abs(delta) < Math.Abs(min))
                        {
                            nearIndex = i;
                            min = delta;
                        }
                    }
                }

                var newIndex = index;
                if (index == -1)
                {
                    //现有值不包含当前值
                    index = count;
                    if (min > 0)
                    {
                        names2[count] = $"{names[nearIndex]}+{min}";
                    }
                    else
                    {
                        names2[count] = $"{names[nearIndex]}{min}";
                    }

                    newIndex = EditorGUI.Popup(popRect, index, names2);
                }
                else
                {
                    newIndex = EditorGUI.Popup(popRect, index, names);
                }

                if (newIndex != index)
                {
                    currentQueue = (int)Enum.Parse(enumType, names[newIndex]);
                    property.intValue = currentQueue;
                }

            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}

#endif

