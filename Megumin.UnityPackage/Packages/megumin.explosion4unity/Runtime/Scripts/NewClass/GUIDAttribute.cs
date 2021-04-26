using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class GUIDAttribute : PropertyAttribute
    {

    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    [CustomPropertyDrawer(typeof(GUIDAttribute))]
    internal sealed class GUIDDrawer : PropertyDrawer
    {
        static GUIStyle left = new GUIStyle("minibuttonleft");
        static GUIStyle right = new GUIStyle("minibuttonright");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var propertyPosition = position;
                propertyPosition.width -= 86;

                var buttonPosition = position;
                buttonPosition.width = 80;
                buttonPosition.x += position.width - 80;

                GUI.enabled = false;
                EditorGUI.PropertyField(propertyPosition, property, label);

                var leftPosotion = buttonPosition;
                leftPosotion.width = 40;
                var rightPosition = buttonPosition;
                rightPosition.width = 40;
                rightPosition.x += 40;

                string current = property.stringValue;

                if (string.IsNullOrEmpty(current))
                {
                    current = GUID.Generate().ToString();
                    property.stringValue = current;
                }

                GUI.enabled = true;

                if (GUI.Button(leftPosotion, "GUID", left))
                {
                    property.stringValue = GUID.Generate().ToString();
                }

                GUI.enabled = true;

                if (GUI.Button(rightPosition, "Copy", right))
                {
                    GUIUtility.systemCopyBuffer = property.stringValue;
                }
            }
            else
            {
                label.tooltip += $"{nameof(GUIDAttribute)}失效！\n{label.text} 字段类型必须是string";
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}

#endif









