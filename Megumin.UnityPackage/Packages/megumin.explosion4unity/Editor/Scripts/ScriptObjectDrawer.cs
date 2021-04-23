using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;

[CustomPropertyDrawer(typeof(ScriptableObject), true)]
public class ScriptObjectDrawer_8F11D385 : PropertyDrawer
{
    static GUIStyle left = new GUIStyle("minibuttonleft");
    static GUIStyle right = new GUIStyle("minibuttonright");
    static Regex typeRegex = new Regex(@"^PPtr\<\$(.*)>$");

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var propertyPosition = position;
        propertyPosition.width -= 86;

        var buttonPosition = position;
        buttonPosition.width = 80;
        buttonPosition.x += position.width - 80;

        EditorGUI.PropertyField(propertyPosition, property, label);
        var obj = property.objectReferenceValue;

        var leftPosotion = buttonPosition;
        leftPosotion.width = 40;
        var rightPosition = buttonPosition;
        rightPosition.width = 40;
        rightPosition.x += 40;

        if (obj)
        {
            if (GUI.Button(leftPosotion, "New", left))
            {
                var clone = ScriptableObject.CreateInstance(obj.GetType());

                var path = obj.CreateNewPath(true);

                AssetDatabase.CreateAsset(clone, path);
                AssetDatabase.Refresh();
                property.objectReferenceValue = clone;
            }

            if (GUI.Button(rightPosition, "Clone", right))
            {
                var clone = ScriptableObject.Instantiate(obj);
                var path = AssetDatabase.GetAssetPath(obj);
                var newFileName = Path.GetFileNameWithoutExtension(path) + " Clone";
                path = path.ReplaceFileName(newFileName);

                AssetDatabase.CreateAsset(clone, path);
                AssetDatabase.Refresh();
                property.objectReferenceValue = clone;
            }
        }
        else
        {
            if (GUI.Button(leftPosotion, "New", left))
            {
                var ret = typeRegex.Match(property.type);
                if (ret.Success)
                {
                    var type = ret.Groups[1].Value;
                    var instance = ScriptableObject.CreateInstance(type);

                    var path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
                    var dir = Path.GetDirectoryName(path);
                    var ex = Path.GetExtension(path);
                    path = dir.CreateFileName(type, ex);

                    AssetDatabase.CreateAsset(instance, path);
                    AssetDatabase.Refresh();
                    property.objectReferenceValue = instance;
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


