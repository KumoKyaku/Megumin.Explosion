using Megumin;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// ConsoleLink:Test () (at <a href="Assets/ConsoleLink.cs" line="11">Assets/ConsoleLink.cs:11</a>)
/// 使用imguidebugger查看链接源文本
/// </summary>
public class ConsoleLink : MonoBehaviour
{
    [Button]
    public void Test()
    {
        //Debug.Log(@$"<a href=""Assets/TestLog.cs"" line=""9"">TestLink</a>");

        string link = typeof(ConsoleLink).GetUnityProjectLink();

        Debug.Log(link);
    }
}
