namespace UnityEngine
{
    /// <summary>
    /// 让字段在编辑器中只读，防止被错误的修改
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public sealed class ReadOnlyInInspectorAttribute : PropertyAttribute
    {

    }

#if UNITY_EDITOR

    [UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyInInspectorAttribute))]
    public class LockedInInspectorAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            using (new UnityEditor.EditorGUI.DisabledGroupScope(true))
            {
                UnityEditor.EditorGUI.PropertyField(position, property, label);
            }
        }
    }

#endif
}
