using Megumin;
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

        GlobalToggle = new Pref<bool>(nameof(TestLog), true);
        Debug.Log($"{nameof(GlobalToggle)}:{GlobalToggle}");
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

    [EditorButton]
    public void TestParseColor()
    {
        foreach (var item in HexColor.GetAllStaticColor())
        {
            bool ret1 =  ColorUtility.TryParseHtmlString(item.Item2.ToString(), out var color);
            bool ret2 = MeguminColorUtility.TryParseHtmlString(item.Item2.ToString(), out var color2);
            if (color.Equals(color2))
            {
                Debug.Log($"结果一致 Color1 = Color2");
            }
            else
            {
                Debug.LogError($"结果不一致 Color1: {color} -- Color2:{color2}");
            }

            ret1 = ColorUtility.TryParseHtmlString(item.Item2.ToString(), out var color3);
            ret2 = MeguminColorUtility.TryParseHtmlString(item.Item2.ToString(), out var color4);

        }
    }

    [Options2String(typeof(ConstTest))]
    public string Optionsss;
    public Pref<bool> GlobalToggle;

    [EditorButton]
    public void TestQ()
    {
        Vector3 vector3 = new Vector3(0.1f, 0.2f, 0.3f);
        var matri = Matrix4x4.TRS(vector3, Quaternion.identity, Vector3.one);
        var r = matri.GetPosition();
    }

}

public class ConstTest
{
    public const string Tset111 = "Tset111";
    [Alias("别名")]
    [Alias("别名测试")]
    public const string aa = "Tset22";
}