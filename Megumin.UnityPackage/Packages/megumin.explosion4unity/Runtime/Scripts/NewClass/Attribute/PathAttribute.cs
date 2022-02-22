using System.Collections;
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

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(PathAttribute))]
#endif
    internal sealed class PathAttributeDrawer : PropertyDrawer
    {
        string m_Path = null;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyPosition = position;
            propertyPosition.width -= 86;

            var buttonPosition = position;
            buttonPosition.width = 80;
            buttonPosition.x += position.width - 80;

            if (m_Path != null)
            {
                property.stringValue = m_Path;
                m_Path = null;
            }

            PathAttribute myattribute = attribute as PathAttribute;
            EditorGUI.PropertyField(propertyPosition, property, label);
            if (GUI.Button(buttonPosition, "Select"))
            {
                if (myattribute.IsFolder)
                {
                    var path = EditorUtility.OpenFolderPanel("选择文件夹", "", "");
                    m_Path = path;
                    //property.stringValue = path;
                    GUIUtility.ExitGUI();
                }
                else
                {
                    var path = EditorUtility.OpenFilePanel("选择文件", "", myattribute.Exetension);
                    m_Path = path;
                    //property.stringValue = path;
                    GUIUtility.ExitGUI();
                }
            }
        }
    }
}

#endif


