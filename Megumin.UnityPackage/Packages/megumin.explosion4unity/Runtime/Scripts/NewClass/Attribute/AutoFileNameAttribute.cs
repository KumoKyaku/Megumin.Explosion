using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

namespace Megumin
{
    public class AutoFileNameAttribute : PropertyAttribute
    {
    }

}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using UnityEditor;

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(AutoFileNameAttribute))]
#endif
    internal sealed class AutoFileNameAttributeDrawer : PropertyDrawer
    {
        
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
                this.NotMatch(position, property, label);
            }
        }
    }
}

#endif



