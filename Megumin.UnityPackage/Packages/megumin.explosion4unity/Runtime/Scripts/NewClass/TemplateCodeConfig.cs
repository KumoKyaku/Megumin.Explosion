using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{

    public class TemplateCodeConfig : ScriptableObject
    {
        public TextAsset Template;

        [Serializable]
        public class Mecro
        {
            public string Name;
            public string Value;
        }

        public List<Mecro> MecroList = new List<Mecro>();
    }
}

