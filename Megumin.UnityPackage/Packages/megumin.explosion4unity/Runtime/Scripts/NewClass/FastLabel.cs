using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FastLabel : MonoBehaviour
{
    public Rect Position = new Rect(0, 50, Screen.width, 200);
    public string Content = "Fast Label";
    public GUIStyle LabelStyle;

    public static Object MacroTarget { get; set; }

    public static FastLabel Instance { get; protected set; }
    void Awake()
    {
        Instance = this;
        InitStyle(Color.white);
    }

    [EditorButton]
    private void InitStyle(Color textColor, int fontSize = 40)
    {
        if (LabelStyle == null)
        {
            InitStyle();
        }

        LabelStyle.fontSize = fontSize;
        LabelStyle.normal.textColor = textColor;
    }

    [EditorButton]
    private void InitStyle(string styleName = "CN CountBadge", bool force = false)
    {
        if (LabelStyle == null || force)
        {
            LabelStyle = new GUIStyle(styleName);
        }
    }

    [EditorButton]
    public static void Show(string text = "Fast Label")
    {
        if (Instance)
        {
            Instance.Content = text;
            Instance.InspectorForceUpdate();
        }
    }

    [EditorButton]
    public static void Clear()
    {
        Show(null);
    }

    void OnGUI()
    {
        if (LabelStyle == null)
        {
            InitStyle(Color.white);
        }

        if (string.IsNullOrEmpty(Content))
        {

        }
        else
        {
            MacroTarget.Macro(ref Content);
            GUI.Label(Position, Content, LabelStyle);
        }
    }
}







