using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Megumin;

namespace UnityEngine
{
    /// <summary>
    /// 在帧数后面提示时长
    /// </summary>
    public class FrameAndTimeAttribute : PropertyAttribute
    {
        public int FrameRate { get; set; } = 60;

        public FrameAndTimeAttribute(int frameRate = 60)
        {
            FrameRate = frameRate;
        }
    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    [CustomPropertyDrawer(typeof(FrameAndTimeAttribute))]
    internal sealed class Frame2TimeDrawer : PropertyDrawer
    {
        public bool inputmode = false;
        static readonly Color warning = new Color(1, 0.7568f, 0.0275f, 1);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                var propertyPosition = position;
                propertyPosition.width -= 100;

                var togglePosition = position;
                togglePosition.x += togglePosition.width - 14;
                togglePosition.width = 16;

                var timePosition = position;
                timePosition.x += position.width - 100;
                timePosition.width = 80;

                inputmode = GUI.Toggle(togglePosition, inputmode, GUIContent.none);
                FrameAndTimeAttribute frame2TimeAttribute = (FrameAndTimeAttribute)attribute;

                var rate = frame2TimeAttribute.FrameRate;
                var value = property.intValue;
                var time = ((float)value / rate);

                if (inputmode)
                {
                    GUI.enabled = false;
                    EditorGUI.PropertyField(propertyPosition, property, label);
                    GUI.enabled = true;

                    time = EditorGUI.FloatField(timePosition, time);
                    var f = (int)(time * rate);
                    property.intValue = f;
                }
                else
                {
                    EditorGUI.PropertyField(propertyPosition, property, label);
                    GUI.enabled = false;
                    EditorGUI.FloatField(timePosition, time);
                    GUI.enabled = true;
                }
            }
            else
            {
                //EditorGUI.HelpBox(position, $"{label.text} 字段类型必须是string", MessageType.Error);
                label.tooltip += $"{nameof(FrameAndTimeAttribute)}失效！\n{label.text} 字段类型必须是int";
                label.text = $"??? " + label.text;
                var old = GUI.color;
                GUI.color = warning;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = old;
            }
        }
    }
}

#endif








