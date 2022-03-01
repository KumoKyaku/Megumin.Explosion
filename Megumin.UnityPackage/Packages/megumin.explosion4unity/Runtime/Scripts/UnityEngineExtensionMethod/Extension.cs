using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Megumin;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Profiling;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine
{
    public static partial class MeguminExtension_2B5D73B2
    {
        public static bool HasError(this UnityWebRequest uwr)
        {
            bool error = false;
#if UNITY_2020_1_OR_NEWER
            error = uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError;
#else
            error = uwr.isNetworkError || uwr.isHttpError;
#endif
            return error;
        }

        /// <summary>
        /// 在编辑器中将资源标记为已更改。
        /// </summary>
        /// <param name="behaviour"></param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void AssetDataSetDirty(this Object obj)
        {

#if UNITY_EDITOR
            if (obj)
            {
                if (!Application.isPlaying)
                {
                    UnityEditor.EditorUtility.SetDirty(obj);
                }
            }
#endif

        }


        /// <summary>
        /// 这里插入一个EditorUpdate达到刷效果，否则编辑器模式下脚本Update调用不及时。
        /// </summary>
        /// <param name="obj"></param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void InspectorForceUpdate(this UnityEngine.Object obj)
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
#endif

        }

        static MethodInfo OpenPropertyEditor;

        /// <summary>
        /// 通过反射打开属性面板
        /// </summary>
        /// <param name="object"></param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void OpenProperty(this UnityEngine.Object @object)
        {
#if UNITY_EDITOR

            if (OpenPropertyEditor == null)
            {
                var ab = Assembly.GetAssembly(typeof(EditorWindow));
                var propertyEditor = ab.GetType("UnityEditor.PropertyEditor");
                OpenPropertyEditor = propertyEditor.GetMethod("OpenPropertyEditor",
                                                              BindingFlags.NonPublic
                                                              | BindingFlags.Static);
            }

            OpenPropertyEditor?.Invoke(null, new object[] { @object, true });
#endif
        }

        /// <summary>
        /// 返回新对象路径，仅编辑下有效
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public static string CreateNewPath(this Object obj, bool isNew = false)
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(obj);
            string newFileName = "NewInstance";
            if (isNew)
            {
                newFileName = obj.GetType().Name;
            }
            else
            {
                newFileName = Path.GetFileNameWithoutExtension(path) + " Clone";
            }
            return path.ReplaceFileName(newFileName);
#else
            return default;
#endif
        }

        public static string GetFileName(this ScriptableObject scriptableObject)
        {

#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(scriptableObject);
            return Path.GetFileNameWithoutExtension(path);
#else
            return default;
#endif

        }

        /// <summary>
        /// 宏替换,将给定字符中的宏替换为unity对象的值。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="orignal"></param>
        public static void Macro(this Object obj, ref string orignal)
        {
            Profiler.BeginSample(nameof(Macro));
            if (!string.IsNullOrEmpty(orignal))
            {
                if (obj)
                {
                    orignal = orignal.Replace("$(name)", obj.name);

                    if (obj is Component component)
                    {
                        MacroGameObject(component.gameObject, ref orignal);
                        MacroTransform(component.transform, ref orignal);
                    }

                    if (obj is GameObject gameObject)
                    {
                        MacroGameObject(gameObject, ref orignal);
                        MacroTransform(gameObject.transform, ref orignal);

                    }

                    if (obj is Transform transform)
                    {
                        MacroTransform(transform, ref orignal);
                        MacroGameObject(transform.gameObject, ref orignal);
                    }
                }
            }

            Profiler.EndSample();
        }

        private static void MacroTransform(Transform transform, ref string orignal)
        {
            orignal = orignal.Replace("$(position)", transform.position.ToString());
            orignal = orignal.Replace("$(rotation)", transform.rotation.ToString());
            orignal = orignal.Replace("$(eulerAngles)", transform.eulerAngles.ToString());

            orignal = orignal.Replace("$(localPosition)", transform.localPosition.ToString());
            orignal = orignal.Replace("$(localRotation)", transform.localRotation.ToString());
            orignal = orignal.Replace("$(localEulerAngles)", transform.localEulerAngles.ToString());

            orignal = orignal.Replace("$(parent)", transform.parent?.name);
        }

        private static void MacroGameObject(GameObject gameObject, ref string orignal)
        {
            orignal = orignal.Replace("$(layer)", LayerMask.LayerToName(gameObject.layer));
            orignal = orignal.Replace("$(tag)", gameObject.tag);
        }

        public static void LogNotImplemented(this Object obj, [CallerMemberName] string funcName = null)
        {
            UnityEngine.Debug.LogError($"{funcName} NotImplemented.    [Instead throw Exception]");
        }

        public static string LogCallerMemberName(this Object obj, [CallerMemberName] string func = default)
        {
            Debug.Log(func);
            return func;
        }

        /// <summary>
        /// UnityEditor或DEBUG中使用Add添加,发生key冲突直接报错.
        /// Runtime中使用索引添加,尽量让程序不崩溃.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="pairs"></param>
        /// <param name="k"></param>
        /// <param name="v"></param>
        public static void UnityAdd<K, V>(this IDictionary<K, V> pairs, K k, V v)
        {
#if UNITY_EDITOR || DEBUG
            pairs.Add(k, v);
#else
            pairs[k] = v;
#endif
        }

        /// <summary>
        /// 获取文件创建时间
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DateTime GetCreationTimeUtc(this UnityEngine.Object @object)
        {
#if UNITY_EDITOR
            return GetFileInfo(@object).CreationTime;
#endif
            throw new NotImplementedException();
        }

        public static FileInfo GetFileInfo(this UnityEngine.Object @object)
        {
#if UNITY_EDITOR
            var file = new FileInfo(GetAbsoluteFilePath(@object));
            return file;
#endif
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetAbsoluteFilePath(this UnityEngine.Object @object)
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(@object);
            var mfp = AssetDatabase.GetTextMetaFilePathFromAssetPath(path);
            //Debug.Log(mfp);
            var fp = AssetDatabase.GetAssetPathFromTextMetaFilePath(mfp);
            //Debug.Log(fp);
            var gp = Path.Combine(MeguminUtility4Unity.ProjectPath, fp);
            //Debug.Log(gp);
            return gp;
#endif
            throw new NotImplementedException();
        }


        public static string ToHyperlink(this UnityEngine.Object @object, string baseStr)
        {
#if UNITY_EDITOR
            var link = $"<a href=\"{UnityEditor.AssetDatabase.GetAssetPath(@object)}\">{baseStr}</a>";
            return link;
#else
            return baseStr;
#endif
        }
    }

    public static partial class MeguminExtension_2B5D73B2
    {

#if UNITY_EDITOR

        /// <summary>
        /// https://github.com/mob-sakai/SoftMaskForUGUI/blob/main/Scripts/Editor/EditorUtils.cs#L24
        /// <para/>Verify whether it can be converted to the specified component.
        /// </summary>
        public static bool CanConvertTo<T>(this MenuCommand command) where T : MonoBehaviour
        {
            return command != null && command.context && command.context.GetType() != typeof(T);
        }

        /// <summary>
        /// https://github.com/mob-sakai/SoftMaskForUGUI/blob/main/Scripts/Editor/EditorUtils.cs#L32
        /// <para/>Convert to the specified component.
        /// </summary>
        public static void ConvertTo<T>(this MenuCommand command) where T : MonoBehaviour
        {
            if (command == null)
            {
                return;
            }

            var target = command.context as MonoBehaviour;
            var so = new SerializedObject(target);
            so.Update();

            var oldEnable = target.enabled;
            target.enabled = false;

            // Find MonoScript of the specified component.
            foreach (var script in Resources.FindObjectsOfTypeAll<MonoScript>())
            {
                if (script.GetClass() != typeof(T))
                    continue;

                // Set 'm_Script' to convert.
                so.FindProperty("m_Script").objectReferenceValue = script;
                so.ApplyModifiedProperties();
                break;
            }

            (so.targetObject as MonoBehaviour).enabled = oldEnable;
        }

        public static void AddUGUI<T>(this MenuCommand command) where T : MonoBehaviour
        {
            System.Reflection.BindingFlags flags
                = System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Static;

            //太复杂了，直接反射

            System.Reflection.Assembly assembly = typeof(UnityEditor.UI.ImageEditor).Assembly;
            var mo = assembly.GetType("UnityEditor.UI.MenuOptions");
            var DefaultEditorFactoryType = assembly.GetType("UnityEditor.UI.MenuOptions+DefaultEditorFactory");

            UI.DefaultControls.IFactoryControls factory
                = DefaultEditorFactoryType.GetField("Default", flags).GetValue(null) as UI.DefaultControls.IFactoryControls;
            var method = mo.GetMethod("PlaceUIElementRoot", flags);

            GameObject go = factory.CreateGameObject(typeof(T).Name, typeof(T));
            method.Invoke(null, new object[] { go, command });
        }

#endif

    }
}


