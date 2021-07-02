using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    /// <summary>
    /// every = int.max 第32位是符号位，有bug不能用
    /// </summary>
    public class AreaMaskAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    [CustomPropertyDrawer(typeof(AreaMaskAttribute))]
    internal sealed class AreaMaskDrawer : PropertyDrawer
    {
        static string[] buildin = new string[31]
        {
            "Build-in 0",
            "Build-in 1",
            "Build-in 2",
            "User 3",
            "User 4",
            "User 5",
            "User 6",
            "User 7",
            "User 8",
            "User 9",
            "User 10",
            "User 11",
            "User 12",
            "User 13",
            "User 14",
            "User 15",
            "User 16",
            "User 17",
            "User 18",
            "User 19",
            "User 20",
            "User 21",
            "User 22",
            "User 23",
            "User 24",
            "User 25",
            "User 26",
            "User 27",
            "User 28",
            "User 29",
            "User 30",
        };

        public bool inputmode = false;
        static readonly Color warning = new Color(1, 0.7568f, 0.0275f, 1);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                var areaNames = GameObjectUtility.GetNavMeshAreaNames();
                var currentMask = property.intValue;
                var compressedMask = 0;
                for (var i = 0; i < areaNames.Length; i++)
                {
                    var areaIndex = GameObjectUtility.GetNavMeshAreaFromName(areaNames[i]);
                    if (((1 << areaIndex) & currentMask) != 0)
                        compressedMask = compressedMask | (1 << i);
                }

                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
                var areaMask = EditorGUI.MaskField(position, label, compressedMask, areaNames, EditorStyles.layerMaskField);
                EditorGUI.showMixedValue = false;

                if (EditorGUI.EndChangeCheck())
                {
                    if (areaMask == ~0)
                    {
                        property.intValue = -1;
                    }
                    else
                    {
                        uint newMask = 0;
                        for (var i = 0; i < areaNames.Length; i++)
                        {
                            //If the bit has been set in the compacted mask
                            if (((areaMask >> i) & 1) != 0)
                            {
                                //Find out the 'real' layer from the name, then set it in the new mask
                                newMask = newMask | (uint)(1 << GameObjectUtility.GetNavMeshAreaFromName(areaNames[i]));
                            }
                        }
                        property.intValue = (int)newMask;
                    }
                }
            }
            else
            {
                //EditorGUI.HelpBox(position, $"{label.text} 字段类型必须是string", MessageType.Error);
                label.tooltip += $"{nameof(AreaMaskAttribute)}失效！\n{label.text} 字段类型必须是int";
                label.text = $"??? " + label.text;
                var old = GUI.color;
                GUI.color = warning;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = old;
            }
        }
    }
}

#endif

