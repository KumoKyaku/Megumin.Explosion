using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 满足线性变换的
    /// </summary>
    public interface ILinearable
    {
    }

    /// <summary>
    /// 使用Y=Ax+b公式的
    /// </summary>
    public interface IYAXBable : ILinearable
    {
        float A { get; }
        float B { get; }

        public float Y(float x)
        {
            return A * x + B;
        }
    }

    [Serializable]
    public class YAXB : IYAXBable
    {
        [field: SerializeField]
        public float A { get; set; } = 1;

        [field: SerializeField]
        public float B { get; set; } = 0;
    }
}

