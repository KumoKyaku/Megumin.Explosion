using Megumin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntConditonTest : ConditionSO<int>
{
    public override bool Match(int input)
    {
        return input == 0;
    }
}
