using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScriptableObject),true)]
public class ScriptObjectDrawer_8F11D385 : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var propertyPosition = position;
        propertyPosition.width -= 76;

        var buttonPosition = position;
        buttonPosition.width = 70;
        buttonPosition.x += position.width - 70;

        EditorGUI.PropertyField(propertyPosition, property, label);
        if (GUI.Button(buttonPosition, "Show"))
        {
            var obj = property.objectReferenceValue;
            if (obj)
            {
                Debug.Log($"Open By {nameof(ScriptObjectDrawer_8F11D385)}");
                obj.OpenProperty();
            }
        }
    }
}
