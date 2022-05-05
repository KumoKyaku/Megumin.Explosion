using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

namespace Megumin
{
    public class LayerAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(LayerAttribute))]
#endif
    internal sealed class LayerAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();
            if (property.propertyType == SerializedPropertyType.String)
            {
                if (string.IsNullOrEmpty(property.stringValue))
                {
                    property.stringValue = "Default";
                }
                EditorGUI.BeginChangeCheck();
                var newlayer = EditorGUI.LayerField(position, label, LayerMask.NameToLayer(property.stringValue));
                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = LayerMask.LayerToName(newlayer);
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                var oldLayer = property.longValue;
                if (oldLayer < 0 || oldLayer > 31)
                {
                    oldLayer = 0;
                }

                EditorGUI.BeginChangeCheck();
                var newlayer = EditorGUI.LayerField(position, label, (int)oldLayer);
                if (EditorGUI.EndChangeCheck())
                {
                    property.longValue = newlayer;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                this.NotMatch(position, property, label, "字段类型必须是String 或 Int");
            }
        }
    }
}

#endif



