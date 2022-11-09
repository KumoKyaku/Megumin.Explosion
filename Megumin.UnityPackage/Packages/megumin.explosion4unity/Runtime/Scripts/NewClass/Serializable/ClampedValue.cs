using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    [Serializable]
    public class ClampedValue<T>
    {
        public T Min;
        public T Max;
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using global::Megumin;
    using UnityEditor;
    using UnityEngine.UIElements;

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(ClampedValue<>), true)]
#endif
    internal sealed class ClampedValueDrawer : PropertyDrawer
    {
        ///// <summary>
        ///// 如果在基于 UIElements 的检查器或 EditorWindow 中使用 PropertyDrawer，则在 PropertyDrawer.CreatePropertyGUI 被任何 IMGUI 实现的回退覆盖时，将使用 UIElements 实现。如果在基于 IMGUI 的检查器或 EditorWindow 中使用 PropertyDrawer，则只会显示 IMGUI 实现。您不能让 UIElements 在 IMGUI 中运行。
        ///// <para>https://docs.unity3d.com/2022.2/Documentation/ScriptReference/PropertyDrawer.html</para>
        ///// </summary>
        ///// <param name="property"></param>
        ///// <returns></returns>
        //public override VisualElement CreatePropertyGUI(SerializedProperty property)
        //{
        //    var root = new VisualElement();
        //    root.Add(new IntegerField("MinTest"));
        //    root.Add(new IntegerField("MaxTest"));
        //    return root;
        //}

        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        //{
        //    var prop = property.FindPropertyRelative("MyOverrideValue");
        //    return EditorGUI.GetPropertyHeight(prop);
        //}

        //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        //{
        //    var togglePosition = position;
        //    togglePosition.x += togglePosition.width - 14;
        //    togglePosition.width = 16;

        //    var valuePosition = position;
        //    valuePosition.width -= 20;
        //    //valuePosition.x += 20;

        //    SerializedProperty toggle = property.FindPropertyRelative("IsOverride");
        //    //EditorGUI.PropertyField(togglePosition, toggle, GUIContent.none);
        //    toggle.boolValue = GUI.Toggle(togglePosition, toggle.boolValue, GUIContent.none);

        //    SerializedProperty myOverrideValue = property.FindPropertyRelative("MyOverrideValue");
        //    SerializedProperty min = property.FindPropertyRelative("Min");
        //    SerializedProperty max = property.FindPropertyRelative("Max");

        //    if (toggle.boolValue)
        //    {
        //        EditorGUI.PropertyField(valuePosition, myOverrideValue, label, true);
        //    }
        //    else
        //    {
        //        using (new EditorGUI.DisabledGroupScope(true))
        //        {
        //            label.text += " [Default]";
        //            EditorGUI.PropertyField(valuePosition, myOverrideValue, label, true);
        //        }
        //    }
        //}
    
    }
}

#endif
