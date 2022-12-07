using Megumin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttribute : MonoBehaviour
{
    [Serializable]
    public class EnableTest
    {
        public bool Enabled;
        public int Test = 1;
    }

    [AreaMask]
    public int AreaMask = -1;
    [AutoFileName]
    public string AutoFileName;

    [MetaGUID]
    public string TestMetaGUID;

    //[ColorSpacer(4, 2, 300, "#E30022")]

    [Enableable]
    public EnableTest EnableAttribut;

    [Enum2String(typeof(LerpType))]
    public string Enum2String = LerpType.Lerp.ToString();

    [EnumQueue(typeof(LerpType))]
    public int EnumQueue = 0;

    [FrameAndTime]
    public int FrameAndTime = 0;

    [GUID]
    public string GUID;

    [HelpBox("HelpBox")]
    [Indent]
    public string Indent = "IndentTest";
    [Indent]
    public int IndentInt = 99;

    [Layer]
    public int LayerInt;
    [Layer]
    public string LayerString;

    [Member2String(nameof(ProtectedInInspector))]
    public string Member2String = "Member2String";

    [Member2String(nameof(ReadOnlyInInspector), nameof(ProtectedInInspector))]
    public List<string> Member2String2;

    [Options2String(typeof(ConstTest))]
    public string Option2String;

    [Options2String(typeof(ConstTest), typeof(ConstTest2))]
    public string Option2String2;

    [Path]
    public string Path;

    [ProtectedInInspector]
    public string[] ProtectedInInspector = new string[]
    {
        "ProtectedInInspector1",
        "ProtectedInInspector2",
        "ProtectedInInspector3"
    };

    [ReadOnlyInInspector]
    public List<string> ReadOnlyInInspector = new List<string>()
    {
        "ReadOnlyInInspector1",
        "ReadOnlyInInspector2",
        "ReadOnlyInInspector3"
    };

    [Path]
    public List<string> Paths = new List<string>();

    [Snap(60)]
    public int Snap = 60;

    [Tag]
    public string Tag;

    public ClampedValue<int> ClampedValue = new ClampedValue<int>() { Max = 99, Min = 1 };
}
