﻿#if !MEGUMIN_Common

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    [Serializable]
    public class EnableableFrame : Enableable
    {
        [FrameAndTime]
        public int Value;

        //public bool CheckAlive(long startFID, long frameID)
        //{
        //    if (Enabled)
        //    {
        //        return element.Config.CacheVaildFrame.Value + element.WaitStartFID
        //                                    >= frameID
        //    }
        //    return true;
        //}
    }

    [Serializable]
    public class EnableableHDRColor : Enableable
    {
        [ColorUsage(true, true)]
        public Color Value;
    }

    /// <summary>
    /// 可启用的,继承后字段值名字一定要是Value.
    /// 解决Value不能设置特性的问题.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks><see cref="Nullable{T}"/>无法序列化,不能写成特性.</remarks>
    [Serializable]
    [Obsolete("use Enable instead", false)]
    public abstract class Enableable
    {
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("Active")]
        public bool Enabled = true;

        /// <summary>
        /// <inheritdoc cref="Nullable{T}.HasValue"/>
        /// </summary>
        public virtual bool HasValue => Enabled;

        public static implicit operator bool(Enableable activeable)
        {
            return activeable?.Enabled ?? false;
        }
    }

    /// <summary>
    /// 可启用的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <see cref="Nullable{T}"/>无法序列化,不能写成特性.
    [Serializable]
    [Obsolete("use Enable<T> instead", false)]
    public class Enableable<T> : Enableable
    {
        [SerializeField]
        public T Value;

        public Enableable() : this(true, default)
        {

        }

        public Enableable(bool enabled = true, T def = default)
        {
            Enabled = enabled;
            Value = def;
        }

        public static implicit operator T(Enableable<T> activeable)
        {
            return activeable.Value;
        }

        public override bool HasValue
        {
            get
            {
                if (!Enabled)
                {
                    return false;
                }
                else
                {
                    return Value != null;
                }
            }
        }

        //public static implicit operator Nullable<T>(Enableable<T> activeable)
        //    where T:struct
        //{
        //    return default;
        //}
    }

    /// <summary>
    /// 可启用的,继承后字段值名字一定要是Value.
    /// 解决Value不能设置特性的问题.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks><see cref="Nullable{T}"/>无法序列化,不能写成特性.</remarks>
    [Serializable]
    public abstract class Enable
    {
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("Active")]
        public bool Enabled = true;

        /// <summary>
        /// <inheritdoc cref="Nullable{T}.HasValue"/>
        /// </summary>
        public virtual bool HasValue => Enabled;

        public static implicit operator bool(Enable activeable)
        {
            return activeable?.Enabled ?? false;
        }
    }

    /// <summary>
    /// 可启用的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <see cref="Nullable{T}"/>无法序列化,不能写成特性.
    [Serializable]
    public class Enable<T> : Enable
    {
        [SerializeField]
        public T Value;

        public Enable() : this(true, default)
        {

        }

        public Enable(bool enabled = true, T def = default)
        {
            Enabled = enabled;
            Value = def;
        }

        public static implicit operator T(Enable<T> activeable)
        {
            return activeable.Value;
        }

        public override bool HasValue
        {
            get
            {
                if (!Enabled)
                {
                    return false;
                }
                else
                {
                    return Value != null;
                }
            }
        }

        //public static implicit operator Nullable<T>(Enable<T> activeable)
        //    where T:struct
        //{
        //    return default;
        //}
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{
    using global::Megumin;
    using UnityEditor;

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(Enable), true)]
    [CustomPropertyDrawer(typeof(Enable<>), true)]
    [CustomPropertyDrawer(typeof(Enableable), true)]
    [CustomPropertyDrawer(typeof(Enableable<>), true)]
#endif
    internal sealed class EnableableDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var prop = property.FindPropertyRelative("Value");
            if (prop != null)
            {
                return EditorGUI.GetPropertyHeight(prop, label);
            }
            else
            {
                return base.GetPropertyHeight(property, label);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var togglePosition = position;
            togglePosition.x += togglePosition.width - 14;
            togglePosition.width = 16;

            var valuePosition = position;
            valuePosition.width -= 20;
            SerializedProperty toggle = property.FindPropertyRelative("Enabled");
            EditorGUI.BeginProperty(valuePosition, label, toggle);
            toggle.boolValue = GUI.Toggle(togglePosition, toggle.boolValue, GUIContent.none);
            EditorGUI.EndProperty();

            SerializedProperty value = property.FindPropertyRelative("Value");
            if (value != null)
            {
                EditorGUI.BeginProperty(valuePosition, label, value);
                EditorGUI.BeginDisabledGroup(!toggle.boolValue);
                EditorGUI.PropertyField(valuePosition, value, label, true);
                EditorGUI.EndDisabledGroup();
                EditorGUI.EndProperty();
            }
        }
    }
}

#endif

#endif
