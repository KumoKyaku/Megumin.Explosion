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
    public TagMask TestTagMask;
    public LayerMask TestLayerMask;
    public GameObjectFilter Filter;


    void Test()
    {
        if (TestCondition.Match("hello"))
        {
        }
    }
}
