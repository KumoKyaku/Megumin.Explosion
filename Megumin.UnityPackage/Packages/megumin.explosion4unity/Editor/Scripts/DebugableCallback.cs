﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEditor.ShortcutManagement;
using System;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Megumin
{
    [InitializeOnLoad]
    public class DebugableCallback
    {
        //static bool Enable = false;

        [InitializeOnLoadMethod]
        public static void Register()
        {
            //Debug.Log("注册");


            //#if ENABLE_INPUT_SYSTEM
            //            InputSystem.onEvent -= InputSystem_onEvent;
            //            InputSystem.onEvent += InputSystem_onEvent;
            //#endif

            //Enable = true;
        }

#if ENABLE_INPUT_SYSTEM

        private unsafe static void InputSystem_onEvent(UnityEngine.InputSystem.LowLevel.InputEventPtr arg1,
                                            InputDevice arg2)
        {
            if (Selection.activeGameObject)
            {
                if (arg2 is Keyboard keyboard)
                {
                    var res = keyboard.ReadValueFromEventAsObject(arg1);
                    var res2 = keyboard.f1Key.ReadValueFromEventAsObject(arg1);
                    //if (keyboard.f1Key.wasPressedThisFrame)
                    if (res2 is float a && a == 1)
                    {
                        OnF1();
                    }
                }
            }
        }

#endif

        public static void Reflection(object item, MemberInfo m)
        {
            if (m is FieldInfo field)
            {
                //控制台Log字段
                var fv = field.GetValue(item);
                Debug.Log($"FieldName  :  {field.Name}    {fv.ToStringReflection(true)}");
            }
            else if (m is PropertyInfo property)
            {
                //控制台Log属性
                var pv = property.GetValue(item);
                Debug.Log($"PropertyName  :  {property.Name}    {pv.ToStringReflection(true)}");
            }
            else if (m is MethodInfo method)
            {
                //反射调用方法
                method.Invoke(item, null);
            }
        }

        static readonly BindingFlags CheckFlag
            = BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.Static;

        public static void ObjectOnF1(object item)
        {
            if (item == null)
            {
                return;
            }

            if (item is IF1able f1Able)
            {
                f1Able?.OnF1();
            }

            {
                //控制台Log对象
                var hasFlag = item.GetType().GetCustomAttributes(typeof(OnF1Attribute), false).Length > 0;
                if (hasFlag)
                {
                    Debug.Log(item.ToStringReflection(true));
                }
            }

            {
                var members = from m in item.GetType().GetMembers(CheckFlag)
                              let attrs = m.GetCustomAttributes(typeof(OnF1Attribute), true)
                              where attrs.Length > 0
                              select m;

                foreach (var m in members)
                {
                    Reflection(item, m);
                }
            }
        }

        public static void ObjectOnKey(object item, ConsoleKey key)
        {
            if (item == null)
            {
                return;
            }

            {
                //控制台Log对象
                var attris = item.GetType().GetCustomAttributes(typeof(OnKeyAttribute), false);
                if (attris.Any(a => a is OnKeyAttribute ok && ok.Key == key))
                {
                    Debug.Log(item.ToStringReflection(true));
                }
            }

            {
                var members = from m in item.GetType().GetMembers(CheckFlag)
                              let attrs = m.GetCustomAttributes(typeof(OnKeyAttribute), true)
                              where attrs.Length > 0
                              select (m, attrs);

                foreach (var kv in members)
                {
                    var m = kv.m;
                    if (kv.attrs.Any(a => a is OnKeyAttribute ok && ok.Key == key))
                    {
                        Reflection(item, m);
                    }
                }
            }
        }

        public static void OnKey(ConsoleKey key)
        {
            if (Selection.activeGameObject)
            {
                var comps = Selection.activeGameObject.GetComponents<MonoBehaviour>();
                foreach (var item in comps)
                {
                    ObjectOnKey(item, key);
                }
            }

            if (Selection.activeObject is ScriptableObject obj)
            {
                ObjectOnKey(obj, key);
            }
        }

        [Shortcut("MeguminDebug/" + nameof(OnF1), KeyCode.F1)]
        public static void OnF1()
        {
            if (Selection.activeGameObject)
            {
                var comps = Selection.activeGameObject.GetComponents<MonoBehaviour>();
                foreach (var item in comps)
                {
                    ObjectOnF1(item);
                }
            }

            if (Selection.activeObject is ScriptableObject obj)
            {
                ObjectOnF1(obj);
            }

            OnKey(ConsoleKey.F1);
        }

        [Shortcut("MeguminDebug/" + nameof(OnF3), KeyCode.F3)]
        public static void OnF3()
        {
            OnKey(ConsoleKey.F3);
        }

        [Shortcut("MeguminDebug/" + nameof(OnF4), KeyCode.F4)]
        public static void OnF4()
        {
            OnKey(ConsoleKey.F4);
        }

        [Shortcut("MeguminDebug/" + nameof(OnF5), KeyCode.F5)]
        public static void OnF5()
        {
            OnKey(ConsoleKey.F5);
        }

        [Shortcut("MeguminDebug/" + nameof(OnF6), KeyCode.F6)]
        public static void OnF6()
        {
            OnKey(ConsoleKey.F6);
        }

        [Shortcut("MeguminDebug/" + nameof(OnF7), KeyCode.F7)]
        public static void OnF7()
        {
            OnKey(ConsoleKey.F7);
        }

        [Shortcut("MeguminDebug/" + nameof(OnF8), KeyCode.F8)]
        public static void OnF8()
        {
            OnKey(ConsoleKey.F8);
        }

        //[Shortcut("MeguminDebug/" + nameof(OnBackQuote), KeyCode.BackQuote)]
        //public static void OnBackQuote()
        //{
        //    OnKey(ConsoleKey.Oem3);
        //}

        //[Shortcut("MeguminDebug/" + nameof(OnApplications), KeyCode.RightApple)]
        //public static void OnApplications()
        //{
        //    OnKey(ConsoleKey.Applications);
        //}
    }

}
