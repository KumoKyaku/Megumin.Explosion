using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;
using System;

namespace UnityEngine
{
    public class SnapAttribute : PropertyAttribute
    {
        public double Value { get; set; } = 1;

        public SnapAttribute(double value)
        {
            Value = value;
        }
    }

}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    [CustomPropertyDrawer(typeof(SnapAttribute))]
    internal sealed class SnapDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer
                || property.propertyType == SerializedPropertyType.Float)
            {
                var propertyPosition = position;
                propertyPosition.width -= 86;

                var buttonPosition = position;
                buttonPosition.width = 80;
                buttonPosition.x += position.width - 80;

                EditorGUI.PropertyField(propertyPosition, property, label);
                SnapAttribute snapAttribute = attribute as SnapAttribute;

                if (GUI.Button(buttonPosition, $"Snap({snapAttribute.Value})"))
                {
                    if (property.propertyType == SerializedPropertyType.Integer)
                    {
                        var ret = property.intValue;
                        ret.SnapCeil(snapAttribute.Value);
                        property.intValue = ret;
                    }
                    else if (property.propertyType == SerializedPropertyType.Float)
                    {
                        double ret = property.floatValue;
                        ret.SnapCeil(snapAttribute.Value);
                        property.floatValue = (float)ret;
                    }
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
