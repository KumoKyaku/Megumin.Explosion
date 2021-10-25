using UnityEngine;

namespace UnityEngine
{
    /// <summary>
    /// 简单设置在面版中的样式
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Method)]
    public sealed class InspectorStyleAttribute : PropertyAttribute
    {
        public bool ReadOnly { get; set; } = false;
        /// <summary>
        /// #00FF00FF 必须#开头
        /// </summary>
        public string Color { get; set; }
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{

    [CustomPropertyDrawer(typeof(InspectorStyleAttribute))]
    public class InspectorStyleAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InspectorStyleAttribute style = attribute as InspectorStyleAttribute;

            EditorGUI.BeginDisabledGroup(style.ReadOnly);
            var oldColor = GUI.color;
            if (ColorUtility.TryParseHtmlString(style.Color, out var color))
            {
                GUI.color = color;
            }

            EditorGUI.PropertyField(position, property, label);
            GUI.color = oldColor;
            EditorGUI.EndDisabledGroup();
        }
    }
}

#endif




