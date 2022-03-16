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
using System.Runtime.InteropServices;
using System.Windows; // Or use whatever point class you like for the implicit cast operator

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
            error = uwr.result != UnityWebRequest.Result.Success;
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

        public static string ToHyperlink(this UnityEngine.Object @object)
        {
#if UNITY_EDITOR
            var link = $"<a href=\"{UnityEditor.AssetDatabase.GetAssetPath(@object)}\">{@object.ToString()}</a>";
            return link;
#else
            return @object.ToString();
#endif
        }
    }

    public static partial class MeguminExtension_2B5D73B2
    {

#if UNITY_EDITOR

        public static void ConvertTo(this MonoBehaviour mono, MonoScript script)
        {
            if (!script)
            {
                Debug.LogError("Target MonoScript is Null!");
            }

            if (!mono)
            {
                Debug.LogError("MonoBehaviour is Null!");
            }

            var so = new SerializedObject(mono);
            so.Update();

            string opName = $"{mono.GetType().FullName} -> {script.GetClass().FullName}";

            //Undo 不起作用疯狂报错。
            Undo.RegisterCompleteObjectUndo(so.targetObject, opName);

            var oldEnable = mono.enabled;
            mono.enabled = false;

            Debug.Log(opName.Html(HexColor.FrenchViolet));

            // Set 'm_Script' to convert.
            so.FindProperty("m_Script").objectReferenceValue = script;
            so.ApplyModifiedProperties();

            (so.targetObject as MonoBehaviour).enabled = oldEnable;
        }

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

            MonoScript targetScript = null;

            // Find MonoScript of the specified component.
            foreach (var script in Resources.FindObjectsOfTypeAll<MonoScript>())
            {
                if (script.GetClass() != typeof(T))
                    continue;

                targetScript = script;
                break;
            }

            var target = command.context as MonoBehaviour;
            target.ConvertTo(targetScript);
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

        [MenuItem("CONTEXT/Component/Convert Inheritance Type", true)]
        private static bool CanConvertInheritanceType(MenuCommand command)
        {
            if (command == null)
            {
                return false;
            }

            var target = command.context as MonoBehaviour;
            if (!target)
            {
                return false;
            }

            return true;
        }

        [MenuItem("CONTEXT/Component/Convert Inheritance Type", false)]
        private static void ConvertInheritanceType(MenuCommand command)
        {
            if (command == null)
            {
                return;
            }

            var target = command.context as MonoBehaviour;
            if (!target)
            {
                return;
            }
            var myType = target.GetType();

            List<(Type Type, MonoScript Script)> child = new List<(Type Type, MonoScript Script)>();
            List<(Type Type, MonoScript Script)> parent = new List<(Type Type, MonoScript Script)>();

            var monoBehaviour = typeof(MonoBehaviour);

            // Find MonoScript of the specified component.
            foreach (var script in Resources.FindObjectsOfTypeAll<MonoScript>())
            {
                var tempType = script.GetClass();
                if (tempType == null
                    || tempType.IsAbstract
                    || tempType.IsGenericType)
                {
                    continue;
                }

                if (!tempType.IsSubclassOf(monoBehaviour))
                {
                    continue;
                }

                if (myType.IsSubclassOf(tempType))
                {
                    parent.Add((tempType, script));
                    continue;
                }

                if (tempType.IsSubclassOf(myType))
                {
                    child.Add((tempType, script));
                    continue;
                }
            }

            Debug.Log("Prepare Conver Monoscript Type......");
            var menu = new GenericMenu();
            void OnClick(object node)
            {
                if (node is ValueTuple<Type, MonoScript> tree)
                {
                    target.ConvertTo(tree.Item2);
                }
            }

            foreach (var item in parent)
            {
                menu.AddItem(new GUIContent(item.Type.FullName), false, OnClick, item);
            }

            if (parent.Count > 0 && child.Count > 0)
            {
                menu.AddItem(new GUIContent("---------"), false, OnClick, null);
            }

            foreach (var item in child)
            {
                menu.AddItem(new GUIContent(item.Type.FullName), false, OnClick, item);
            }

            var rect = Rect.zero;

            rect = EditorGUIUtility.GetMainWindowPosition();
            //Debug.Log(rect.ToString());

            //rect = EditorGUIUtility.PointsToPixels(rect);
            //Debug.Log(rect.ToString());

            var w1 = EditorWindow.focusedWindow;
            if (w1)
            {
                var m = w1.position;

                m.y = m.y - 100;

                if (Win32CursorPoint.TryGet(out var point))
                {
                    m.y = point.y - rect.height;
                }

                var r2 = GUIUtility.ScreenToGUIRect(m);
                //Debug.Log(r2.ToString());
                rect = r2;
            }

            //var w2 = EditorWindow.mouseOverWindow;
            //if (w2)
            //{
            //    var m = w2.position;
            //    var r2 = GUIUtility.ScreenToGUIRect(m);
            //    //Debug.Log(r2.ToString());
            //    rect = r2;
            //}

            //rect.y = rect.y - rect.height / 2;
            //rect.x = m.x - rect.x;
            //Debug.Log(rect.ToString());

            //var v3 = Input.mousePosition; //无效
            //Debug.Log(v3.ToString());

            //Debug.Log(rect.ToStringReflection());
            //rect.x = -1920 /2;
            //rect.y = -540;
            //rect.width = 500;
            //rect.height = 500;
            //Debug.Log(rect.ToStringReflection());
            menu.DropDown(rect);
        }
#endif

    }
}

namespace Megumin
{
    /// <summary>
    /// https://stackoverflow.com/questions/1316681/getting-mouse-position-in-c-sharp
    /// </summary>
    public struct Win32CursorPoint
    {
        public Win32CursorPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Win32CursorPoint(POINT point)
            {
                return new Win32CursorPoint(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Win32CursorPoint GetCursorPosition()
        {
            POINT lpPoint = default;

#if UNITY_EDITOR_WIN
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

#endif

            return lpPoint;
        }

        public static bool TryGet(out Win32CursorPoint point)
        {
            POINT lpPoint = default;
            bool success = false;
#if UNITY_EDITOR_WIN
            success = GetCursorPos(out lpPoint);
#endif
            point = lpPoint;
            return success;
        }
    }

}

