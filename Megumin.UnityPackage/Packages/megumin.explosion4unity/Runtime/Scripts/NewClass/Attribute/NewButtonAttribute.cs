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
    /// 使用<see cref="SupportTypesAttribute"/>设置支持类型的搜索范围。
    /// <para><see cref="SerializeReference"/>不从<see cref="PropertyAttribute"/>继承，所以这个特性无法省略。</para>
    /// </summary>
    /// <remarks>
    /// 注意，截止到2023.2不能通过<see cref="SerializeReference"/>，同时支持序列化普通对象和unity对象切换，赋值时回报错
    /// </remarks>
    public class NewButtonAttribute : PropertyAttribute
    {
        public int LeftButtonWidth { get; set; } = 40;
        public int RightButtonWidth { get; set; } = 40;

        /// <summary>
        /// 是否在当前Inspector展开子嵌套对象
        /// </summary>
        public bool CanExpand { get; set; } = true;

        /// <summary>
        /// 指定一个bool值属性作为开关。并将Toggle绘制在外侧顶部。
        /// </summary>
        public string EnabledPropertyPath { get; set; } = "Enabled";

        public NewButtonAttribute()
        {

        }

        public NewButtonAttribute(int leftButtonWidth = 40, int rightButtonWidth = 40)
        {
            LeftButtonWidth = leftButtonWidth;
            RightButtonWidth = rightButtonWidth;
        }
    }

    /// <summary>
    /// 在面板绘制new clone 按钮
    /// </summary>
    [Obsolete("Use NewButtonAttribute instead.")]
    public interface INewCloneButton
    {

    }

    /// <summary>
    /// 使用<see cref="SupportTypesAttribute"/>设置支持类型的搜索范围。
    /// <para><see cref="SerializeReference"/>不从<see cref="PropertyAttribute"/>继承，所以这个特性无法省略。</para>
    /// </summary>
    [Obsolete("Use NewButtonAttribute instead.")]
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

//命名空间应该用Megumin.UnityEditor
//还是UnityEditor.Megumin
//UnityEditor.Megumin 导致无法访问Megumin根命名空间，需要加 global::Megumin
namespace UnityEditor.Megumin
{

#if UNITY_EDITOR

    /// <summary>
    /// 增加new,clone两个按钮
    /// </summary>
    /// 
#if !DISABLE_MEGUMIN_PROPERTYDRWAER
#if !DISABLE_SCROBJ_DRAWER
    //[CustomPropertyDrawer(typeof(Material), true)] //对Material无效
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
#endif
    //[CustomPropertyDrawer(typeof(INewCloneButton), true)]
    [CustomPropertyDrawer(typeof(NewButtonAttribute), true)]
    [CustomPropertyDrawer(typeof(SerializeReferenceNewButtonAttribute), true)]
#endif
    public partial class INewCloneButtonDrawer_72946648EEF14A43B837BAC45D14BFF3 : PropertyDrawer
    {
        static GUIStyle left = new GUIStyle("minibuttonleft");
        static GUIStyle right = new GUIStyle("minibuttonright");
        static GUIStyle TypeLabel = new GUIStyle("ObjectField");
        static GUIStyle TypeButton = new GUIStyle("NotificationText");
        static Regex typeRegex = new Regex(@"^PPtr\<\$(.*)>$");

        public class SaveTask
        {
            public Type T;
            public string TName;
            public string instancePath;
        }

        /// <summary>
        /// 多态序列化时，每个不同的类型都有自己的PropertyDrawer实例，所以这个index会每个实例有自己的值。
        /// 表现就是选项当前值会每次new都会变，没有什么好的办法。
        /// <para/> 而且必须CanCacheInspectorGUI为true，否则类型缓存和savetask都会失效。
        /// 也没办法使用静态LastIndex,因为多个draw同时工作，设置之后立刻会被List其他元素的draw使用。
        /// </summary>
        int DropMenuIndex = 0;

        /// <summary>
        /// 为每个序列化对象的每个path缓存index，同一个path共享index。
        /// </summary>
        public static Dictionary<(UnityEngine.Object Target, string Path), int> SelectedIndex = new();

        /// <summary>
        /// 代码中声明的成员类型
        /// </summary>
        public Type memberType;

        /// <summary>
        /// 代码中声明的成员类型名字，有时候类型取不到
        /// </summary>
        public string memberTypeName;
        string[] SupportNames;
        public Type[] SupportTypes;
        public HashSet<Type> allTypes;
        public SaveTask saveTask = null;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                return GetUnityObjectFieldPropertyHeight(property, label);
            }
            return EditorGUI.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
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

                if (instancePath.StartsWith(PathUtility.ProjectPath))
                {
                    instancePath = instancePath.Replace(PathUtility.ProjectPath, "");
                }

                AssetDatabase.CreateAsset(instance, instancePath);
                AssetDatabase.Refresh();
                property.objectReferenceValue = instance;
            }

            var leftButtonWidth = 40;
            var rightButtonWidth = 40;

            if (attribute is NewButtonAttribute newButtonAttribute)
            {
                leftButtonWidth = newButtonAttribute.LeftButtonWidth;
                rightButtonWidth = newButtonAttribute.RightButtonWidth;
            }

            //按钮和属性间隔
            var spaceWidth = 4;

            var propertyPosition = position;
            propertyPosition.width -= leftButtonWidth + rightButtonWidth + spaceWidth;

            var leftPosotion = position;
            leftPosotion.width = leftButtonWidth;
            leftPosotion.height = 18;
            leftPosotion.x = position.x + position.width - leftButtonWidth - rightButtonWidth;

            var rightPosition = position;
            rightPosition.width = rightButtonWidth;
            rightPosition.height = 18;
            rightPosition.x = position.x + position.width - rightButtonWidth;

            EditorGUI.BeginProperty(position, label, property);

            var propertyType = property.propertyType;

            if (propertyType == SerializedPropertyType.ManagedReference
                || propertyType == SerializedPropertyType.ObjectReference)
            {
                DrawReference(property, label, position, propertyPosition, leftPosotion, rightPosition);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            EditorGUI.EndProperty();
        }

        protected void DrawReference(SerializedProperty property, GUIContent label, Rect position, Rect propertyPosition, Rect leftPosotion, Rect rightPosition)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference
                && property.objectReferenceValue)
            {
                //当前已经存在unityObject对象。根据当前对象new clone即可，不用计算类型
                NewCloneCurrentUnityObject(property, label, position, propertyPosition, leftPosotion, rightPosition, property.objectReferenceValue);
            }
            else
            {
                CacheSupportType(property);
                if (SupportNames != null && SupportNames.Length > 0)
                {
                    //通过特性支持多个类型

                    //偏移两个像素，让UI更好看，不然左边会有一个空隙
                    var popPosition = rightPosition;
                    popPosition.width += 2;
                    popPosition.x -= 2;

                    var indexCacheKey = (property.serializedObject.targetObject, property.propertyPath);
                    SelectedIndex.TryGetValue(indexCacheKey, out var index);

                    var oldSelectedIndex = index;

                    var oldIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;

                    ///EditorGUI会自动应用缩进，导致Popup每个层级向右偏移15个单位
                    ///这里Popup默认不出来缩进
                    index = EditorGUI.Popup(popPosition, index, SupportNames);

                    EditorGUI.indentLevel = oldIndentLevel;

                    SelectedIndex[indexCacheKey] = index;

                    var targetType = SupportTypes[index];

                    if (oldSelectedIndex != index)
                    {
                        //这里打印日志，方便定为脚本文件位置。
                        //本来想在TypeLabel 上点击跳转的，但是不好实现OpenAsset，改为超链接。
                        if (targetType == null)
                        {
                            Debug.Log($"Select Type: null");
                        }
                        else
                        {
                            Debug.Log($"Select Type: {targetType.GetUnityProjectLink()}");
                        }

                        //切换时自动切换对象
                        if (property.propertyType == SerializedPropertyType.ManagedReference)
                        {
                            ChangeManagedReferenceInstance(property, targetType);
                        }
                    }

                    //new button 放在上面，不然new时会与Expanded折叠功能冲突。
                    if (GUI.Button(leftPosotion, "New", left))
                    {
                        if (property.propertyType == SerializedPropertyType.ManagedReference)
                        {
                            ChangeManagedReferenceInstance(property, targetType);
                        }
                        else
                        {
                            CreateUnityObjectInstance(property, targetType);
                        }
                    }

                    if (property.propertyType == SerializedPropertyType.ManagedReference)
                    {
                        var textPosition = propertyPosition;
                        textPosition.x += EditorGUIUtility.labelWidth + 2;
                        //textPosition.width -= EditorGUIUtility.labelWidth;
                        textPosition.xMax = propertyPosition.xMax;
                        textPosition.height = 18;

                        bool contentEnabled = true;
                        #region 绘制额外的开启关闭Toggle

                        //当序列化对象允许设置开启关闭时，将开启关闭开关绘制在外层
                        var enabledPath = "Enabled";
                        if (attribute is NewButtonAttribute newButtonAttribute)
                        {
                            enabledPath = newButtonAttribute.EnabledPropertyPath;
                        }

                        SerializedProperty toggle = property.FindPropertyRelative(enabledPath);
                        if (toggle != null)
                        {
                            var toggleRect = textPosition;
                            toggleRect.width = 18;

                            EditorGUI.BeginProperty(toggleRect, GUIContent.none, toggle);
                            toggle.boolValue = GUI.Toggle(toggleRect, toggle.boolValue, GUIContent.none);
                            contentEnabled = toggle.boolValue;
                            EditorGUI.EndProperty();

                            textPosition.x += 20;
                            textPosition.width -= 20;
                        }

                        #endregion

                        //绘制当前类型按钮，点击选中类型脚本，双击打开类型脚本
                        using (new UnityEditor.EditorGUI.DisabledScope(!contentEnabled))
                        {
                            //绘制一个Disabled风格的Field，再在上面绘制一个button
                            //直接绘制button在里面没办法点击
                            GUI.Label(textPosition, SupportNames[index], TypeLabel);
                        }

                        if (GUI.Button(textPosition, "", TypeButton))//按钮不显示名字
                        {
                            ClickTypeLable(targetType);
                        }

                        using (new EditorGUI.DisabledScope(!contentEnabled))
                        {
                            if (!property.isExpanded)
                            {
                                EditorGUI.PropertyField(propertyPosition, property, label, true);
                            }
                            else
                            {
                                EditorGUI.PropertyField(position, property, label, true);
                            }
                        }
                    }
                    else
                    {
                        DrawUnityObjectField(property, label, position, propertyPosition, targetType);
                    }
                }
                else
                {
                    //没有设置特性
                    //通常这里是UnityObject类型。如果是托管类型，SupportNames肯定至少有2个值。

                    EditorGUI.PropertyField(propertyPosition, property, label);

                    CacheMemberType(property);
                    if (GUI.Button(rightPosition, "Save", right))
                    {
                        string dir = GetDir(property);
                        var fileName = $"{property.serializedObject.targetObject.name}_{memberTypeName}";
                        fileName = fileName.AutoFileName(dir, ".asset",
                                          EditorSettings.gameObjectNamingScheme.ToString(),
                                          EditorSettings.gameObjectNamingDigits);
                        var instancePath = EditorUtility.SaveFilePanel("Create", dir, fileName, "asset");
                        if (!string.IsNullOrEmpty(instancePath))
                        {
                            instancePath = Path.GetFullPath(instancePath);
                            saveTask = new SaveTask() { instancePath = instancePath, T = memberType, TName = memberTypeName };
                        }
                        GUIUtility.ExitGUI();
                    }

                    if (GUI.Button(leftPosotion, "New", left))
                    {
                        // 识别是不是按住Alt，进入subAsset模式。
                        if (memberType != null)
                        {
                            CreateUnityObjectInstance(property, memberType);
                        }
                        else
                        {
                            CreateUnityObjectInstance(property, memberTypeName);
                        }
                    }
                }
            }
        }

        float LastTypeLableClickTime = -100;
        public void ClickTypeLable(Type targetType)
        {
            var delta = Time.realtimeSinceStartup - LastTypeLableClickTime;
            LastTypeLableClickTime = Time.realtimeSinceStartup;
            //Debug.Log(global::Megumin.Utility.ToStringReflection<Time>());

            if (targetType != null)
            {

                if (delta <= 0.5f)
                {
                    //Debug.Log($"Double----{Event.current.clickCount}");
#if MEGUMIN_Common
                    targetType.OpenScript();
#endif

                }
                else
                {
                    //Debug.Log($"Click----{Event.current.clickCount}");
#if MEGUMIN_Common
                    targetType.PingScript();
#endif

                }
            }
        }

        public void NewCloneCurrentUnityObject(SerializedProperty property, GUIContent label, Rect position, Rect propertyPosition, Rect leftPosotion, Rect rightPosition, UnityEngine.Object obj)
        {
            Type targetType = obj.GetType();
            //EditorGUI.PropertyField(propertyPosition, property, label);
            DrawUnityObjectField(property, label, position, propertyPosition, targetType);

            if (GUI.Button(rightPosition, "Clone", right))
            {
                var clone = UnityEngine.Object.Instantiate(obj);
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
                    CreateUnityObjectAsset(property, clone);
                }
            }

            if (GUI.Button(leftPosotion, "New", left))
            {
                CreateUnityObjectInstance(property, targetType);
            }
        }

        /// <summary>
        /// 求当前成员代码声明时的类型
        /// </summary>
        /// <param name="property"></param>
        public void CacheMemberType(SerializedProperty property)
        {
            if (!string.IsNullOrEmpty(memberTypeName))
            {
                return;
            }

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

            memberTypeName = resultTName;
            memberType = resultType;
        }

        /// <summary>
        /// 缓存支持的类型
        /// </summary>
        public void CacheSupportType(SerializedProperty property)
        {
            if (allTypes == null)
            {
                CacheMemberType(property);
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

                ///多态序列化强制搜索所有子类型 
                var forceReference = property.propertyType == SerializedPropertyType.ManagedReference;

                ////Todo 反射查找有没有SerializeReference
                //var attri = fieldInfo.GetCustomAttribute<SerializeReference>();
                //{
                //    forceReference = attri != null;
                //}

                this.fieldInfo.CollectSupportType(allTypes,
                                                  AssemblyFilter,
                                                  forceReference,
                                                  extraCheck: extraAss);

                if (property.propertyType == SerializedPropertyType.ManagedReference)
                {
                    //一般类型，因为没办法设置为null，在下拉菜单中最后加入null类型，用于清空当前值
                    int index = 0;
                    SupportNames = new string[allTypes.Count + 1];
                    SupportTypes = new Type[allTypes.Count + 1];

                    var currentType = property.managedReferenceValue?.GetType();

                    var indexCacheKey = (property.serializedObject.targetObject, property.propertyPath);
                    SelectedIndex[indexCacheKey] = allTypes.Count;

                    foreach (var item in allTypes)
                    {
                        SupportNames[index] = item.Name;
                        SupportTypes[index] = item;

                        if (currentType == item)
                        {
                            SelectedIndex[indexCacheKey] = index;
                        }
                        index++;
                    }

                    SupportNames[allTypes.Count] = "Null";
                    SupportTypes[allTypes.Count] = null;
                }
                else
                {
                    //Unity对象会绘制为ObjectField，不用再 下拉列表中加null
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
        }
    }

    //Expanded  UnityObject 展开。托管对象时自动展开的，不用处理
    //从 https://github.com/dbrizov/NaughtyAttributes 修改
    partial class INewCloneButtonDrawer_72946648EEF14A43B837BAC45D14BFF3
    {
        public float GetUnityObjectFieldPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var canExpand = (attribute as NewButtonAttribute)?.CanExpand ?? true;

            if (canExpand && property.isExpanded && property.objectReferenceValue != null)
            {
                using (SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue))
                {
                    float totalHeight = EditorGUIUtility.singleLineHeight;

                    using (var iterator = serializedObject.GetIterator())
                    {
                        if (iterator.NextVisible(true))
                        {
                            do
                            {
                                SerializedProperty childProperty = serializedObject.FindProperty(iterator.name);
                                if (childProperty.name.Equals("m_Script", System.StringComparison.Ordinal))
                                {
                                    continue;
                                }

                                bool visible = true;
                                if (!visible)
                                {
                                    continue;
                                }

                                float height = EditorGUI.GetPropertyHeight(childProperty, includeChildren: true);
                                totalHeight += height + EditorGUIUtility.standardVerticalSpacing;
                            }
                            while (iterator.NextVisible(false));
                        }
                    }

                    totalHeight += EditorGUIUtility.standardVerticalSpacing;
                    return totalHeight;
                }
            }
            return EditorGUI.GetPropertyHeight(property, label);
        }

        public void DrawUnityObjectField(SerializedProperty property,
                                                GUIContent label,
                                                Rect position,
                                                Rect propertyPosition,
                                                Type targetType)
        {
            var canExpand = (attribute as NewButtonAttribute)?.CanExpand ?? true;
            if (canExpand && property.propertyType == SerializedPropertyType.ObjectReference
                && property.objectReferenceValue != null)
            {
                // Draw a foldout
                Rect foldoutRect = new Rect()
                {
                    x = position.x,
                    y = position.y,
                    width = EditorGUIUtility.labelWidth,
                    height = EditorGUIUtility.singleLineHeight
                };

                //使用GUIContent.none，避免Foldout绘制多余的label
                property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, GUIContent.none, toggleOnLabelClick: true);

                // Draw the scriptable object field
                var objPos = propertyPosition;
                objPos.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.ObjectField(objPos, property, targetType, label);

                // Draw the child properties
                if (property.isExpanded)
                {
                    DrawChildProperties(position, property);
                }
            }
            else
            {
                EditorGUI.ObjectField(propertyPosition, property, targetType, label);
            }
        }

        public static void DrawChildProperties(Rect rect, SerializedProperty property)
        {
            if (property.objectReferenceValue == null)
            {
                return;
            }

            Rect boxRect = new Rect()
            {
                x = 0.0f,
                y = rect.y + EditorGUIUtility.singleLineHeight,
                width = rect.width * 2.0f,
                height = rect.height - EditorGUIUtility.singleLineHeight
            };

            GUI.Box(boxRect, GUIContent.none);

            using (new EditorGUI.IndentLevelScope())
            {
                SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
                serializedObject.Update();

                using (var iterator = serializedObject.GetIterator())
                {
                    float yOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    if (iterator.NextVisible(true))
                    {
                        do
                        {
                            SerializedProperty childProperty = serializedObject.FindProperty(iterator.name);
                            if (childProperty.name.Equals("m_Script", System.StringComparison.Ordinal))
                            {
                                continue;
                            }

                            bool visible = true;
                            if (!visible)
                            {
                                continue;
                            }

                            float childHeight = EditorGUI.GetPropertyHeight(childProperty, includeChildren: true); ;
                            Rect childRect = new Rect()
                            {
                                x = rect.x,
                                y = rect.y + yOffset,
                                width = rect.width,
                                height = childHeight
                            };

                            //EditorGUI.BeginProperty(childRect, GUIContent.none, childProperty);
                            EditorGUI.PropertyField(childRect, childProperty, true);
                            //EditorGUI.EndProperty();

                            yOffset += childHeight + EditorGUIUtility.standardVerticalSpacing;
                        }
                        while (iterator.NextVisible(false));
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
    }


    //tool
    partial class INewCloneButtonDrawer_72946648EEF14A43B837BAC45D14BFF3
    {
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

        //多态序列化历史值缓存
        //一个路径代表一个位置，这个位置在多个类型中切换，每个类型保存一个历史值
        public class PathManagedReferencePair : Dictionary<string, Dictionary<Type, object>> { }
        public class HistoryCache : Dictionary<UnityEngine.Object, PathManagedReferencePair>
        {
            public bool TryGetOld(SerializedProperty property, Type type, out object old)
            {
                old = null;
                if (type == null)
                {
                    return false;
                }
                if (TryGetValue(property.serializedObject.targetObject, out var pair))
                {
                    if (pair.TryGetValue(property.propertyPath, out var TVPair))
                    {
                        return TVPair.TryGetValue(type, out old);
                    }
                }

                return false;
            }

            public void Cache(SerializedProperty property, object newObj)
            {
                if (newObj == null)
                {
                    return;
                }

                if (!TryGetValue(property.serializedObject.targetObject, out var pair))
                {
                    pair = new PathManagedReferencePair();
                    this[property.serializedObject.targetObject] = pair;
                }

                if (!pair.TryGetValue(property.propertyPath, out var TVPair))
                {
                    TVPair = new Dictionary<Type, object>();
                    pair[property.propertyPath] = TVPair;
                }

                TVPair[newObj.GetType()] = newObj;
            }
        }

        /// <summary>
        /// 可能内存泄露，但是管不了那么多了
        /// </summary>
        static HistoryCache History = new HistoryCache();

        /// <summary>
        /// 切换新的托管对象
        /// </summary>
        /// <param name="property"></param>
        /// <param name="type"></param>
        private static void ChangeManagedReferenceInstance(SerializedProperty property, Type type, bool forceNew = false)
        {
            object newObj = null;
            if (type == null)
            {
                //Debug.Log($"SerializeReference: null");
            }
            else
            {
                bool isChangeType = property.managedReferenceValue?.GetType() != type;

                //Debug.Log($"SerializeReference: {type.GetUnityProjectLink()}");
                if (History.TryGetOld(property, type, out var cache)
                    && property.managedReferenceValue != cache) //切换时使用旧值，没切换，保持当前值是旧值是，表示new，强制new新值
                {
                    //存在旧的值就不用new了
                    newObj = cache;
                }
                else
                {
                    //创建新的引用值
                    if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                    {
                        //当类型是unity类型，使用unity创建
                        newObj = CreateUnityObjectInstance(property, type);
                    }
                    else
                    {
                        newObj = Activator.CreateInstance(type);
                    }

                    if (property.managedReferenceValue != null
                        && isChangeType)
                    {
                        //当前对象不是相同类型，表示从其他类型切换而来
                        //新的引用值，将当前值尽可能复制字段到新值
                        var source = property.managedReferenceValue;
                        source.SimilarityCopyTo(newObj);
                    }
                    else
                    {
                        //相同类型二次new，
                        //这里表示强制new 新对象，不做任何操作。
                    }
                }
            }

            //切换前缓存当前值，否则第一显示对象，切换后，初始值没有被缓存。
            History.Cache(property, property.managedReferenceValue);

            if (newObj is UnityEngine.Object unityObj)
            {
                //当多态序列化时unity对象时赋值到objectReferenceValue
                //还是不能同时支持unity对象和接口多态序列化。这里赋值回报错，并不能正确执行。
                property.objectReferenceValue = unityObj;
            }
            else
            {
                property.managedReferenceValue = newObj;
            }

            History.Cache(property, newObj);
        }

        private static UnityEngine.Object CreateUnityObjectInstance(SerializedProperty property, string type)
        {
            var instance = ScriptableObject.CreateInstance(type);
            return CreateUnityObjectAsset(property, instance);
        }

        private static UnityEngine.Object CreateUnityObjectInstance(SerializedProperty property, Type type)
        {
            UnityEngine.Object instance = null;
            if (typeof(ScriptableObject).IsAssignableFrom(type))
            {
                instance = ScriptableObject.CreateInstance(type);
            }
            else if (typeof(Component).IsAssignableFrom(type))
            {
                if (property.serializedObject.targetObject is GameObject gameObject)
                {
                    instance = gameObject.AddComponent(type);
                }
            }

            if (!instance)
            {
                var constructor = type.GetConstructors()[0];
                var count = constructor.GetParameters().Length;
                object[] args = null;
                switch (count)
                {
                    case 0:
                        break;
                    case 1:
                        args = new object[] { null };
                        break;
                    case 2:
                        args = new object[] { null, null };
                        break;
                    case 3:
                        args = new object[] { null, null, null };
                        break;
                    case 4:
                        args = new object[] { null, null, null, null };
                        break;
                    default:
                        break;
                }

                instance = constructor.Invoke(args) as UnityEngine.Object;
            }

            return CreateUnityObjectAsset(property, instance);
        }

        const string Root = @"Assets";
        private static UnityEngine.Object CreateUnityObjectAsset(SerializedProperty property, UnityEngine.Object instance)
        {
            string dir = GetDir(property);

            if (Event.current.alt || ClonePathModeSetting.PathMode == ClonePathModeSetting.ClonePathMode.SubAsset)
            {
                var success = TryCreateSubAsset(property, instance, dir, out var newAsset);
                if (success)
                {
                    return newAsset;
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
            return instance;
        }

        public static bool TryCreateSubAsset(
            SerializedProperty property,
            UnityEngine.Object instance,
            string dir,
            out UnityEngine.Object newAsset)
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
                newAsset = instance;
                property.objectReferenceValue = instance;
                property.serializedObject.targetObject.AssetDataSetDirty();
                AssetDatabase.SaveAssetIfDirty(property.serializedObject.targetObject);
            }
            catch (Exception)
            {
                newAsset = null;
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




