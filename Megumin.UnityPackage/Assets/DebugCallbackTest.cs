using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;
using System;

[OnF1]
public class DebugCallbackTest : MonoBehaviour
{
    [OnF1]
    public string F1 = "F1";

    [OnF1]
    public float F1Prop = 2.5f;

    [OnKey(ConsoleKey.F5)]
    public int OnKeyF5 = 5;

    [OnKey(ConsoleKey.F6)]
    public long OnKeyF6 { get; set; } = long.MaxValue;

    [OnF1]
    public void TestF1()
    {
        Debug.Log($"µ˜ ‘≤‚ ‘F1");
    }

    [OnKey(ConsoleKey.F4)]
    public void TestF4()
    {
        Debug.Log($"µ˜ ‘≤‚ ‘OnKeyF4");
    }
}
