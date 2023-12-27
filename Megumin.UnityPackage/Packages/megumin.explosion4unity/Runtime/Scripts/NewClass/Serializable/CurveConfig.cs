using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    public interface ICurveable<out T>
    {
        T Evaluate(float time, bool enabled = true, float defaultValue = 0f);
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

        public float Evaluate(float time, bool enabled = true, float defaultValue = 0f)
        {
            if (!enabled)
            {
                return defaultValue;
            }

            var x = time / XScale;
            x += XOffset;
            var y = Curve?.Evaluate(x) ?? 0f;
            y += YOffset;
            y *= YScale;
            return y;
        }

        public float this[float time, bool enabled = true, float defaultValue = 0f]
        {
            get
            {
                return Evaluate(time, enabled, defaultValue);
            }
        }
    }

    public static class EnableCurveConfigExtension
    {
        public static float Evaluate(this Enable<CurveConfig> curveConfig, float time, float defaultValue = 0)
        {
            if (curveConfig == null)
            {
                return defaultValue;
            }

            if (curveConfig.HasValue)
            {
                return curveConfig.Value.Evaluate(time);
            }

            return defaultValue;
        }
    }

}




