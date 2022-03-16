using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

public class StringConditionTest : ConditionSO<string>
{
    public override bool Match(string input)
    {
        return false;
    }
}
