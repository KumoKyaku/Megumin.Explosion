using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FastLabel : MonoBehaviour
{
    public Rect Position = new Rect(0, 50, Screen.width, 200);
    public string Content = "Fast Label";
    public GUIStyle LabelStyle;

    public static FastLabel Instance { get; protected set; }
    void Awake()
    {
        Instance = this;
        InitStyle(Color.white);
    }

    [EditorButton]
    private void InitStyle(Color textColor, int fontSize = 20)
    {
        LabelStyle = new GUIStyle("DefaultCenteredLargeText");
        LabelStyle.fontSize = fontSize;
        LabelStyle.normal.textColor = textColor;
    }

    [EditorButton]
    public static void Show(string text = "Fast Label")
    {
        if (Instance)
        {
            Instance.Content = text;
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
            GUI.Label(Position, Content, LabelStyle);
        }
    }
}







