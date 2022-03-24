using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

public class TestClass : MonoBehaviour
{
    public Condition TestCondition;
    public Condition TestCondition2;
    public TagMask TestTagMask;

    void Test()
    {
        if (TestCondition.Match("hello"))
        {

        }
    }
}
