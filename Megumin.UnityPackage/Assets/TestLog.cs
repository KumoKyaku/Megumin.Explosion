using Megumin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[OnKey(ConsoleKey.Oem3)]
[OnKey(Key = System.ConsoleKey.F5)]
public class TestLog : MonoBehaviour
{
    [InspectorStyle(ReadOnly = true, Color = "#00FF00FF")]
    public string teststr = "TestStr";
    [ColorSpacer(30, 3, 100, 1, 0, 0)]
    public string ret = "";
    //[OnValueChanged]
    public int IntValue;
    [ReadOnlyInInspector]
    public int ReadOnlyIntValue = 9999;
    public Enableable<int> IntValue2 = new Enableable<int>(false, 20);
    public EnableFrame IntValue3 = new EnableFrame() { Enabled = false, Value = 30 };

    public Overridable<int> OverridableInt = new Overridable<int>(20);
    [FrameAndTime]
    public int FrameCount = 20;
    [Enum2String(typeof(TestEnum))]
    public string TestEnumField;
    public iint Iint;

    [Indent]
    public Color TestColor;
    public HexColor HexColor;
    public List<Loger> Loggers;
    public List<Test12345> Test12345s;

    //[ReadOnlyInInspector]
    //[OnValueChanged]
    public HSVColor HSVColor;

    //[OnValueChanged(CallBackName = "OnValueChangeTest1")]
    public STest STest;
    public SaveAsset SaveAsset;
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

    [Button]
    public void Test(int a, string str = "默认值1111111111")
    {
        Debug.Log($"{str.Html(HSVColor)}--{HSVColor}--{(Color)HSVColor}--{(HexColor)HSVColor}");
    }

    [Button]
    public void LogColor()
    {
        HexColor.DebugLogColor();
    }

    [Button]
    public void TestParseColor()
    {
        foreach (var item in HexColor.GetAllStaticColor())
        {
            bool ret1 = ColorUtility.TryParseHtmlString(item.Item2.ToString(), out var color);
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


    public Pref<bool> GlobalToggle;

    [Button]
    public void TestQ()
    {
        Vector3 vector3 = new Vector3(0.1f, 0.2f, 0.3f);
        var matri = Matrix4x4.TRS(vector3, Quaternion.identity, Vector3.one);
        var r = matri.GetPosition();
    }

    [Button]
    public void TestArgs(Loger loger)
    {
        //MeguminEditorUtility.FindCustomEditorTypeByType(typeof(TestScriptObject), false);
    }

    [Button(true)]
    public void Test2(int? loger)
    {

    }

    public enum TestEnum
    {
        Aaa,
        Bbb,
    }

    private void OnValidate()
    {

    }

    public void OnValueChangeTest1()
    {
        Debug.LogError($"OnValueChangeTest1 {STest.intb}");
    }

    public TestScriptObject TestScriptObject;
}

[Serializable]
public class STest : INewCloneButton
{
    //[OnValueChanged(CallBackName = "OnValueChangeTest2")]
    public int inta;
    public int intb;

    public void OnValueChangeTest2()
    {
        Debug.LogError($"OnValueChangeTest2 {inta}");
    }

    public void OnValidate()
    {
        Debug.LogError($"OnValidate {inta}");
    }
}

public class ConstTest
{
    public const string Tset111 = "Tset111";
    [Alias("别名")]
    [Alias("别名测试")]
    public const string aa = "Tset22";
}

public class ConstTest2
{
    public const string 攻击力 = nameof(攻击力);
    public const string 防御力 = nameof(防御力);
}

[Serializable]
public class EnableFrame : Enableable
{
    [FrameAndTime]
    public int Value;
}

[Serializable]
public class Test12345
{
    [ContextMenuItem("Reset12345", "Reset")]
    public Mesh mesh;


    public void Reset()
    {
        mesh = null;
        Debug.LogError("reset");
    }
}