using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using Megumin;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Megumin
{
    /// <summary>
    /// 在面板绘制new clone 按钮
    /// </summary>
    public interface INewCloneButton
    {

    }

    /// <summary>
    /// 使用<see cref="SupportTypesAttribute"/>设置支持类型的搜索范围。
    /// <para><see cref="SerializeReference"/>不从<see cref="PropertyAttribute"/>继承，所以这个特性无法省略。</para>
    /// </summary>
    public class SerializeReferenceNewButtonAttribute : PropertyAttribute
    {

    }

    public static class ClonePathModeSetting
    {
        public enum ClonePathMode
        {
            ReferencePath = 0,
            ParentPath = 1,
            /// <summary>
            /// 或者按住Alt键创建，自动进入SubAsset模式。
            /// </summary>
            SubAsset = 2,
        }

        /// <summary>
        /// clone时使用父路径还是克隆对象路径.
        /// </summary>
        public static ClonePathMode PathMode { get; set; } = ClonePathMode.ParentPath;
    }
}

namespace UnityEditor.Megumin
{

#if UNITY_EDITOR

    /// <summary>
    /// 增加new,clone两个按钮.对Material无效.想增加SubAsset功能,但是发现没有必要.
    /// </summary>
    /// 
#if !DISABLE_MEGUMIN_PROPERTYDRWAER
#if !DISABLE_SCROBJ_DRAWER
    //[CustomPropertyDrawer(typeof(Material), true)]
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
#endif
    [CustomPropertyDrawer(typeof(INewCloneButton), true)]
    [CustomPropertyDrawer(typeof(SerializeReferenceNewButtonAttribute), true)]
#endif
    public class INewCloneButtonDrawer_8F11D385 : PropertyDrawer
    {
        static GUIStyle left = new GUIStyle("minibuttonleft");
        static GUIStyle right = new GUIStyle("minibuttonright");
        static Regex typeRegex = new Regex(@"^PPtr\<\$(.*)>$");

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        string[] SupportNames;
        /// <summary>
        /// 多态序列化时，每个不同的类型都有自己的PropertyDrawer实例，所以这个index会每个实例有自己的值。
        /// 表现就是选项当前值会每次new都会变，没有什么好的办法。
        /// <para/> 而且必须CanCacheInspectorGUI为true，否则类型缓存和savetask都会失效。
        /// </summary>
        int index = 0;
        Type[] SupportTypes;
        HashSet<Type> allTypes;
        SaveTask saveTask = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyPosition = position;
            propertyPosition.width -= 86;

            var buttonPosition = position;
            buttonPosition.width = 80;
            buttonPosition.x += position.width - 80;


            var leftPosotion = buttonPosition;
            leftPosotion.width = 40;
            var rightPosition = buttonPosition;
            rightPosition.width = 40;
            rightPosition.x += 40;

            if (property.type.StartsWith("PPtr"))
            {
                CacheSupportType(property);
                DrawPPtrType(property, label, propertyPosition, leftPosotion, rightPosition);
            }
            else if (attribute is SerializeReferenceNewButtonAttribute add)
            {
                CacheSupportType(property, true);
                DrawSerializeReference(property, label, position, propertyPosition, leftPosotion, rightPosition);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }
        }

        /// <summary>
        /// 缓存支持的类型
        /// </summary>
        public void CacheSupportType(SerializedProperty property, bool collectSelfTypeIfNoAttribute = false)
        {
            if (allTypes == null)
            {
                allTypes = new HashSet<Type>();

                //额外检测SO对象本身类型所在程序集
                HashSet<Assembly> extraAss = new HashSet<Assembly>();
                if (property.serializedObject.targetObjects != null)
                {
                    foreach (var item in property.serializedObject.targetObjects)
                    {
                        extraAss.Add(item.GetType().Assembly);
                    }
                }

                this.fieldInfo.CollectSupportType(allTypes,
                                                  AssemblyFilter,
                                                  collectSelfTypeIfNoAttribute,
                                                  extraCheck: extraAss);

                int index = 0;
                SupportNames = new string[allTypes.Count];
                SupportTypes = new Type[allTypes.Count];
                foreach (var item in allTypes)
                {
                    SupportNames[index] = item.Name;
                    SupportTypes[index] = item;
                    index++;
                }
            }
        }

        public bool AssemblyFilter(Assembly assembly)
        {
            //过滤掉一些，不然肯能太卡
            var assName = assembly.FullName;
            if (assName.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //可以通过这个宏来强行搜索unity中的类型
#if !SCROBJ_DRAWER_FORCEDSEARCH_UNITY
            if (assName.StartsWith("Unity", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
#endif

            return true;
        }

        public void DrawSerializeReference(SerializedProperty property, GUIContent label, Rect position, Rect propertyPosition, Rect leftPosotion, Rect rightPosition)
        {
            if (leftPosotion.height > 18)
            {
                leftPosotion.height = 18;
                rightPosition.height = 18;
            }

            if (SupportNames != null && SupportNames.Length > 0)
            {
                //通过特性支持多个类型

                //偏移两个像素，让UI更好看，不然左边会有一个空隙
                var popPosition = rightPosition;
                popPosition.width += 2;
                popPosition.x -= 2;
                index = EditorGUI.Popup(popPosition, index, SupportNames);

                var targetType = SupportTypes[index];

                if (GUI.Button(leftPosotion, "New", left))
                {
                    var newObj = Activator.CreateInstance(targetType);
                    var source = property.managedReferenceValue;
                    source.SimilarityCopyTo(newObj);
                    property.managedReferenceValue = newObj;
                }
            }

            if (!property.isExpanded)
            {
                EditorGUI.PropertyField(propertyPosition, property, label, true);
                using (new UnityEditor.EditorGUI.DisabledGroupScope(true))
                {
                    var textPosition = propertyPosition;
                    textPosition.x += EditorGUIUtility.labelWidth;
                    textPosition.width -= EditorGUIUtility.labelWidth;
                    EditorGUI.TextField(textPosition, property.managedReferenceValue?.ToString());
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        protected void DrawPPtrType(SerializedProperty property, GUIContent label, Rect propertyPosition, Rect leftPosotion, Rect rightPosition)
        {
            if (saveTask != null)
            {
                var T = saveTask.T;
                var TName = saveTask.TName;
                var instancePath = saveTask.instancePath;
                saveTask = null;

                ScriptableObject instance = null;
                if (T != null)
                {
                    instance = ScriptableObject.CreateInstance(T);
                }
                else
                {
                    instance = ScriptableObject.CreateInstance(TName);
                }

                if (instancePath.StartsWith(MeguminUtility4Unity.ProjectPath))
                {
                    instancePath = instancePath.Replace(MeguminUtility4Unity.ProjectPath, "");
                }

                AssetDatabase.CreateAsset(instance, instancePath);
                AssetDatabase.Refresh();
                property.objectReferenceValue = instance;
            }

            var obj = property.objectReferenceValue;
            if (obj)
            {
                EditorGUI.PropertyField(propertyPosition, property, label);

                if (GUI.Button(rightPosition, "Clone", right))
                {
                    var clone = ScriptableObject.Instantiate(obj);
                    if (!Event.current.alt && ClonePathModeSetting.PathMode == ClonePathModeSetting.ClonePathMode.ReferencePath)
                    {
                        var path = AssetDatabase.GetAssetPath(obj);
                        var oriName = Path.GetFileNameWithoutExtension(path);

                        var newFileName = oriName.FileNameAddOne();
                        path = path.ReplaceFileName(newFileName);

                        AssetDatabase.CreateAsset(clone, path);
                        AssetDatabase.Refresh();
                        property.objectReferenceValue = clone;
                    }
                    else
                    {
                        CreateInstanceAsset(property, clone);
                    }
                }

                if (GUI.Button(leftPosotion, "New", left))
                {
                    CreateInstance(property, obj.GetType());
                }
            }
            else
            {
                if (SupportNames != null && SupportNames.Length > 0)
                {
                    //通过特性支持多个类型

                    //偏移两个像素，让UI更好看，不然左边会有一个空隙
                    var popPosition = rightPosition;
                    popPosition.width += 2;
                    popPosition.x -= 2;
                    index = EditorGUI.Popup(popPosition, index, SupportNames);

                    var targetType = SupportTypes[index];
                    EditorGUI.ObjectField(propertyPosition, property, targetType, label);

                    if (GUI.Button(leftPosotion, "New", left))
                    {
                        CreateInstance(property, targetType);
                    }
                }
                else
                {
                    //没有设置特性
                    EditorGUI.PropertyField(propertyPosition, property, label);

                    (Type T, string TName) CalTargetType()
                    {
                        var fieldType = fieldInfo.FieldType;
                        Type resultType = null;
                        string resultTName = null;
                        if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            var type = fieldType.GetGenericArguments()[0];
                            resultType = type;
                        }
                        else if (fieldType.IsSubclassOf(typeof(Array)))
                        {
                            var type = fieldType.GetElementType();
                            resultType = type;
                        }
                        else
                        {
                            var ret = typeRegex.Match(property.type);
                            if (ret.Success)
                            {
                                if (ret.Groups[1].Value == fieldType.Name)
                                {
                                    resultType = fieldType;
                                }
                                else
                                {
                                    resultTName = ret.Groups[1].Value;
                                }
                            }
                            else
                            {
                                resultType = fieldType;
                            }
                        }

                        if (resultType != null)
                        {
                            resultTName = resultType.Name;
                        }

                        return (resultType, resultTName);
                    }

                    if (GUI.Button(rightPosition, "Save", right))
                    {
                        var (T, TName) = CalTargetType();
                        string dir = GetDir(property);
                        var fileName = $"{property.serializedObject.targetObject.name}_{TName}";
                        fileName = fileName.AutoFileName(dir, ".asset",
                                          EditorSettings.gameObjectNamingScheme.ToString(),
                                          EditorSettings.gameObjectNamingDigits);
                        var instancePath = EditorUtility.SaveFilePanel("Create", dir, fileName, "asset");
                        if (!string.IsNullOrEmpty(instancePath))
                        {
                            instancePath = Path.GetFullPath(instancePath);
                            saveTask = new SaveTask() { instancePath = instancePath, T = T, TName = TName };
                        }
                        GUIUtility.ExitGUI();
                    }

                    if (GUI.Button(leftPosotion, "New", left))
                    {
                        // 识别是不是按住Alt，进入subAsset模式。
                        var (T, TName) = CalTargetType();
                        if (T != null)
                        {
                            CreateInstance(property, T);
                        }
                        else
                        {
                            CreateInstance(property, TName);
                        }
                    }
                }
            }
        }

        class SaveTask
        {
            public Type T;
            public string TName;
            public string instancePath;
        }

        private static void CreateInstance(SerializedProperty property, string type)
        {
            var instance = ScriptableObject.CreateInstance(type);
            CreateInstanceAsset(property, instance);
        }

        private static void CreateInstance(SerializedProperty property, Type type)
        {
            var instance = ScriptableObject.CreateInstance(type);
            CreateInstanceAsset(property, instance);
        }

        const string Root = @"Assets";
        private static void CreateInstanceAsset(SerializedProperty property, UnityEngine.Object instance)
        {
            string dir = GetDir(property);

            if (Event.current.alt || ClonePathModeSetting.PathMode == ClonePathModeSetting.ClonePathMode.SubAsset)
            {
                var success = CreateSubAsset(property, instance, dir);
                if (success)
                {
                    return;
                }
                else
                {
                    Debug.LogError($"创建SubAsset失败，使用普通模式创建。");
                }
            }

            var ex = ".asset";
            var path = dir.CreateFileName($"{property.serializedObject.targetObject.name}_{instance.GetType().Name}",
                                          ex,
                                          EditorSettings.gameObjectNamingScheme.ToString(),
                                          EditorSettings.gameObjectNamingDigits);

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.Refresh();
            property.objectReferenceValue = instance;
        }

        public static bool CreateSubAsset(SerializedProperty property, UnityEngine.Object instance, string dir)
        {
            try
            {
                AssetDatabase.AddObjectToAsset(instance, property.serializedObject.targetObject);

                //虽然是子对象，名字还是要带上父名字。其他位置选择SO资源的时候，能看见子对象却看不见父，不是全名不方便。
                var ex = ".asset";
                var path = dir.CreateFileName($"{property.serializedObject.targetObject.name}_{instance.GetType().Name}",
                                              ex,
                                              EditorSettings.gameObjectNamingScheme.ToString(),
                                              EditorSettings.gameObjectNamingDigits);

                var tempName = Path.GetFileNameWithoutExtension(path);

                string assetPath = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
                var allAsset = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                while (allAsset.Any(a => a.name == tempName))
                {
                    tempName = tempName.FileNameAddOne(EditorSettings.gameObjectNamingScheme.ToString(),
                                              EditorSettings.gameObjectNamingDigits);
                }
                instance.name = tempName;
                //AssetDatabase.ImportAsset(assetPath);
                property.objectReferenceValue = instance;
                property.serializedObject.targetObject.AssetDataSetDirty();
                AssetDatabase.SaveAssetIfDirty(property.serializedObject.targetObject);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 取得对象在资源路径.
        /// 如果是场景中的对象,取得prefab和变体的资源路径.
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string GetPathFixed(UnityEngine.Object @object)
        {
            var path = AssetDatabase.GetAssetPath(@object);
            if (string.IsNullOrEmpty(path))
            {
                var root = PrefabUtility.GetOutermostPrefabInstanceRoot(@object);
                if (root)
                {
                    var ori = PrefabUtility.GetCorrespondingObjectFromSource(root);
                    path = AssetDatabase.GetAssetPath(ori);
                }
            }

            return path;
        }

        /// <summary>
        /// 安全处理对象路径.
        /// 如果路径为空,返回当前场景的资源路径.
        /// 如果场景为保存,返回根路径.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string SafeObjectDir(string path)
        {
            string dir = Root;
            if (!string.IsNullOrEmpty(path))
            {
                dir = Path.GetDirectoryName(path);
            }
            else
            {
                var scene = EditorSceneManager.GetActiveScene();
                if (!string.IsNullOrEmpty(scene.path))
                {
                    dir = Path.GetDirectoryName(scene.path);
                }
            }

            return dir;
        }

        private static string GetDir(SerializedProperty property)
        {
            var path = GetPathFixed(property.serializedObject.targetObject);
            return SafeObjectDir(path);
        }
    }


#endif
}




