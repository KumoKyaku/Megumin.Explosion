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

    [Member2String(nameof(ProtectedInInspector))]
    public string Member2String = "Member2String";

    [Member2String(nameof(ReadOnlyInInspector))]
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

    [Snap(60)]
    public int Snap = 60;
}
