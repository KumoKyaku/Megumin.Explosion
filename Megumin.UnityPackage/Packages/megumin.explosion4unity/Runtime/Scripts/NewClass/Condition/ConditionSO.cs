using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    public abstract class ConditionSO : ScriptableObject
    {
        public abstract bool Match<T>(T input);
    }

    public abstract class ConditionSO<V> : ConditionSO
    {
        public sealed override bool Match<T>(T input)
        {
            if (input is V matchInput)
            {
                return Match(matchInput);
            }

            return false;
        }

        public abstract bool Match(V input);
    }

    [Serializable]
    public class EnableableConditionSO : Enableable
    {
        [SupportTypes(typeof(ConditionSO),
            AllowAbstract = false,
            AllowGenericType = false,
            AllowInterface = false,
            IncludeChildInSameAssembly = true,
            IncludeChildInOtherAssembly = true)]
        public ConditionSO Value;

        public override bool HasValue
        {
            get
            {
                return Enabled && Value;
            }
        }
    }

    [Serializable]
    public class Condition
    {
        [SerializeField]
        private EnableableConditionSO ConditionSO = new EnableableConditionSO();

        public bool Match<T>(T input)
        {
            if (ConditionSO.HasValue)
            {
                return ConditionSO.Value.Match(input);
            }
            return false;
        }
    }

}






