﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class PathAttribute : PropertyAttribute
    {
        public bool IsFolder { get; set; } = true;
        public string Exetension;
    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(PathAttribute))]
    internal sealed class PathAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyPosition = position;
            propertyPosition.width -= 86;

            var buttonPosition = position;
            buttonPosition.width = 80;
            buttonPosition.x += position.width - 80;

            PathAttribute myattribute = attribute as PathAttribute;
            EditorGUI.PropertyField(propertyPosition, property, label);
            if (GUI.Button(buttonPosition, "Select"))
            {
                if (myattribute.IsFolder)
                {
                    var path = EditorUtility.OpenFolderPanel("选择文件夹", "", "");
                    property.stringValue = path;
                }
                else
                {
                    var path = EditorUtility.OpenFilePanel("选择文件", "", myattribute.Exetension);
                    property.stringValue = path;
                }
            }
        }
    }
}

#endif


