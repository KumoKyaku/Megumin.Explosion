﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

namespace Megumin
{
    /// <summary>
    /// 安全序列化枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public struct SEnum<T> : ISerializationCallbackReceiver
        where T : struct, Enum
    {
        public string StrValue;
        public T Value;
        public SEnum(T def)
        {
            Value = def;
            StrValue = def.ToString();
        }

        public static implicit operator SEnum<T>(T v)
        {
            return new SEnum<T>(v);
        }

        public static implicit operator T(SEnum<T> sv)
        {
            return sv.Value;
        }

        public void OnBeforeSerialize()
        {
            if (string.IsNullOrEmpty(StrValue))
            {
                StrValue = Value.ToString();
            }
        }

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(StrValue))
            {
                StrValue = Value.ToString();
            }

            if (Enum.TryParse<T>(StrValue, out var temp))
            {
                if (StrValue != Value.ToString())
                {
                    Debug.LogWarning($"枚举值类型变化，保存的枚举值可能与之前不一致。Type:{typeof(T).Name}  StrValue:{StrValue} != Value:{Value}");

                    //不行，编辑器报错
                    //if (Application.isPlaying)
                    //{
                    //    //运行时以Str为准。
                    //    Value = temp;
                    //}
                }
            }
            else
            {
                Debug.LogWarning($"枚举值类型变化，保存的string：[{StrValue}]无法解析成枚举:[{typeof(T)}]");
            }
        }
    }

}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using global::Megumin;
    using UnityEditor;

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(SEnum<>), true)]
#endif
    internal sealed class SEnumDrawer : PropertyDrawer
    {
        static readonly Color warning = new Color(1, 0.7568f, 0.0275f, 1);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty e = property.FindPropertyRelative("Value");
            SerializedProperty se = property.FindPropertyRelative("StrValue");

            var t = fieldInfo.FieldType;
            var enumType = t.GetGenericArguments()[0];

            var canParseStr = Enum.TryParse(enumType, se.stringValue, true, out var enumValue);
            var valueParseStr = Enum.ToObject(enumType, e.longValue).ToString();

            if (canParseStr && valueParseStr == se.stringValue)
            {
                EditorGUI.PropertyField(position, e, label);
                var name = Enum.ToObject(enumType, e.longValue).ToString();
                se.stringValue = name;
            }
            else
            {
                var propertyPosition = position;
                propertyPosition.width -= 100;

                var togglePosition = position;
                togglePosition.x += togglePosition.width - 18;
                togglePosition.width = 18;

                var timePosition = position;
                timePosition.x += position.width - 100;
                timePosition.width = 80;

                using ((ValueGUIColor)warning)
                {
                    EditorGUI.PropertyField(propertyPosition, e, label);
                }

                var str = EditorGUI.TextField(timePosition, se.stringValue);
                se.stringValue = str;


                if (GUI.Button(togglePosition, "S"))
                {
                    if (canParseStr)
                    {
                        var longv = Convert.ToInt64(enumValue);
                        e.longValue = longv;
                    }
                    else
                    {
                        se.stringValue = valueParseStr;
                    }
                }
            }
        }
    }
}

#endif





