using System;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 使用做闭右开区间可以大大减小逻辑复杂度
    /// 和<see cref="EnableableAttribute"/>一起使用时不要继承Scope,因为同一时刻只能有PropertyDrawer生效.
    /// </summary>
    [Serializable]
    public class Scope
    {
        public bool IsDurationMode = false;

        [Tooltip("闭区间")]
        [FrameAndTime]
        public int Start;

        [FrameAndTime]
        public int Duration;

        [Tooltip("开区间")]
        [FrameAndTime]
        public int End;

        public bool InScope(int current)
        {
            if (current >= Start)
            {
                if (current < End)
                {
                    return true;
                }
            }

            return false;
        }

        public bool InScope(long current)
        {
            if (current >= Start)
            {
                if (current < End)
                {
                    return true;
                }
            }

            return false;
        }

        public void Scale(double scale)
        {
            Start = (int)(Start * scale);
            if (IsDurationMode)
            {
                Duration = (int)(Duration * scale + 0.5f);
                End = Start + Duration;
            }
            else
            {
                End = (int)(End * scale);
                Duration = End - Start;
            }
        }
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
            return base.GetPropertyHeight(property, label) * 4 + 6;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var startP = property.FindPropertyRelative("Start");
            var durationP = property.FindPropertyRelative("Duration");
            var endp = property.FindPropertyRelative("End");
            var isDurationModeP = property.FindPropertyRelative("IsDurationMode");

            //是不是持续时间模式
            var isdumode = isDurationModeP.boolValue;
            var oldstart = startP.intValue;
            var olddu = durationP.intValue;
            var oldend = endp.intValue;

            var rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, isDurationModeP);
            rect.y += 20;
            EditorGUI.PropertyField(rect, startP);

            rect.y += 20;
            using (new EditorGUI.DisabledScope(!isdumode))
            {
                EditorGUI.PropertyField(rect, durationP);
            }

            rect.y += 20;
            using (new EditorGUI.DisabledScope(isdumode))
            {
                EditorGUI.PropertyField(rect, endp);
            }

            var newstart = startP.intValue;
            if (isdumode)
            {
                endp.intValue = oldend;

                var newdu = durationP.intValue;
                if (oldstart != newstart
                    || olddu != newdu)
                {
                    endp.intValue = newstart + newdu;
                }
            }
            else
            {
                durationP.intValue = olddu;

                var newend = endp.intValue;
                if (oldstart != newstart
                    || oldend != newend)
                {
                    durationP.intValue = newend - newstart;
                }
            }
        }
    }
}

#endif


