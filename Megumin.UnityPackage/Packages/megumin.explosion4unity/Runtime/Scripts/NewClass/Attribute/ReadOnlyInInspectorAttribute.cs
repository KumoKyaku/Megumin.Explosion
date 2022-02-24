using Megumin;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 让字段在编辑器中只读，防止被错误的修改
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public sealed class ReadOnlyInInspectorAttribute : PropertyAttribute
    {

    }

#if UNITY_EDITOR

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyInInspectorAttribute))]
#endif
    public class LockedInInspectorAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
        {
            return UnityEditor.EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            using (new UnityEditor.EditorGUI.DisabledGroupScope(true))
            {
                UnityEditor.EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }

#endif
}
