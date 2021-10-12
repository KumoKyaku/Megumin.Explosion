using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

#if DISABLE_SCROBJ_DRAWER

#else
[CustomPropertyDrawer(typeof(ScriptableObject), true)]
#endif
public class ScriptObjectDrawer_8F11D385 : PropertyDrawer
{
    static GUIStyle left = new GUIStyle("minibuttonleft");
    static GUIStyle right = new GUIStyle("minibuttonright");
    static Regex typeRegex = new Regex(@"^PPtr\<\$(.*)>$");

    string[] SupportNames;
    int index = 0;
    Type[] SupportTypes;
    HashSet<Type> allTypes;

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
                        allTypes.Add(ab.Support[i]);
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
                var path = AssetDatabase.GetAssetPath(obj);
                var oriName = Path.GetFileNameWithoutExtension(path);

                var newFileName = oriName.FileNameAddOne();
                path = path.ReplaceFileName(newFileName);

                AssetDatabase.CreateAsset(clone, path);
                AssetDatabase.Refresh();
                property.objectReferenceValue = clone;
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
                    ScriptableObject instance = null;
                    if (T != null)
                    {
                        instance = ScriptableObject.CreateInstance(T);
                    }
                    else
                    {
                        instance = ScriptableObject.CreateInstance(TName);
                    }

                    var path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
                    string dir = Root;
                    if (!string.IsNullOrEmpty(path))
                    {
                        dir = Path.GetDirectoryName(path);
                    }
                    else
                    {
                        var scene = EditorSceneManager.GetActiveScene();
                        if (scene.path != null)
                        {
                            dir = Path.GetDirectoryName(scene.path);
                        }
                    }
                    var instancePath = EditorUtility.SaveFilePanel("Create", dir, TName, "asset");

                    //if (instancePath.StartsWith(Application.dataPath))
                    //{
                    //    var t = instancePath.Replace(Application.dataPath, "");
                    //    instancePath = Path.Combine(@"/Assets", t);
                    //}

                    AssetDatabase.CreateAsset(instance, instancePath);
                    AssetDatabase.Refresh();
                    property.objectReferenceValue = instance;
                }
            }
        }
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
    private static void CreateInstanceAsset(SerializedProperty property, ScriptableObject instance)
    {
        var path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
        if (string.IsNullOrEmpty(path))
        {
            var root = PrefabUtility.GetOutermostPrefabInstanceRoot(property.serializedObject.targetObject);
            if (root)
            {
                var ori = PrefabUtility.GetCorrespondingObjectFromSource(root);
                path = AssetDatabase.GetAssetPath(ori);
            }
        }

        string dir = Root;
        if (!string.IsNullOrEmpty(path))
        {
            dir = Path.GetDirectoryName(path);
        }
        else
        {
            var scene = EditorSceneManager.GetActiveScene();
            if (scene.path != null)
            {
                dir = Path.GetDirectoryName(scene.path);
            }
        }

        var ex = ".asset";
        path = dir.CreateFileName($"{property.serializedObject.targetObject.name}_{instance.GetType().Name}", ex);

        AssetDatabase.CreateAsset(instance, path);
        AssetDatabase.Refresh();
        property.objectReferenceValue = instance;
    }
}


