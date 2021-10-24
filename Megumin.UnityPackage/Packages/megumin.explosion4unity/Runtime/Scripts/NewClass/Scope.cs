using System;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 使用做闭右开区间可以大大减小逻辑复杂度
    /// </summary>
    [Serializable]
    public class Scope
    {
        [UnityEngine.Serialization.FormerlySerializedAs("Active")]
        public bool Enabled = true;
        public bool IsDurationMode = false;

        [Space]
        [Tooltip("闭区间")]
        [FrameAndTime]
        public int Start;

        [FrameAndTime]
        public int Duration;

        [Tooltip("开区间")]
        [FrameAndTime]
        public int End;
    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using global::Megumin;
    using UnityEditor;

    /// <summary>
    /// 使用<see cref="OnValueChangedAttribute"/>没有成功.Odin的OnvalueChanged,可以的但是Odin太卡.
    /// </summary>
    [CustomPropertyDrawer(typeof(Scope), true)]
    internal sealed class ScopeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var s = property.FindPropertyRelative("Start");
            var du = property.FindPropertyRelative("Duration");
            var end = property.FindPropertyRelative("End");

            var isdu = property.FindPropertyRelative("IsDurationMode");
            var isdumode = isdu.boolValue;
            var olddu = du.intValue;
            var oldend = end.intValue;

            EditorGUI.PropertyField(position, property, label, true);
            var start = s.intValue;

            if (isdumode)
            {
                var newdu = du.intValue;
                if (olddu != newdu)
                {
                    //Debug.LogError($"old:{olddu}--new:{newdu}");
                    end.intValue = start + newdu;
                }
                else
                {
                    end.intValue = oldend;
                }
            }
            else
            {
                var newend = end.intValue;
                if (oldend != newend)
                {
                    //Debug.LogError($"old:{oldend}--new:{newend}");
                    du.intValue = newend - start;
                }
                else
                {
                    du.intValue = olddu;
                }
            }
        }
    }
}

#endif


