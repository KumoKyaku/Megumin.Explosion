using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    [Serializable]
    public class Activeable<T>
    {
        [SerializeField]
        public bool Active = true;
        [SerializeField]
        public T Value;

        public Activeable(bool active = true, T def = default)
        {
            Active = active;
            Value = def;
        }

        public static implicit operator T(Activeable<T> activeable)
        {
            return activeable.Value;
        }

        public static implicit operator bool(Activeable<T> activeable)
        {
            return activeable?.Active ?? false;
        }

        //public static implicit operator Nullable<T>(Activeable<T> activeable)
        //{
        //    return default;
        //}
    }

}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using global::Megumin;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(Activeable<>), true)]
    internal sealed class ActiveableDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var prop = property.FindPropertyRelative("Value");
            return EditorGUI.GetPropertyHeight(prop);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var togglePosition = position;
            togglePosition.x += togglePosition.width - 14;
            togglePosition.width = 16;

            var valuePosition = position;
            valuePosition.width -= 20;
            SerializedProperty toggle = property.FindPropertyRelative("Active");
            toggle.boolValue = GUI.Toggle(togglePosition, toggle.boolValue, GUIContent.none);

            GUI.enabled = toggle.boolValue;
            EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("Value"), label, true);
            GUI.enabled = true;
        }
    }
}

#endif
