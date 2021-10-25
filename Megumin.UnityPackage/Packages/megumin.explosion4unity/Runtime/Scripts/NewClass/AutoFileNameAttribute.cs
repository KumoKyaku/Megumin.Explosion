using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class AutoFileNameAttribute : PropertyAttribute
    {
    }

}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(AutoFileNameAttribute))]
    internal sealed class AutoFileNameAttributeDrawer : PropertyDrawer
    {
        static readonly Color warning = new Color(1, 0.7568f, 0.0275f, 1);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var propertyPosition = position;
                propertyPosition.width -= 86;

                var buttonPosition = position;
                buttonPosition.width = 80;
                buttonPosition.x += position.width - 80;

                string fileName = property.serializedObject.targetObject.name;
                if (property.stringValue != fileName)
                {
                    property.stringValue = fileName;
                }

                using(new EditorGUI.DisabledScope(true))
                {
                    EditorGUI.PropertyField(propertyPosition, property, label);
                }

                if (GUI.Button(buttonPosition, "Copy"))
                {
                    GUIUtility.systemCopyBuffer = property.stringValue;
                }
            }
            else
            {
                label.tooltip += $"{nameof(AutoFileNameAttribute)}失效！\n{label.text} 字段类型必须是string";
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



