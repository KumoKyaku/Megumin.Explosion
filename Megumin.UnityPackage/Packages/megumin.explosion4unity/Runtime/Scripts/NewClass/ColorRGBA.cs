using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ColorRGBA : MonoBehaviour
{
    public Color Orignal;

    public string OrignalString;

    [Space(20)]
    public string ParseColorString = "#00FF00FF";

    public Color ParsedColor = Color.green;
    public bool ParseResult = true;


    void OnValidate()
    {
        OrignalString = "#" + ColorUtility.ToHtmlStringRGBA(Orignal);
        if (!ParseColorString.StartsWith("#"))
        {
            ParseColorString = "#" + ParseColorString;
        }
        ParseResult = ColorUtility.TryParseHtmlString(ParseColorString, out ParsedColor);
    }
}
