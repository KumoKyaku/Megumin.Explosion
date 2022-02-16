using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

/// <summary>
/// 在面板绘制new clone 按钮
/// </summary>
public interface INewCloneButton
{

}


#if UNITY_EDITOR

public static class ClonePathModeSetting
{
    public enum ClonePathMode
    {
        ReferencePath = 0,
        ParentPath = 1,
    }

    /// <summary>
    /// clone时使用父路径还是克隆对象路径.
    /// </summary>
    public static ClonePathMode PathMode { get; set; } = ClonePathMode.ParentPath;
}

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
    int index = 0;
    Type[] SupportTypes;
    HashSet<Type> allTypes;
    SaveTask saveTask = null;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (allTypes == null)
        {
            allTypes = new HashSet<Type>();
            var customattributes = this.fieldInfo.GetCustomAttributes(true);
            var abs = from cab in customattributes
                      where cab is Megumin.SupportTypesAttribute
                      let sa = cab as Megumin.SupportTypesAttribute
                      select sa;

            foreach (var ab in abs)
            {
                if (ab != null && ab.Support != null)
                {
                    for (int i = 0; i < ab.Support.Length; i++)
                    {
                        void TryAddType(Type type)
                        {
                            if (!ab.AllowInterface && type.IsInterface)
                            {
                                return;
                            }

                            if (!ab.AllowAbstract && type.IsAbstract)
                            {
                                return;
                            }

                            if (!ab.AllowGenericType && type.IsGenericType)
                            {
                                return;
                            }

                            allTypes.Add(type);
                        }

                        var type = ab.Support[i];

                        TryAddType(type);

                        if (ab.IncludeChildInSameAssembly)
                        {
                            Assembly assembly = type.Assembly;
                            foreach (var item in assembly.GetTypes())
                            {
                                if (type.IsAssignableFrom(item))
                                {
                                    TryAddType(item);
                                }
                            }
                        }
                    }
                }
            }

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

        if (!property.type.StartsWith("PPtr"))
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        DrawPPtrType(property, label, propertyPosition, leftPosotion, rightPosition);
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

            if (GUI.Button(leftPosotion, "New", left))
            {
                CreateInstance(property, obj.GetType());
            }

            if (GUI.Button(rightPosition, "Clone", right))
            {
                var clone = ScriptableObject.Instantiate(obj);
                if (ClonePathModeSetting.PathMode == ClonePathModeSetting.ClonePathMode.ReferencePath)
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
        }
        else
        {
            if (SupportNames != null && SupportNames.Length > 0)
            {
                //通过特性支持多个类型

                var popPosition = rightPosition;
                popPosition.width = 55;
                popPosition.x -= 15;

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

                if (GUI.Button(leftPosotion, "New", left))
                {
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

                if (GUI.Button(rightPosition, "Save", right))
                {
                    var (T, TName) = CalTargetType();
                    string dir = GetDir(property);
                    var fileName = $"{property.serializedObject.targetObject.name}_{TName}";
                    fileName = fileName.AutoFileName(dir, ".asset");
                    var instancePath = EditorUtility.SaveFilePanel("Create", dir, fileName, "asset");
                    instancePath = Path.GetFullPath(instancePath);
                    saveTask = new SaveTask() { instancePath = instancePath, T = T, TName = TName };
                    GUIUtility.ExitGUI();
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

        var ex = ".asset";
        var path = dir.CreateFileName($"{property.serializedObject.targetObject.name}_{instance.GetType().Name}",
                                      ex,
                                      EditorSettings.gameObjectNamingScheme.ToString(),
                                      EditorSettings.gameObjectNamingDigits);

        AssetDatabase.CreateAsset(instance, path);
        AssetDatabase.Refresh();
        property.objectReferenceValue = instance;
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


