using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    public class GameObjectConditon : ConditionSO<GameObject>
    {
        public GameObjectFilter filter;
        public override bool Match(GameObject input)
        {
            return filter.Check(input);
        }
    }
}

