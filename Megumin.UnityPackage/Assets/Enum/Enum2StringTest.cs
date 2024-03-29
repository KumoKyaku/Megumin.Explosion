﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

public class Enum2StringTest : MonoBehaviour
{
    [Enum2String(typeof(KeypadSudoku))]
    public string key;
    [Enum2String(typeof(KeypadSudoku))]
    public string sodoku;
    [Enum2String(typeof(KeypadSudoku))]
    public int test;
    public int test2 = 1;

    void Awake()
    {
#if UNITY_EDITOR
        string detail = Utility.ToStringReflection<UnityEditor.EditorApplication>();
        Debug.LogError(detail);
#endif
    }
}
