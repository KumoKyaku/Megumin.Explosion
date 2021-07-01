using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>unity支持泛型序列化后可用</remarks>
    [Serializable]
    public class Overridable<T>
    {
        [SerializeField]
        protected T defaultValue;
        [SerializeField]
        public bool IsOverride = false;
        //[ColorUsage(true, true)] //加了其他类型会报错，支持不了HDR
        [SerializeField]
        public T MyOverrideValue;

        public Overridable()
        {
            if (typeof(Color) == typeof(T))
            {
                defaultValue = (T)(object)Color.gray;
            }
        }

        public Overridable(T defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public T Value
        {
            get
            {
                if (IsOverride)
                {
                    return MyOverrideValue;
                }
                else
                {
                    return defaultValue;
                }
            }
            set
            {
                IsOverride = true;
                MyOverrideValue = value;
            }
        }

        public static implicit operator T(Overridable<T> overrideValue)
        {
            return overrideValue.Value;
        }
    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using global::Megumin;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(Overridable<>), true)]
    internal sealed class OverrideValueDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var prop = property.FindPropertyRelative("MyOverrideValue");
            return EditorGUI.GetPropertyHeight(prop);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var togglePosition = position;
            togglePosition.x += togglePosition.width - 14;
            togglePosition.width = 16;

            var valuePosition = position;
            valuePosition.width -= 20;
            //valuePosition.x += 20;

            SerializedProperty toggle = property.FindPropertyRelative("IsOverride");
            //EditorGUI.PropertyField(togglePosition, toggle, GUIContent.none);
            toggle.boolValue = GUI.Toggle(togglePosition, toggle.boolValue, GUIContent.none);

            if (toggle.boolValue)
            {
                EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("MyOverrideValue"), label, true);
            }
            else
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    label.text += " [Default]";
                    EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("defaultValue"), label, true);
                }
            }
        }
    }
}

#endif



