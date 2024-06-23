using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

namespace Megumin
{
    /// <summary>
    /// int正负没有意义,这是一个按位标记.
    /// </summary>
    public class AreaMaskAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(AreaMaskAttribute))]
#endif
    internal sealed class AreaMaskDrawer : PropertyDrawer
    {
        /// <summary>
        /// 从 <see cref="NavMeshAgentInspector"/> 复制. 修改了int溢出bug.
        /// <see cref="NavMeshComponentsGUIUtility.AreaPopup"/> 不能多选.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                var propertyPosition = position;
                propertyPosition.width -= 84;

                var buttonPosition = position;
                buttonPosition.width = 80;
                buttonPosition.x += position.width - 80;

                //Initially needed data

#if UNITY_6000_0_OR_NEWER
                var areaNames = UnityEngine.AI.NavMesh.GetAreaNames();
#else
                var areaNames = GameObjectUtility.GetNavMeshAreaNames();
#endif

                var currentMask = property.longValue;
                var compressedMask = 0;

                if ((currentMask & 0xffffffff) == 0xffffffff)
                {
                    compressedMask = ~0;
                }
                else
                {
                    //Need to find the index as the list of names will compress out empty areas
                    for (var i = 0; i < areaNames.Length; i++)
                    {

#if UNITY_6000_0_OR_NEWER
                        var areaIndex = UnityEngine.AI.NavMesh.GetAreaFromName(areaNames[i]);
#else
                        var areaIndex = GameObjectUtility.GetNavMeshAreaFromName(areaNames[i]);
#endif

                        if (((1 << areaIndex) & currentMask) != 0)
                            compressedMask = compressedMask | (1 << i);
                    }
                }

                EditorGUI.BeginProperty(propertyPosition, GUIContent.none, property);

                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
                var areaMask = EditorGUI.MaskField(propertyPosition, label, compressedMask, areaNames, EditorStyles.layerMaskField);
                EditorGUI.showMixedValue = false;

                if (EditorGUI.EndChangeCheck())
                {
                    if (areaMask == ~0)
                    {
                        property.longValue = ~0L;
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
                                newMask = newMask | (uint)(1 << UnityEngine.AI.NavMesh.GetAreaFromName(areaNames[i]));
                            }
                        }

                        ///修复int类型可能的溢出bug,最高位为1时.
                        property.longValue = (int)newMask;
                    }
                }
                EditorGUI.EndProperty();

                if (GUI.Button(buttonPosition, "Settings"))
                {
                    UnityEditor.AI.NavMeshEditorHelpers.OpenAreaSettings();
                }
            }
            else
            {
                this.NotMatch(position, property, label, "字段类型必须是Int");
            }
        }
    }
}

#endif

