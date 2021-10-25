using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Megumin
{
    [CustomPropertyDrawer(typeof(iint), true)]
    public class iintDrawer : PropertyDrawer
    {
        static GUIStyle left = new GUIStyle("minibuttonleft");
        static GUIStyle right = new GUIStyle("minibuttonright");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyPosition = position;
            propertyPosition.width -= 86;

            var buttonPosition = position;
            buttonPosition.width = 80;
            buttonPosition.x += position.width - 80;

            var p = property.FindPropertyRelative("_int");
            EditorGUI.PropertyField(propertyPosition, p, label);


            var leftPosotion = buttonPosition;
            leftPosotion.width = 40;
            var rightPosition = buttonPosition;
            rightPosition.width = 40;
            rightPosition.x += 40;

            iint value = p.intValue;

            using (new EditorGUI.DisabledScope(value.IsNegativeInfinity))
            {
                if (GUI.Button(leftPosotion, "- ∞", left))
                {
                    p.intValue = iint.NegativeInfinity;
                }
            }

            using (new EditorGUI.DisabledScope(value.IsPositiveInfinity))
            {
                if (GUI.Button(rightPosition, "+ ∞", right))
                {
                    p.intValue = iint.PositiveInfinity;
                }
            }
        }
    }

}

