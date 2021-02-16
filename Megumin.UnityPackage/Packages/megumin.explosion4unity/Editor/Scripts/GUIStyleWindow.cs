using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// https://www.cnblogs.com/fengxing999/p/11096049.html
/// <see cref="GUIStyleWindow.UseCase"/>
/// </summary>
public class GUIStyleWindow : EditorWindow
{
    private Vector2 scrollVector2 = Vector2.zero;
    private string search = "";

    [MenuItem("Tool/Style/GUIStyle")]
    public static void InitWindow()
    {
        EditorWindow.GetWindow(typeof(GUIStyleWindow));
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        GUILayout.Space(30);
        search = EditorGUILayout.TextField("", search, "SearchTextField", GUILayout.MaxWidth(position.x / 3));
        GUILayout.Label("", "SearchCancelButtonEmpty");
        GUILayout.EndHorizontal();
        scrollVector2 = GUILayout.BeginScrollView(scrollVector2);
        foreach (GUIStyle style in GUI.skin.customStyles)
        {
            if (style.name.ToLower().Contains(search.ToLower()))
            {
                DrawStyleItem(style);
            }
        }
        GUILayout.EndScrollView();
    }

    void DrawStyleItem(GUIStyle style)
    {
        GUILayout.BeginHorizontal("box");
        GUILayout.Space(40);
        EditorGUILayout.SelectableLabel(style.name);
        GUILayout.FlexibleSpace();
        EditorGUILayout.SelectableLabel(style.name, style);
        GUILayout.Space(40);
        EditorGUILayout.SelectableLabel("", style, GUILayout.Height(40), GUILayout.Width(40));
        GUILayout.Space(50);
        if (GUILayout.Button("复制GUIStyle名字"))
        {
            TextEditor textEditor = new TextEditor();
            textEditor.text = style.name;
            textEditor.OnFocus();
            textEditor.Copy();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    /// <summary>
    /// 用法示例
    /// <para></para> https://www.jianshu.com/p/44078f1f07ef
    /// </summary>
    void UseCase()
    {

        GUIStyle btnStyle = new GUIStyle("Command");

        btnStyle.fontSize = 12;

        btnStyle.alignment = TextAnchor.MiddleCenter;

        btnStyle.imagePosition = ImagePosition.ImageAbove;

        btnStyle.fontStyle = FontStyle.Normal;

        btnStyle.fixedWidth = 60;

        //等同于：

        GUIStyle btnStyle_1 = new GUIStyle("Command")

        {

            fontSize = 12,

            alignment = TextAnchor.MiddleCenter,

            imagePosition = ImagePosition.ImageAbove,

            fontStyle = FontStyle.Normal,

            fixedWidth = 60

        };

    }
}


