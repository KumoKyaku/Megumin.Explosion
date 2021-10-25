using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace UnityEngine
{
    /// <summary>
    /// 很多情况不工作,处理不了UndoRedo,没有Odin的好用.
    /// </summary>
    [Obsolete]
    internal class OnValueChangedAttribute : PropertyAttribute
    {
        public string CallBackName;
    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using System;
    using UnityEditor;

    [Obsolete]
    [CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
    internal sealed class OnValueChangedAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label, true);
            var isChanged = EditorGUI.EndChangeCheck();

            if (isChanged)
            {
                property.serializedObject.ApplyModifiedProperties();
                OnValueChangedAttribute a = attribute as OnValueChangedAttribute;
                var method = property.serializedObject.targetObject.GetType().GetMethod(a.CallBackName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                method?.Invoke(property.serializedObject.targetObject, null);
                //Debug.LogError($"{a.CallBackName}--");
            }
        }
    }
}

#endif


