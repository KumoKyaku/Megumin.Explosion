using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    /// <summary>
    /// 可启用的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Enableable<T>
    {
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("Active")]
        public bool Enabled = true;
        [SerializeField]
        public T Value;

        public Enableable(bool enabled = true, T def = default)
        {
            Enabled = enabled;
            Value = def;
        }

        public static implicit operator T(Enableable<T> activeable)
        {
            return activeable.Value;
        }

        public static implicit operator bool(Enableable<T> activeable)
        {
            return activeable?.Enabled ?? false;
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

    [CustomPropertyDrawer(typeof(Enableable<>), true)]
    internal sealed class EnableableDrawer : PropertyDrawer
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
            SerializedProperty toggle = property.FindPropertyRelative("Enabled");
            toggle.boolValue = GUI.Toggle(togglePosition, toggle.boolValue, GUIContent.none);

            GUI.enabled = toggle.boolValue;
            EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("Value"), label, true);
            GUI.enabled = true;
        }
    }
}

#endif
