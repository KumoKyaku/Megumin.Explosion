using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

public class TestClass : MonoBehaviour
{
    public Condition TestCondition;
    public Condition TestCondition2;

    void Test()
    {
        if (TestCondition.Match("hello"))
        {

        }
    }
}
