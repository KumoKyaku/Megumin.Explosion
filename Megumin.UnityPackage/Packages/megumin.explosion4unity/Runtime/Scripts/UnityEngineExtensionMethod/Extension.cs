using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Megumin;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine
{
    public static class MeguminExtension_2B5D73B2
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
            if (orignal is null)
            {
                return;
            }

            if (!obj)
            {
                return;
            }

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

        private static void MacroTransform(Transform transform, ref string orignal)
        {
            orignal = orignal.Replace("$(position)", transform.position.ToString());
            orignal = orignal.Replace("$(rotation)", transform.rotation.ToString());
            orignal = orignal.Replace("$(eulerAngles)", transform.eulerAngles.ToString());

            orignal = orignal.Replace("$(localPosition)", transform.localPosition.ToString());
            orignal = orignal.Replace("$(localRotation)", transform.localRotation.ToString());
            orignal = orignal.Replace("$(localEulerAngles)", transform.localEulerAngles.ToString());

            orignal = orignal.Replace("$(parent)", transform.parent.name);
        }

        private static void MacroGameObject(GameObject gameObject, ref string orignal)
        {
            orignal = orignal.Replace("$(layer)", LayerMask.LayerToName(gameObject.layer));
            orignal = orignal.Replace("$(tag)", gameObject.tag);
        }
    }
}


