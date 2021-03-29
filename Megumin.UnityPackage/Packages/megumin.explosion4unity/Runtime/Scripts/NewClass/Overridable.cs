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
        private T defaultValue;
        [SerializeField]
        public bool IsOverride = false;
        [SerializeField]
        public T MyOverrideValue;

        public Overridable()
        {

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
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var togglePosition = position;

            SerializedProperty toggle = property.FindPropertyRelative("IsOverride");
            EditorGUI.PropertyField(togglePosition, toggle, GUIContent.none);

            var valuePosition = position;
            valuePosition.width -= 20;
            valuePosition.x += 20;

            if (toggle.boolValue)
            {
                EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("MyOverrideValue"), label);
            }
            else
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    label.text += " [Default]";
                    EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("defaultValue"), label);
                }
            }
        }
    }
}

#endif



