using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;
using System.IO;

namespace Megumin
{
    public class PathAttribute : PropertyAttribute
    {
        public bool RelativePath { get; set; } = true;
        public bool IsFolder { get; set; } = true;
        public string Exetension;
    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using UnityEditor;
    using static UnityEditor.EditorGUI;

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

            PathAttribute myattribute = attribute as PathAttribute;

            if (m_Path != null)
            {
                if (myattribute.RelativePath)
                {
                    if (m_Path.Contains(":/"))
                    {
                        //转换为相对路径。
                        var p1 = new System.Uri(m_Path);
                        var p2 = new System.Uri(MeguminUtility4Unity.ProjectPath);
                        var r = p2.MakeRelativeUri(p1);
                        m_Path = r.ToString();
                    }
                }

                property.stringValue = m_Path;
                m_Path = null;
            }

            var obj = AssetDatabase.LoadAssetAtPath(property.stringValue, typeof(UnityEngine.Object));
            if (obj)
            {
                EditorGUI.BeginProperty(position, label, property);
                float width = propertyPosition.width;
                propertyPosition.width = EditorGUIUtility.labelWidth;
                EditorGUI.LabelField(propertyPosition, label);
                propertyPosition.x += propertyPosition.width;
                propertyPosition.width = (width - propertyPosition.width);

                using (new DisabledScope(true))
                {
                    EditorGUI.ObjectField(propertyPosition, obj, typeof(UnityEngine.Object), false);
                }
                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(propertyPosition, property, label);
            }

            if (GUI.Button(buttonPosition, "Select"))
            {
                string dir = Path.Combine(MeguminUtility4Unity.ProjectPath, property.stringValue);
                dir = Path.GetDirectoryName(dir);
                dir = Path.GetFullPath(dir);

                if (myattribute.IsFolder)
                {
                    var path = EditorUtility.OpenFolderPanel("选择文件夹", dir, "");
                    m_Path = path;
                    //property.stringValue = path;
                    GUIUtility.ExitGUI();
                }
                else
                {
                    var path = EditorUtility.OpenFilePanel("选择文件", dir, myattribute.Exetension);
                    m_Path = path;
                    //property.stringValue = path;
                    GUIUtility.ExitGUI();
                }
            }
        }
    }
}

#endif


