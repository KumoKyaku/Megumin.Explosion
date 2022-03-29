using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEngine
{
    /// <summary>
    /// todo：拾色器
    /// </summary>
    [System.Serializable]
    public struct HSVColor
    {
        /// <summary>
        /// Hue component of the color. [0..1].
        /// </summary>
        [Range(0, 1)]
        public float Hue;

        /// <summary>
        /// Saturation component of the color. [0..1].
        /// </summary>
        [Range(0, 1)]
        public float Saturation;

        /// <summary>
        /// Value component of the color.
        /// </summary>
        public float Value;

        /// <summary>
        /// Alpha component of the color. [0..1].
        /// </summary>
        [Range(0, 1)]
        public float Alpha;

        /// <summary>
        /// Initialize a new HSV color with alpha channel.
        /// </summary>
        /// <param name="hue">Hue component of the color.</param>
        /// <param name="saturation">Saturation component of the color.</param>
        /// <param name="value">Value component of the color.</param>
        /// <param name="alpha">Alpha component of the color. 0 — transperent; 1 — opaque.</param>
        public HSVColor(float hue, float saturation, float value, float alpha = 1)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Value = value;
            this.Alpha = alpha;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(in HSVColor hsv)
        {
            var color = Color.HSVToRGB(hsv.Hue, hsv.Saturation, hsv.Value);
            color.a = hsv.Alpha;
            return color;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color32(in HSVColor hsv)
        {
            return (Color)hsv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HexColor(in HSVColor hsv)
        {
            return (Color)hsv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HSVColor(in Color color)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            return new HSVColor(h, s, v, color.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HSVColor(in Color32 color)
        {
            return (Color)color;
        }

        /// <summary>
        /// Returns a formatted string with components of the color.
        /// </summary>
        public override string ToString()
        {
            return $"HSVA({Hue:0.0000}, {Saturation:0.0000}, {Value:0.0000}, {Alpha:0.0000})";
        }

        public static implicit operator Vector4(in HSVColor c)
        {
            return new Vector4(c.Hue, c.Saturation, c.Value, c.Alpha);
        }

        public static implicit operator HSVColor(in Vector4 v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }


        //运算

        public static bool operator ==(in HSVColor lhs, in HSVColor rhs)
        {
            return (Vector4)lhs == (Vector4)rhs;
        }

        public static bool operator !=(in HSVColor lhs, in HSVColor rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (!(other is HSVColor))
            {
                return false;
            }

            return Equals((HSVColor)other);
        }

        public bool Equals(HSVColor other)
        {
            return Hue.Equals(other.Hue)
                   && Saturation.Equals(other.Saturation)
                   && Value.Equals(other.Value)
                   && Alpha.Equals(other.Alpha);
        }


        public override int GetHashCode()
        {
            return ((Vector4)this).GetHashCode();
        }

        public static HSVColor operator +(in HSVColor a, in HSVColor b)
        {
            return new Color(a.Hue + b.Hue, a.Saturation + b.Saturation, a.Value + b.Value, a.Alpha + b.Alpha);
        }

        public static HSVColor operator -(in HSVColor a, in HSVColor b)
        {
            return new Color(a.Hue - b.Hue, a.Saturation - b.Saturation, a.Value - b.Value, a.Alpha - b.Alpha);
        }

        public static HSVColor operator *(in HSVColor a, float b)
        {
            return new HSVColor(a.Hue * b, a.Saturation * b, a.Value * b, a.Alpha * b);
        }

        public static HSVColor operator *(float b, in HSVColor a)
        {
            return new HSVColor(a.Hue * b, a.Saturation * b, a.Value * b, a.Alpha * b);
        }

        public static HSVColor operator /(in HSVColor a, float b)
        {
            return new HSVColor(a.Hue / b, a.Saturation / b, a.Value / b, a.Alpha / b);
        }



        //插值

        public static HSVColor Lerp(in HSVColor a, in HSVColor b, float t)
        {
            t = Mathf.Clamp01(t);
            return new HSVColor(a.Hue + (b.Hue - a.Hue) * t,
                                a.Saturation + (b.Saturation - a.Saturation) * t,
                                a.Value + (b.Value - a.Value) * t,
                                a.Alpha + (b.Alpha - a.Alpha) * t);
        }

        public static HSVColor LerpUnclamped(in HSVColor a, in HSVColor b, float t)
        {
            return new HSVColor(a.Hue + (b.Hue - a.Hue) * t,
                                a.Saturation + (b.Saturation - a.Saturation) * t,
                                a.Value + (b.Value - a.Value) * t,
                                a.Alpha + (b.Alpha - a.Alpha) * t);
        }
    }

}



