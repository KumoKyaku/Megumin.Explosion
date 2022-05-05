using Megumin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    [Serializable]
    public class TagMask
    {
        public static bool InspectorShowTagList = false;

        [Tag]
        public List<string> Tags = new List<string>() { "Untagged" };

        public TagMask()
        {
        }

        public TagMask(params string[] tags)
        {
            Tags.Clear();
            Tags.AddRange(tags);
        }

        public bool HasFlag(string tag)
        {
            return Tags.Contains(tag);
        }

        /// <summary>
        /// 不知道调用CompareTag和 Tags.Contains 哪个更省一些？
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool HasFlag(GameObject gameObject)
        {
            foreach (var item in Tags)
            {
                if (gameObject.CompareTag(item))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasFlag(Component component)
        {
            foreach (var item in Tags)
            {
                if (component.CompareTag(item))
                {
                    return true;
                }
            }

            return false;
        }
    }

}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using UnityEditorInternal;

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(TagMask))]
#endif
    internal sealed class TagMaskDrawer : PropertyDrawer
    {
        List<string> mytags = new List<string>();
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (TagMask.InspectorShowTagList)
            {
                return EditorGUI.GetPropertyHeight(property, true);
            }
            return 18;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var prop = property.FindPropertyRelative("Tags");
            mytags.Clear();
            for (int i = 0; i < prop.arraySize; i++)
            {
                mytags.Add(prop.GetArrayElementAtIndex(i).stringValue);
            }

            string[] tags = InternalEditorUtility.tags;
            var compressedMask = 0;
            for (int i = 0; i < tags.Length; i++)
            {
                if (mytags.Contains(tags[i]))
                {
                    compressedMask |= (1 << i);
                }
            }

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            compressedMask = EditorGUI.MaskField(position, label, compressedMask, tags);
            if (EditorGUI.EndChangeCheck())
            {
                mytags.Clear();
                if (compressedMask == ~0)
                {
                    //everything
                    mytags.AddRange(tags);
                }
                else
                {
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if ((compressedMask & (1 << i)) != 0)
                        {
                            mytags.Add((string)tags[i]);
                        }
                    }
                }

                prop.arraySize = mytags.Count;
                prop.serializedObject.ApplyModifiedProperties();
                for (int i = 0; i < mytags.Count; i++)
                {
                    prop.GetArrayElementAtIndex(i).stringValue = mytags[i];
                }
            }
            EditorGUI.EndProperty();

            if (TagMask.InspectorShowTagList)
            {
                using (new EditorGUI.IndentLevelScope(2))
                {
                    var listposition = position;
                    listposition.y += 20;
                    EditorGUI.PropertyField(listposition, prop, label, true);
                }
            }
        }
    }
}

#endif

