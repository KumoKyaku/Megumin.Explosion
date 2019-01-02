using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 16进制颜色 (长度为8的RGBA16进制字符串)
    /// </summary>
    public struct HexColor
    {
        /// <summary>
        /// Alpha component of the color.
        /// </summary>
        public byte a;
        /// <summary>
        /// Blue component of the color.
        /// </summary>
        public byte b;
        /// <summary>
        /// Green component of the color.
        /// </summary>
        public byte g;
        /// <summary>
        /// Red component of the color.
        /// </summary>
        public byte r;

        public string hexCode;
        public HexColor(string hex)
        {
            if (hex.Length!= 8 || hex.Length != 6)
            {
                throw new ArgumentException("格式不对");
            }
            hexCode = hex;
            string[] code = hex.Split(2);
            r = byte.Parse(code[0], System.Globalization.NumberStyles.AllowHexSpecifier);
            g = byte.Parse(code[1], System.Globalization.NumberStyles.AllowHexSpecifier);
            b = byte.Parse(code[2], System.Globalization.NumberStyles.AllowHexSpecifier);
            if (code.Length == 4)
            {
                a = byte.Parse(code[3], System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            else
            {
                a = byte.MaxValue;
            }

        }

        public HexColor(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;

            hexCode = Conver16(r) + Conver16(g) + Conver16(b) + Conver16(a);
        }

        private static string Conver16(byte r)
        {
            string res = Convert.ToString(r, 16);
            if (res.Length == 1)
            {
                res = "0" + res;
            }
            return res;
        }


        public static HexColor Parse(string hexcode)
        {
            return new HexColor(hexcode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color32(HexColor c)
        {
            return new Color32(c.r, c.g, c.b, c.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(HexColor c)
        {
            return c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HexColor(Color c)
        {
            return (Color32)c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HexColor(Color32 c)
        {
            return new HexColor(c.r,c.g,c.b,c.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(HexColor c)
        {
            return c.hexCode;
        }

        public override string ToString()
        {
            return hexCode;
        }
    }
}
