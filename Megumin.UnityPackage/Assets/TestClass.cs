using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;
using System;

public class TestClass : MonoBehaviour
{
    public Condition TestCondition;
    public Condition TestCondition2;
    public TagFilter TagFilter;



    [Space]
    public TagMask TestTagMask;
    public LayerMask TestLayerMask;


    void Test()
    {
        if (TestCondition.Match("hello"))
        {
        }
    }
}
