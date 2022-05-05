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

    public UnityEngine.Object Folder;

    public Enableable<int> EnableableInt = new Enableable<int>(false, 20);
    public Overridable<int> OverridableInt = new Overridable<int>(99);

    void Test()
    {
        if (TestCondition.Match("hello"))
        {
        }
    }
}
