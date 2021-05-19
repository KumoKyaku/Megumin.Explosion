using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    public interface ICurveable<out T>
    {
        T Evaluate(float time);
    }

    [Serializable]
    public class CurveConfig : ICurveable<float>
    {
        public AnimationCurve Curve;
        [Range(-20, 20)]
        public float XScale = 1;
        [Range(-20, 20)]
        public float XOffset = 0;
        [Range(-20, 20)]
        public float YScale = 1;
        [Range(-20, 20)]
        public float YOffset = 0;

        public float Evaluate(float time)
        {
            var x = time / XScale;
            x += XOffset;
            var y = Curve.Evaluate(x);
            y += YOffset;
            y *= YScale;
            return y;
        }
    }
}




