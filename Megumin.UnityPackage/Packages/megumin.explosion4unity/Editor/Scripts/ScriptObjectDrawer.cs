using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ScriptableObject), true)]
public class ScriptObjectDrawer_8F11D385 : PropertyDrawer
{
    static GUIStyle left = new GUIStyle("minibuttonleft");
    static GUIStyle right = new GUIStyle("minibuttonright");
    static Regex typeRegex = new Regex(@"^PPtr\<\$(.*)>$");

    string[] SupportNames;
    int index = 0;
    Type[] SupportTypes;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (SupportNames == null)
        {
            var customattributes = this.fieldInfo.GetCustomAttributes(true);
            Megumin.SupportTypesAttribute ab = customattributes.FirstOrDefault(e => e is Megumin.SupportTypesAttribute) as Megumin.SupportTypesAttribute;
            if (ab != null)
            {
                SupportNames = new string[ab.Support.Length];
                SupportTypes = ab.Support;
                for (int i = 0; i < SupportTypes.Length; i++)
                {
                    SupportNames[i] = SupportTypes[i].Name;
                }
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
                CreateInstance(property, obj.GetType().Name);
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
                    CreateInstance(property, targetType.Name);
                }
            }
            else
            {
                //没有设置特性
                EditorGUI.PropertyField(propertyPosition, property, label);

                if (GUI.Button(leftPosotion, "New", left))
                {
                    var ret = typeRegex.Match(property.type);
                    if (ret.Success)
                    {
                        CreateInstance(property, ret.Groups[1].Value);
                    }
                }

                if (GUI.Button(rightPosition, "Save", right))
                {
                    var ret = typeRegex.Match(property.type);
                    if (ret.Success)
                    {
                        var type = ret.Groups[1].Value;
                        var instance = ScriptableObject.CreateInstance(type);
                        var path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
                        var dir = Path.GetDirectoryName(path);
                        var instancePath = EditorUtility.SaveFilePanel("Create", dir, type, "asset");
                        instancePath = "Assets" + instancePath.Replace(Application.dataPath, "");

                        AssetDatabase.CreateAsset(instance, instancePath);
                        AssetDatabase.Refresh();
                        property.objectReferenceValue = instance;
                    }
                }
            }
        }
    }

    private static void CreateInstance(SerializedProperty property, string type)
    {
        var instance = ScriptableObject.CreateInstance(type);
        var path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
        if (string.IsNullOrEmpty(path))
        {
            var root = PrefabUtility.GetOutermostPrefabInstanceRoot(property.serializedObject.targetObject);
            if (root)
            {
                var ori = PrefabUtility.GetPrefabParent(root);
                path = AssetDatabase.GetAssetPath(ori);
            }
        }
        var dir = Path.GetDirectoryName(path);
        var ex = ".asset";
        path = dir.CreateFileName($"{property.serializedObject.targetObject.name}_{type}", ex);

        AssetDatabase.CreateAsset(instance, path);
        AssetDatabase.Refresh();
        property.objectReferenceValue = instance;
    }
}


