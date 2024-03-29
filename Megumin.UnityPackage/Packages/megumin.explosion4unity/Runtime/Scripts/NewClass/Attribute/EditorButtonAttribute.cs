﻿using System;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using Object = UnityEngine.Object;
using System.Reflection;
using Megumin;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Megumin
{

#if false && COMPATIBILITY_NaughtyAttributes //兼容NaughtyAttributes特性库

    /// <summary>
    /// 兼容后不支持参数而且控制台日志太多
    /// </summary>
    public class EditorButtonAttribute : NaughtyAttributes.ButtonAttribute
    {
        public int order { get; set; }
        public string ButtonName => Text;
        public bool UseTypeFullName { get; set; }
        public bool OnlyPlaying { get; set; } = false;
        public EditorButtonAttribute(string buttonName = null) : base(buttonName)
        {

        }

        public EditorButtonAttribute(bool onlyPlaying) :
            base(null,
                 onlyPlaying ?
            NaughtyAttributes.EButtonEnableMode.Playmode
            : NaughtyAttributes.EButtonEnableMode.Always)
        {
            OnlyPlaying = onlyPlaying;
        }
    }

#else

    //从 https://github.com/miguel12345/EditorButton 修改
    //支持ScrpitObject

    /// <summary> 
    /// 绘制代码在 <see cref="EditorGUIMethod"/> 中
    /// <para></para>自定义Editor中想要支持这个特性，
    /// <para></para>请在<see cref="Editor.OnInspectorGUI"/>中调用
    /// this.DrawInspectorMethods
    /// 
    /// <para></para><see cref="EditorGUIMethod.DrawInspectorMethods"/>
    /// <para>在自定义Serializable类型方法中无效,无法显示在面板</para>
    /// </summary>
    [Obsolete("Rename. Use ButtonAttribute instead.", true)]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class EditorButtonAttribute : PropertyAttribute
    {
        public bool AfterDrawDefaultInspector = true;
        public string ButtonName { get; set; }
        public bool UseTypeFullName { get; set; }
        public float ButtonHeight { get; set; } = 20f;

        /// <summary>
        /// <para/>true :编辑模式运行模式都有效.
        /// <para/>false:仅运行模式有效.
        /// </summary>
        /// <value></value>
        [Obsolete("Use EnableMode")]
        public bool OnlyPlaying { get; set; } = false;

        public EditorButtonAttribute(string buttonName = null)
        {
            ButtonName = buttonName;
        }

        public EditorButtonAttribute(bool onlyPlaying)
        {
            OnlyPlaying = onlyPlaying;
        }

        /// <summary>
        /// <para/> 1 : only playing  
        /// <para/> 2 : only editor
        /// <para/> others : always   
        /// </summary>
        public int EnableMode { get; set; } = 0;
    }



#endif

    /// <summary>
    /// <see cref="UnityEngine.ContextMenu"/> 和 <see cref="System.ComponentModel.EditorAttribute"/>
    /// 同样可以显示一个按钮，因为是借用通用的特性，所以没有精确绘制参数可以设置。
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public bool AfterDrawDefaultInspector = true;
        public string ButtonName { get; set; }
        public bool UseTypeFullName { get; set; }
        public float ButtonHeight { get; set; } = 20f;

        /// <summary>
        /// <para/>true :编辑模式运行模式都有效.
        /// <para/>false:仅运行模式有效.
        /// </summary>
        /// <value></value>
        [Obsolete("Use EnableMode")]
        public bool OnlyPlaying { get; set; } = false;

        public string Name
        {
            get
            {
                return ButtonName;
            }
            set
            {
                ButtonName = value;
            }
        }

        public ButtonAttribute(string name = null, int buttonSize = 20)
        {
            Name = name;
            ButtonHeight = buttonSize;
        }

        public ButtonAttribute(bool onlyPlaying)
        {
            EnableMode = onlyPlaying ? 1 : 0;
        }


        /// <inheritdoc cref="EnableMode"/>
        public ButtonAttribute(int enableMode)
        {
            EnableMode = enableMode;
        }

        /// <summary>
        /// <para/> 1 : only playing  
        /// <para/> 2 : only editor
        /// <para/> others : always   
        /// </summary>
        public int EnableMode { get; set; } = 0;
    }
}



///Editor

#if UNITY_EDITOR

namespace UnityEditor.Megumin
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
            public Attribute Attribute { get; internal set; }

            /// <summary>
            /// <para/> 1 : only playing  
            /// <para/> 2 : only editor
            /// <para/> others : always   
            /// </summary>
            public int EnableMode { get; set; } = 0;
            public TooltipAttribute Tooltip { get; internal set; }
            public string HelpBox { get; internal set; }
            public MessageType HelpBoxMessageType { get; internal set; }
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
            {typeof(Enum),DrawEnumParameter},
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

        private static object DrawEnumParameter(ParameterInfo parameter, object val)
        {
            var v = Enum.Parse(parameter.ParameterType, val.ToString());
            var res = EditorGUILayout.EnumFlagsField((Enum)v);
            return res;
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
            else
            {
                if (parameterType.IsEnum)
                {
                    return (DrawEnumParameter, isnullable);
                }
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
                    else if (res.ParamType == typeof(Color))
                    {
                        return Color.white;
                    }
                    return Activator.CreateInstance(res.ParamType);
                }
            }

            if (parameterType.IsValueType)
            {
                if (res.ParamType == typeof(Color))
                {
                    return Color.white;
                }
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

                EditorGUI.BeginDisabledGroup(!curNull);
                if (drawer != null)
                {
                    if (currentValue == null)
                        currentValue = GetDefaultValue(parameterInfo);
                    paramValue = drawer.Invoke(parameterInfo, currentValue);
                }
                EditorGUI.EndDisabledGroup();

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

        public static string MethodDisplayName(MethodInfo method, Attribute drawAttribute)
        {
            string editorButtonName = "";
            bool useTypeFullName = false;
            if (drawAttribute is ButtonAttribute buttonAttribute)
            {
                editorButtonName = buttonAttribute.ButtonName;
                useTypeFullName = buttonAttribute.UseTypeFullName;
            }
            else if (drawAttribute is ContextMenu contextMenu)
            {
                editorButtonName = contextMenu.menuItem;
            }

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
                    sb.Append(MethodParameterDisplayName(parameter, useTypeFullName));
                    sb.Append(",");
                }

                if (methodParams.Length > 0)
                    sb.Remove(sb.Length - 1, 1);

                sb.Append(")");
                editorButtonName = sb.ToString();
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

        [Obsolete("", true)]
        public static void DrawButtonforMethod(Object target,
                                               MethodInfo methodInfo,
                                               EditorButtonState state,
                                               bool onlyPlaying)
        {
            DrawButtonforMethod(target, methodInfo, null, state, onlyPlaying ? 1 : 0);
        }

        public static void DrawButtonforMethod(Object target,
                                               DrawMethod drawMethod)
        {
            DrawButtonforMethod(target, drawMethod.Method, drawMethod.Attribute, drawMethod.State, drawMethod.EnableMode, drawMethod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodInfo"></param>
        /// <param name="state"></param>
        /// <param name="enableMode">
        /// <para/> 1 : only playing  
        /// <para/> 2 : only editor
        /// <para/> others : always   
        /// </param>
        public static void DrawButtonforMethod(Object target,
                                               MethodInfo methodInfo,
                                               Attribute drawAttribute,
                                               EditorButtonState state,
                                               int enableMode = 0,
                                               DrawMethod drawMethod = null)
        {
            ///方法绘制间隔
            EditorGUILayout.Space(1);

            if (!string.IsNullOrEmpty(drawMethod?.HelpBox))
            {
                EditorGUILayout.HelpBox(drawMethod.HelpBox, drawMethod.HelpBoxMessageType);
            }

            EditorGUILayout.BeginHorizontal();

            using (new EditorGUI.DisabledScope(methodInfo.GetParameters().Length <= 0))
            {
                var foldoutRect = EditorGUILayout.GetControlRect(GUILayout.Width(10.0f));
                state.opened = EditorGUI.Foldout(foldoutRect, state.opened, "");
            }

            var enable = true;
            switch (enableMode)
            {
                case 1:
                    enable = Application.isPlaying;
                    break;
                case 2:
                    enable = !Application.isPlaying;
                    break;
                default:
                    break;
            }

            EditorGUI.BeginDisabledGroup(!enable);

            //绘制按钮
            string buttonName = MethodDisplayName(methodInfo, drawAttribute);
            GUIContent button = new GUIContent(buttonName);
            button.tooltip = drawMethod?.Tooltip?.tooltip;
            bool clicked = GUILayout.Button(button, GUILayout.ExpandWidth(true));

            EditorGUI.EndDisabledGroup();

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

                if (returnVal != null)
                {
                    UnityEngine.Debug.Log($"{methodInfo.Name} return : {returnVal}");
                }

                if (Application.isPlaying)
                {
                    if (returnVal is IEnumerator)
                    {
                        if (target is MonoBehaviour mono)
                        {
                            mono.StartCoroutine((IEnumerator)returnVal);
                        }
                    }
                }

                ///强制更新
                if (target)
                {
                    target.InspectorForceUpdate();
                }
            }
        }

        /// <summary>
        /// 搜集需要绘制按钮的方法
        /// </summary>
        /// <param name="target"></param>
        /// <param name="list"></param>
        public static void CollectDrawButtonMethod(this Editor editor,
                                                   List<DrawMethod> list,
                                                   bool alsoCollectContextMenu = true,
                                                   bool alsoCollectSystemComponentModelEditor = true)
        {
            list.Clear();

            var methods = from method in editor.target.GetType()
                         .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                             BindingFlags.NonPublic)
                          select method;

            var methodsButtonAttribute = from method in methods
                                         let attri = method.GetCustomAttribute<ButtonAttribute>()
                                         where attri != null
                                         orderby attri.order ascending
                                         select (method, attri);

            foreach (var item in methodsButtonAttribute)
            {
                DrawMethod draw = new DrawMethod()
                {
                    Method = item.method,
                    Attribute = item.attri,
                    State = new EditorButtonState(item.method),
                    EnableMode = item.attri.EnableMode,
                };

#pragma warning disable CS0618 // 类型或成员已过时
                if (item.attri.OnlyPlaying)
                {
                    draw.EnableMode = 1;
                }
#pragma warning restore CS0618 // 类型或成员已过时

                list.Add(draw);
            }

            if (alsoCollectContextMenu)
            {
                //借用 UnityEngine.ContextMenu
                var methodsContextMenu = from method in methods
                                         let attri = method.GetCustomAttribute<UnityEngine.ContextMenu>()
                                         where attri != null
                                         orderby method.MetadataToken ascending
                                         select (method, attri);

                foreach (var item in methodsContextMenu)
                {
                    if (!list.Any(draw => draw.Method == item.method))
                    {
                        list.Add(new DrawMethod()
                        {
                            Method = item.method,
                            Attribute = item.attri,
                            State = new EditorButtonState(item.method),
                        });
                    }
                }
            }

            if (alsoCollectSystemComponentModelEditor)
            {
                //借用 System.ComponentModel.EditorAttribute
                var methodsEditorAttribute = from method in methods
                                             let attri = method.GetCustomAttribute<System.ComponentModel.EditorAttribute>()
                                             where attri != null
                                             orderby method.MetadataToken ascending
                                             select (method, attri);

                foreach (var item in methodsEditorAttribute)
                {
                    if (!list.Any(draw => draw.Method == item.method))
                    {
                        DrawMethod draw = new DrawMethod()
                        {
                            Method = item.method,
                            Attribute = item.attri,
                            State = new EditorButtonState(item.method),
                        };

                        if (string.Equals(item.attri.EditorTypeName, "OnlyPlaying", StringComparison.OrdinalIgnoreCase))
                        {
                            draw.EnableMode = 1;
                        }

                        if (string.Equals(item.attri.EditorTypeName, "OnlyEditor", StringComparison.OrdinalIgnoreCase))
                        {
                            draw.EnableMode = 2;
                        }

                        list.Add(draw);
                    }
                }
            }

            foreach (var item in list)
            {
                item.Tooltip = item.Method.GetCustomAttribute<TooltipAttribute>();
                var helpBoxAttribute = item.Method.GetCustomAttribute<HelpBoxAttribute>();
                if (helpBoxAttribute != null)
                {
                    item.HelpBox = helpBoxAttribute.Text;
                    item.HelpBoxMessageType = (MessageType)helpBoxAttribute.MessageType;
                }

            }
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
                DrawButtonforMethod(editor.target, draw);
            }
        }

        public static void DrawInspectorMethodsBeforeDefault(this Editor editor, List<DrawMethod> list)
        {
            foreach (var draw in list)
            {
                if (draw.Attribute is ButtonAttribute button)
                {
                    if (button.AfterDrawDefaultInspector == false || button.order < 0)
                    {
                        DrawButtonforMethod(editor.target, draw);
                    }
                }
            }
        }

        public static void DrawInspectorMethodsAfterDefault(this Editor editor, List<DrawMethod> list)
        {
            foreach (var draw in list)
            {
                if (draw.Attribute is ButtonAttribute button)
                {
                    if (button.AfterDrawDefaultInspector == false || button.order < 0)
                    {

                    }
                    else
                    {
                        DrawButtonforMethod(editor.target, draw);
                    }
                }
                else
                {
                    DrawButtonforMethod(editor.target, draw);
                }
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
        public static void DrawInspectorMethods(this Editor editor,
                                                bool alsoCollectContextMenu = true,
                                                bool alsoCollectSystemComponentModelEditor = true)
        {
            Type type = editor.target.GetType();
            if (!MethodCache.TryGetValue(type, out var list))
            {
                list = new List<DrawMethod>();
                editor.CollectDrawButtonMethod(list, alsoCollectContextMenu, alsoCollectSystemComponentModelEditor);
                MethodCache[type] = list;
            }

            editor.DrawInspectorMethods(list);
        }

        public static void DrawButtonBeforeDefaultInspector(this Editor editor,
                                                            bool alsoCollectContextMenu = true,
                                                            bool alsoCollectSystemComponentModelEditor = true)
        {
            Type type = editor.target.GetType();
            if (!MethodCache.TryGetValue(type, out var list))
            {
                list = new List<DrawMethod>();
                editor.CollectDrawButtonMethod(list, alsoCollectContextMenu, alsoCollectSystemComponentModelEditor);
                MethodCache[type] = list;
            }

            editor.DrawInspectorMethodsBeforeDefault(list);
        }

        public static void DrawButtonAfterDefaultInspector(this Editor editor,
                                                           bool alsoCollectContextMenu = true,
                                                           bool alsoCollectSystemComponentModelEditor = true)
        {
            Type type = editor.target.GetType();
            if (!MethodCache.TryGetValue(type, out var list))
            {
                list = new List<DrawMethod>();
                editor.CollectDrawButtonMethod(list, alsoCollectContextMenu, alsoCollectSystemComponentModelEditor);
                MethodCache[type] = list;
            }

            editor.DrawInspectorMethodsAfterDefault(list);
        }
    }
}

#endif

namespace UnityEditor.Megumin
{

#if UNITY_EDITOR

#if DISABLE_EDITORBUTTONATTRIBUTE

#else

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromObjectFactory]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = false)]
    public class EditorButton : Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawButtonBeforeDefaultInspector();
            base.OnInspectorGUI();
            this.DrawButtonAfterDefaultInspector();
        }
    }

    /// <summary>
    /// isFallback 是true 会被其他 [CustomEditor(typeof(UnityEngine.Object))]抢占,恶性竞争
    /// </summary>
    [ExcludeFromObjectFactory]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Component), true, isFallback = false)]
    public class ComponentEditorButton : EditorButton { }

    [ExcludeFromObjectFactory]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Behaviour), true, isFallback = true)]
    public class BehaviourEditorButton : EditorButton { }

    [ExcludeFromObjectFactory]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    public class MonoBehaviourEditorButton : EditorButton { }

    [ExcludeFromObjectFactory]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObject), true, isFallback = false)]
    public class ScriptableObjectButton : EditorButton { }

#endif

#endif


}

