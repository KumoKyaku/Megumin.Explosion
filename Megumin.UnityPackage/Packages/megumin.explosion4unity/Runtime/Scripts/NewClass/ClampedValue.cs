using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    [Serializable]
    public class ClampedValue<T> : Overridable<T>
    {
        public T Min;
        public T Max;
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using global::Megumin;
    using UnityEditor;

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(ClampedValue<>), true)]
#endif
    internal sealed class ClampedValueDrawer : PropertyDrawer
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

            SerializedProperty myOverrideValue = property.FindPropertyRelative("MyOverrideValue");
            SerializedProperty min = property.FindPropertyRelative("Min");
            SerializedProperty max = property.FindPropertyRelative("Max");

            if (toggle.boolValue)
            {
                EditorGUI.PropertyField(valuePosition, myOverrideValue, label, true);
            }
            else
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    label.text += " [Default]";
                    EditorGUI.PropertyField(valuePosition, myOverrideValue, label, true);
                }
            }
        }
    }
}

#endif
