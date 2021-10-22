using System;
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

/// <summary> 
/// 绘制代码在 <see cref="EditorGUIMethod"/> 中
/// <para></para>自定义Editor中想要支持这个特性，
/// <para></para>请在<see cref="Editor.OnInspectorGUI"/>中调用
/// this.DrawInspectorMethods
/// 
/// <para></para><see cref="EditorGUIMethod.DrawInspectorMethods"/>
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method)]
public class EditorButtonAttribute : PropertyAttribute
{
    public string ButtonName { get; set; }
    public bool UseTypeFullName { get; set; }

    /// <summary>
    /// <para/>true :编辑模式运行模式都有效.
    /// <para/>false:仅运行模式有效.
    /// </summary>
    /// <value></value>
    public bool ExecuteAlways { get; set; } = true;

    public EditorButtonAttribute(string buttonName = null)
    {
        ButtonName = buttonName;
    }

    public EditorButtonAttribute(bool executeAlways)
    {
        ExecuteAlways = executeAlways;
    }
}

///Editor

#if UNITY_EDITOR

namespace UnityEditor
{
    ///<summary>
    ///一旦脚本有其他自定义Editor，那么EditorButtonAttribute就失效了，
    ///所以改成静态函数和扩展函数，方便其他自定义Editor直接对接 EditorButtonAttribute
    ///</summary>
    public static class EditorGUIMethod
    {
        public class EditorButtonState
        {
            public bool opened;
            public System.Object[] parameters;
            public EditorButtonState(MethodInfo method)
            {
                var paras = method.GetParameters();
                parameters = new System.Object[paras.Length];
                for (int i = 0; i < paras.Length; i++)
                {
                    var parameterInfo = paras[i];
                    if (parameterInfo.HasDefaultValue)
                    {
                        parameters[i] = parameterInfo.DefaultValue;
                    }
                }
            }
        }

        public class DrawMethod
        {
            public MethodInfo Method { get; set; }
            public EditorButtonState State { get; set; }
            public EditorButtonAttribute Attribute { get; internal set; }
        }

        public delegate object ParameterDrawer(ParameterInfo parameter, object val);
        public static Dictionary<Type, ParameterDrawer> TypeDrawer { get; } = new Dictionary<Type, ParameterDrawer>
        {
            {typeof(float),DrawFloatParameter},
            {typeof(int),DrawIntParameter},
            {typeof(string),DrawStringParameter},
            {typeof(bool),DrawBoolParameter},
            {typeof(Color),DrawColorParameter},
            {typeof(Vector3),DrawVector3Parameter},
            {typeof(Vector2),DrawVector2Parameter},
            {typeof(Quaternion),DrawQuaternionParameter},
            {typeof(DateTime),DrawDateTimeParameter},
            {typeof(DateTimeOffset),DrawDateTimeOffsetParameter},
            {typeof(TimeSpan),DrawTimeSpanParameter}
        };

        public static Dictionary<Type, string> TypeDisplayName { get; } = new Dictionary<Type, string>
        {
            {typeof(float),"float"},
            {typeof(int),"int"},
            {typeof(string),"string"},
            {typeof(bool),"bool"},
            {typeof(Color),"Color"},
            {typeof(Vector3),"Vector3"},
            {typeof(Vector2),"Vector2"},
            {typeof(Quaternion),"Quaternion"}
        };

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
            if (val == null && typeof(ScriptableObject).IsAssignableFrom(parameterInfo.ParameterType))
            {
                if (GUILayout.Button("Temp", GUILayout.Width(50)))
                {
                    //创建一个临时对象,没有保存在工程里.
                    var instance = ScriptableObject.CreateInstance(parameterInfo.ParameterType);
                    //instance.OpenProperty(); 报错
                    return instance;
                }
            }
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

        static object DrawTimeSpanParameter(ParameterInfo parameterInfo, object val)
        {
            TimeSpan span = (TimeSpan)val;
            var str = EditorGUILayout.TextField(span.ToString());
            var success = TimeSpan.TryParse(str, out span);
            return span;
        }

        static object DrawDateTimeParameter(ParameterInfo parameterInfo, object val)
        {
            DateTime dateTime = (DateTime)val;
            var str = EditorGUILayout.TextField(dateTime.ToString());
            DateTime.TryParse(str, out dateTime);
            return dateTime;
        }

        static object DrawDateTimeOffsetParameter(ParameterInfo parameterInfo, object val)
        {
            DateTimeOffset dateTime = (DateTimeOffset)val;
            var str = EditorGUILayout.TextField(dateTime.ToString());
            DateTimeOffset.TryParse(str, out dateTime);
            return dateTime;
        }

        public static (ParameterDrawer, bool) GetParameterDrawer(ParameterInfo parameter)
        {
            Type parameterType = parameter.ParameterType;

            if (typeof(UnityEngine.Object).IsAssignableFrom(parameterType))
                return (DrawUnityEngineObjectParameter, false);

            ParameterDrawer drawer;

            bool isnullable = false;
            (isnullable, parameterType) = CheckNullable(parameterType);

            if (TypeDrawer.TryGetValue(parameterType, out drawer))
            {
                return (drawer, isnullable);
            }

            return default;
        }

        private static (bool Isnullable, Type ParamType) CheckNullable(Type parameterType)
        {
            bool isnullable = false;
            if (parameterType.IsGenericType)
            {
                if (parameterType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    isnullable = true;
                    parameterType = parameterType.GetGenericArguments()[0];
                }
            }

            return (isnullable, parameterType);
        }

        static object GetDefaultValue(ParameterInfo parameter)
        {
            bool hasDefaultValue = !DBNull.Value.Equals(parameter.DefaultValue);

            if (hasDefaultValue)
                return parameter.DefaultValue;

            Type parameterType = parameter.ParameterType;
            var res = CheckNullable(parameterType);
            if (res.Isnullable)
            {
                if (res.ParamType.IsValueType)
                {
                    if (res.ParamType == typeof(TimeSpan))
                    {
                        return TimeSpan.FromSeconds(5);
                    }
                    else if (res.ParamType == typeof(DateTimeOffset))
                    {
                        return DateTimeOffset.Now;
                    }
                    else if (res.ParamType == typeof(DateTime))
                    {
                        return DateTime.Now;
                    }
                    return Activator.CreateInstance(res.ParamType);
                }
            }

            if (parameterType.IsValueType)
            {
                return Activator.CreateInstance(parameterType);
            }

            return null;
        }

        public static object DrawParameterInfo(ParameterInfo parameterInfo, object currentValue)
        {

            object paramValue = null;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(parameterInfo.Name, GUILayout.MinWidth(100), GUILayout.MaxWidth(200));

            (ParameterDrawer drawer, bool isNullable) = GetParameterDrawer(parameterInfo);
            if (isNullable)
            {
                var curNull = currentValue != null;
                GUI.enabled = curNull;

                if (drawer != null)
                {
                    if (currentValue == null)
                        currentValue = GetDefaultValue(parameterInfo);
                    paramValue = drawer.Invoke(parameterInfo, currentValue);
                }

                GUI.enabled = true;
                var newNull = EditorGUILayout.Toggle(curNull);
                if (!newNull)
                {
                    paramValue = null;
                }
            }
            else
            {
                if (drawer != null)
                {
                    if (currentValue == null)
                        currentValue = GetDefaultValue(parameterInfo);
                    paramValue = drawer.Invoke(parameterInfo, currentValue);
                }
            }

            EditorGUILayout.EndHorizontal();

            return paramValue;
        }

        public static string MethodParameterDisplayName(ParameterInfo parameterInfo, bool useFullName = false)
        {
            string parameterTypeDisplayName;
            if (!TypeDisplayName.TryGetValue(parameterInfo.ParameterType, out parameterTypeDisplayName))
            {
                if (useFullName)
                {
                    parameterTypeDisplayName = parameterInfo.ParameterType.FullName;
                }
                else
                {
                    parameterTypeDisplayName = parameterInfo.ParameterType.Name;
                }
            }

            return parameterTypeDisplayName + " " + parameterInfo.Name;
        }

        public static string MethodDisplayName(MethodInfo method)
        {
            string editorButtonName = "Unknown";
            if (Attribute.IsDefined(method, typeof(EditorButtonAttribute)))
            {
                EditorButtonAttribute tmp =
                    (EditorButtonAttribute)Attribute.GetCustomAttribute(method, typeof(EditorButtonAttribute));
                editorButtonName = tmp.ButtonName;
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
                        sb.Append(MethodParameterDisplayName(parameter, tmp.UseTypeFullName));
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

        public static string MethodUID(MethodInfo method)
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

        public static void DrawButtonforMethod(Object target, MethodInfo methodInfo, EditorButtonState state, bool executeAlways)
        {
            ///方法绘制间隔
            EditorGUILayout.Space(1);

            EditorGUILayout.BeginHorizontal();

            GUI.enabled = methodInfo.GetParameters().Length > 0;
            var foldoutRect = EditorGUILayout.GetControlRect(GUILayout.Width(10.0f));
            state.opened = EditorGUI.Foldout(foldoutRect, state.opened, "");

            GUI.enabled = executeAlways || Application.isPlaying;
            string buttonName = MethodDisplayName(methodInfo);
            bool clicked = GUILayout.Button(buttonName, GUILayout.ExpandWidth(true));

            GUI.enabled = true;
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
                if (executeAlways || Application.isPlaying)
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

                    ///强制更新
                    if (target)
                    {
                        target.InspectorForceUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// 搜集需要绘制按钮的方法
        /// </summary>
        /// <param name="target"></param>
        /// <param name="list"></param>
        public static void CollectDrawButtonMethod(this Editor editor, List<DrawMethod> list)
        {
            var methods = editor.target.GetType()
                        .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                            BindingFlags.NonPublic)
                        .Where(o => Attribute.IsDefined(o, typeof(EditorButtonAttribute)));

            list.Clear();
            foreach (var methodInfo in methods)
            {
                list.Add(new DrawMethod()
                {
                    Method = methodInfo,
                    Attribute = methodInfo.GetCustomAttribute<EditorButtonAttribute>(),
                    State = new EditorButtonState(methodInfo),
                });
            }

            list.Sort((lhs, rhs) =>
            {
                return lhs.Attribute.order.CompareTo(rhs.Attribute.order);
            });
        }

        /// <summary>
        /// 绘制按钮
        /// </summary>
        /// <param name="target"></param>
        /// <param name="list"></param>
        public static void DrawInspectorMethods(this Editor editor, List<DrawMethod> list)
        {
            foreach (var draw in list)
            {
                DrawButtonforMethod(editor.target, draw.Method, draw.State, draw.Attribute.ExecuteAlways);
            }
        }

        /// <summary>
        /// 方法缓存，每个脚本类型公用一组方法和参数。每次重写编译，重载域，静态缓存都会被清除。
        /// </summary>
        static Dictionary<Type, List<DrawMethod>> MethodCache
            = new Dictionary<Type, List<DrawMethod>>();

        /// <summary>
        /// 绘制按钮
        /// </summary>
        /// <param name="target"></param>
        /// <param name="list"></param>
        public static void DrawInspectorMethods(this Editor editor)
        {
            Type type = editor.target.GetType();
            if (!MethodCache.TryGetValue(type, out var list))
            {
                list = new List<DrawMethod>();
                editor.CollectDrawButtonMethod(list);
                MethodCache[type] = list;
            }

            editor.DrawInspectorMethods(list);
        }
    }
}

#endif

#if UNITY_EDITOR

#if DISABLE_EDITORBUTTONATTRIBUTE

#else
[ExcludeFromObjectFactory]
[CanEditMultipleObjects]
[CustomEditor(typeof(Object), true)]
#endif
public class EditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        this.DrawInspectorMethods();
    }
}

#endif

