namespace UnityEngine
{
    /// <summary>
    /// 缩进
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public sealed class IndentAttribute : PropertyAttribute
    {
        public IndentAttribute(int increment = 1)
        {
            this.Increment = increment;
        }

        public int Increment { get; set; } = 1;
    }

#if UNITY_EDITOR

    [UnityEditor.CustomPropertyDrawer(typeof(IndentAttribute))]
    public class IndentAttributeAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
        {
            return UnityEditor.EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            using (new UnityEditor.EditorGUI.IndentLevelScope((attribute as IndentAttribute).Increment))
            {
                UnityEditor.EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }

#endif
}












