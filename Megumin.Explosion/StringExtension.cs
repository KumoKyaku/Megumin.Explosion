using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// string扩展
    /// </summary>
    public static class StringExtension_E68DD56066C94F2286AF4BD18126A406
    {
        /// <summary>
        /// 按指定长度分割字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="perLength">每段长度</param>
        /// <param name="allowLastOneShortCount">是够允许最后一段长度小于每段长度，
        /// 如果为false，不足的末尾将被舍弃。默认为false。</param>
        /// <returns></returns>
        public static string[] Split(this string s,int perLength,bool allowLastOneShortCount = false)
        {
            if (perLength > 0 == false)
            {
                throw new ArgumentException("每段长度必须大于0");
            }

            if (s.Length < perLength)
            {
                //字符串长度小于要分割每段长度
                if (allowLastOneShortCount)
                {
                    return new string[1] { s };
                }
                else
                {
                    return null;
                }
            }
            else if(s.Length == perLength)
            {
                return new string[1] { s };
            }
            else
            {
                int count = s.Length/perLength;
                int yushu = s.Length % perLength;
                //
                string[] res = null;
                if (yushu != 0 && allowLastOneShortCount)
                {
                    res = new string[count + 1];
                }
                else
                {
                    res = new string[count];
                }

                for (int i = 0; i < count; i++)
                {
                    string per = s.Substring(i * perLength, perLength);
                    res[i] = per;
                }

                if (yushu != 0 && allowLastOneShortCount)
                {
                    res[count] = s.Substring((count) * perLength, yushu);
                }

                return res;
            }
        }



        /// <summary>
        /// 使用int.Parse转换一个字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        /// <summary>
        /// 将一个字符串转换成枚举
        /// </summary>
        /// <typeparam name="T">提供一个枚举类型</typeparam>
        /// <param name="value"></param>
        /// <returns>返回对应的枚举值</returns>
        /// <exception cref="ArgumentException">所给泛型不是枚举</exception>
        public static T ToEnum<T>(this string value)
            where T:Enum
        {
            return  (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// 将string转换成bool。
        /// <para>当值为"true""TRUE""True"之一时返回true，否则返回false。</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this string value)
        {
            return value == "true"|| value == "TRUE"||value == "True";
        }

        //#region 加密相关

        //const int DefaultSeed = 63;

        ///// <summary>
        ///// 对字符串进行轻量的加密，使用Decrypt方法解密
        ///// </summary>
        ///// <param name="original"></param>
        ///// <param name="Seed">种子</param>
        ///// <returns></returns>
        //public static string Encipher(this string original,int Seed = 0)
        //{
        //    var array = original.ToCharArray();
        //    var tempseed = GetSeed(Seed);
        //    string result = "";
        //    foreach (var item in array)
        //    {
        //        int tempvalue = (int)item;
        //        result += (tempvalue + tempseed).ToString() + ".";
        //    }

        //    return result;
        //}

        //private static int GetSeed(int seed)
        //{
        //    return seed == 0 ? DefaultSeed : seed;
        //}

        ///// <summary>
        ///// 解密密文
        ///// </summary>
        ///// <param name="ciphertext"></param>
        ///// <param name="Seed">种子</param>
        ///// <returns></returns>
        //public static string Decrypt(this string ciphertext,int Seed = 0)
        //{
        //    string[] result = ciphertext.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

        //    var tempseed = GetSeed(Seed);

        //    string original = "";
        //    foreach (var item in result)
        //    {
        //        int tempvalue = item.ToInt();
        //        char temp = (char)(tempvalue-tempseed);
        //        original += temp;
        //    }
        //    return original;
        //}

        //#endregion
    }
}
