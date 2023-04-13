using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Megumin;

namespace Megumin
{
    [Serializable]
    public class TagFilter
    {
        [Flags]
        public enum TestMode
        {
            Equal = 1 << 0,
            Ignore = 1 << 1,
        }

        public TestMode Mode = 0;

        [Tag]
        public string EqualTag = "Untagged";

        [Tag]
        public List<string> IgnoreTag = new List<string>();

        public bool Check(Component component)
        {
            if ((Mode & TestMode.Equal) != 0)
            {
                if (!component.CompareTag(EqualTag))
                {
                    return false;
                }
            }

            if ((Mode & TestMode.Ignore) != 0)
            {
                foreach (var item in IgnoreTag)
                {
                    if (component.CompareTag(item))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    /// <summary>
    /// 太费事，不写了
    /// </summary>
#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    //[CustomPropertyDrawer(typeof(TagFilter))]
#endif
    internal sealed class TagFilterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var amountRect = new Rect(position.x, position.y, 30, position.height);
            var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
            var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

            var propMode = property.FindPropertyRelative("Mode");
            var propEqualTag = property.FindPropertyRelative("EqualTag");
            var propIgnoreTag = property.FindPropertyRelative("IgnoreTag");
            EditorGUI.PropertyField(position, propMode, true);


            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}

#endif

