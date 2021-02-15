﻿using System;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using Object = UnityEngine.Object;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

//从 https://github.com/miguel12345/EditorButton 修改
//支持ScrpitObject

[System.AttributeUsage(System.AttributeTargets.Method)]
public class EditorButtonAttribute : PropertyAttribute
{
    public readonly string buttonName;

    public EditorButtonAttribute(string name = null)
    {
        buttonName = name;
    }
}

///Editor

#if UNITY_EDITOR

[CustomEditor(typeof(Object), true)]
public class EditorButton : Editor
{
    class EditorButtonState
    {
        public bool opened;
        public System.Object[] parameters;
        public EditorButtonState(int numberOfParameters)
        {
            parameters = new System.Object[numberOfParameters];
        }
    }

    EditorButtonState[] editorButtonStates;

    delegate object ParameterDrawer(ParameterInfo parameter, object val);

    Dictionary<Type, ParameterDrawer> typeDrawer = new Dictionary<Type, ParameterDrawer> {

        {typeof(float),DrawFloatParameter},
        {typeof(int),DrawIntParameter},
        {typeof(string),DrawStringParameter},
        {typeof(bool),DrawBoolParameter},
        {typeof(Color),DrawColorParameter},
        {typeof(Vector3),DrawVector3Parameter},
        {typeof(Vector2),DrawVector2Parameter},
        {typeof(Quaternion),DrawQuaternionParameter}
    };

    Dictionary<Type, string> typeDisplayName = new Dictionary<Type, string> {

        {typeof(float),"float"},
        {typeof(int),"int"},
        {typeof(string),"string"},
        {typeof(bool),"bool"},
        {typeof(Color),"Color"},
        {typeof(Vector3),"Vector3"},
        {typeof(Vector2),"Vector2"},
        {typeof(Quaternion),"Quaternion"}
    };
    string editorButtonName;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var methods = target.GetType()
            .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                BindingFlags.NonPublic)
            .Where(o => Attribute.IsDefined(o, typeof(EditorButtonAttribute)));
        int methodIndex = 0;


        if (editorButtonStates == null)
        {
            CreateEditorButtonStates(methods.Select(member => (MethodInfo)member).ToArray());
        }

        foreach (var memberInfo in methods)
        {
            var method = memberInfo as MethodInfo;

            DrawButtonforMethod(target, method, GetEditorButtonState(method, methodIndex));
            methodIndex++;
        }
    }

    void CreateEditorButtonStates(MethodInfo[] methods)
    {
        editorButtonStates = new EditorButtonState[methods.Length];
        int methodIndex = 0;
        foreach (var methodInfo in methods)
        {
            editorButtonStates[methodIndex] = new EditorButtonState(methodInfo.GetParameters().Length);
            methodIndex++;
        }
    }

    EditorButtonState GetEditorButtonState(MethodInfo method, int methodIndex)
    {
        return editorButtonStates[methodIndex];
    }

    void DrawButtonforMethod(Object target, MethodInfo methodInfo, EditorButtonState state)
    {
        EditorGUILayout.BeginHorizontal();
        var foldoutRect = EditorGUILayout.GetControlRect(GUILayout.Width(10.0f));
        state.opened = EditorGUI.Foldout(foldoutRect, state.opened, "");
        string buttonName = MethodDisplayName(methodInfo);
        bool clicked = GUILayout.Button(buttonName, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        if (state.opened)
        {
            EditorGUI.indentLevel++;
            int paramIndex = 0;
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                object currentVal = state.parameters[paramIndex];
                state.parameters[paramIndex] = DrawParameterInfo(parameterInfo, currentVal);
                paramIndex++;
            }
            EditorGUI.indentLevel--;
        }

        if (clicked)
        {
            object returnVal = methodInfo.Invoke(target, state.parameters);

            if (returnVal is IEnumerator)
            {
                if (target is MonoBehaviour mono)
                {
                    mono.StartCoroutine((IEnumerator)returnVal);
                }
            }
            else if (returnVal != null)
            {
                Debug.Log("Method call result -> " + returnVal);
            }
        }
    }

    object GetDefaultValue(ParameterInfo parameter)
    {
        bool hasDefaultValue = !DBNull.Value.Equals(parameter.DefaultValue);

        if (hasDefaultValue)
            return parameter.DefaultValue;

        Type parameterType = parameter.ParameterType;
        if (parameterType.IsValueType)
        {
            return Activator.CreateInstance(parameterType);
        }

        return null;
    }

    object DrawParameterInfo(ParameterInfo parameterInfo, object currentValue)
    {

        object paramValue = null;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(parameterInfo.Name);

        ParameterDrawer drawer = GetParameterDrawer(parameterInfo);
        if (currentValue == null)
            currentValue = GetDefaultValue(parameterInfo);
        paramValue = drawer.Invoke(parameterInfo, currentValue);

        EditorGUILayout.EndHorizontal();

        return paramValue;
    }

    ParameterDrawer GetParameterDrawer(ParameterInfo parameter)
    {
        Type parameterType = parameter.ParameterType;

        if (typeof(UnityEngine.Object).IsAssignableFrom(parameterType))
            return DrawUnityEngineObjectParameter;

        ParameterDrawer drawer;
        if (typeDrawer.TryGetValue(parameterType, out drawer))
        {
            return drawer;
        }

        return null;
    }

    static object DrawFloatParameter(ParameterInfo parameterInfo, object val)
    {
        //Since it is legal to define a float param with an integer default value (e.g void method(float p = 5);)
        //we must use Convert.ToSingle to prevent forbidden casts
        //because you can't cast an "int" object to float 
        //See for http://stackoverflow.com/questions/17516882/double-casting-required-to-convert-from-int-as-object-to-float more info

        return EditorGUILayout.FloatField(Convert.ToSingle(val));
    }

    static object DrawIntParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.IntField((int)val);
    }

    static object DrawBoolParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.Toggle((bool)val);
    }

    static object DrawStringParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.TextField((string)val);
    }

    static object DrawColorParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.ColorField((Color)val);
    }

    static object DrawUnityEngineObjectParameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.ObjectField((UnityEngine.Object)val, parameterInfo.ParameterType, true);
    }

    static object DrawVector2Parameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.Vector2Field("", (Vector2)val);
    }

    static object DrawVector3Parameter(ParameterInfo parameterInfo, object val)
    {
        return EditorGUILayout.Vector3Field("", (Vector3)val);
    }

    static object DrawQuaternionParameter(ParameterInfo parameterInfo, object val)
    {
        return Quaternion.Euler(EditorGUILayout.Vector3Field("", ((Quaternion)val).eulerAngles));
    }

    string MethodDisplayName(MethodInfo method)
    {


        if (Attribute.IsDefined(method, typeof(EditorButtonAttribute)))
        {
            EditorButtonAttribute tmp =
                (EditorButtonAttribute)Attribute.GetCustomAttribute(method, typeof(EditorButtonAttribute));
            editorButtonName = tmp.buttonName;
            if (string.IsNullOrEmpty(editorButtonName))
            {
                editorButtonName = method.Name;
            }

            var methodParams = method.GetParameters();
            if (methodParams.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(editorButtonName + "(");

                foreach (ParameterInfo parameter in methodParams)
                {
                    sb.Append(MethodParameterDisplayName(parameter));
                    sb.Append(",");
                }

                if (methodParams.Length > 0)
                    sb.Remove(sb.Length - 1, 1);

                sb.Append(")");
                editorButtonName = sb.ToString();
            }
        }
        return editorButtonName;
    }

    string MethodParameterDisplayName(ParameterInfo parameterInfo)
    {
        string parameterTypeDisplayName;
        if (!typeDisplayName.TryGetValue(parameterInfo.ParameterType, out parameterTypeDisplayName))
        {
            parameterTypeDisplayName = parameterInfo.ParameterType.ToString();
        }

        return parameterTypeDisplayName + " " + parameterInfo.Name;
    }

    string MethodUID(MethodInfo method)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(method.Name + "_");
        foreach (ParameterInfo parameter in method.GetParameters())
        {
            sb.Append(parameter.ParameterType.ToString());
            sb.Append("_");
            sb.Append(parameter.Name);
        }
        sb.Append(")");
        return sb.ToString();
    }
}
#endif

