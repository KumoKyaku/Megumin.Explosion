using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLog : MonoBehaviour
{
    [InspectorStyle(ReadOnly = true, Color = "#00FF00FF")]
    public string teststr = "TestStr";

    [ColorSpacer(30, 3, 100, 1, 0, 0)]
    public string ret = "";

    public Color TestColor;
    // Start is called before the first frame update

    [Path(IsFolder = false, Exetension = "txt")]
    public string path;

    public Megumin.Overridable<string> overridable = new Megumin.Overridable<string>("test");
    void Start()
    {
        string message = ColorUtility.ToHtmlStringRGBA(Color.green);
        Debug.Log(message);
        ColorUtility.TryParseHtmlString(message, out TestColor);
        ret = teststr.Html(HexColor.Blueberry);
        Debug.Log(ret);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [EditorButton]
    public void Test(int a, string str = "默认值1111111111")
    {
        Debug.Log(str);
    }

    [EditorButton]
    public void LogColor()
    {
        HexColor.DebugLogColor();
    }
}
