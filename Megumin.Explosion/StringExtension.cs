﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

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
    public static string[] Split(this string s, int perLength, bool allowLastOneShortCount = false)
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
        else if (s.Length == perLength)
        {
            return new string[1] { s };
        }
        else
        {
            int count = s.Length / perLength;
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
        where T : Enum
    {
        return (T)Enum.Parse(typeof(T), value);
    }

    /// <summary>
    /// 将string转换成bool。
    /// <para>当值为"true""TRUE""True"之一时返回true，否则返回false。</para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool ToBool(this string value)
    {
        return value == "true" || value == "TRUE" || value == "True";
    }

    public static string Url2FileName(this string url)
    {
        return Regex.Replace(url,
                             @"[^a-z0-9.]+",
                             "_",
                             RegexOptions.IgnoreCase
                             | RegexOptions.Multiline
                             | RegexOptions.CultureInvariant);
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

    public static bool TryPerseHexColor(this string hex,
                                        out byte r,
                                        out byte g,
                                        out byte b,
                                        out byte a,
                                        int startIndex = 0)
    {
        r = 0;
        g = 0;
        b = 0;
        a = byte.MaxValue;

        try
        {
            if (!IsDigit(hex[startIndex], 16, out var r1))
            {
                return false;
            }

            if (!IsDigit(hex[startIndex + 1], 16, out var r2))
            {
                return false;
            }
            r = (byte)(r1 * 16 + r2);


            if (!IsDigit(hex[startIndex + 2], 16, out var g1))
            {
                return false;
            }

            if (!IsDigit(hex[startIndex + 3], 16, out var g2))
            {
                return false;
            }
            g = (byte)(g1 * 16 + g2);


            if (!IsDigit(hex[startIndex + 4], 16, out var b1))
            {
                return false;
            }

            if (!IsDigit(hex[startIndex + 5], 16, out var b2))
            {
                return false;
            }
            b = (byte)(b1 * 16 + b2);

            if (hex.Length >= 8 + startIndex)
            {
                //透明度是可选的
                if (!IsDigit(hex[startIndex + 6], 16, out var a1))
                {
                    return false;
                }

                if (!IsDigit(hex[startIndex + 7], 16, out var a2))
                {
                    return false;
                }
                a = (byte)(a1 * 16 + a2);
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// https://source.dot.net/#System.Private.CoreLib/ParseNumbers.cs,634
    /// </summary>
    /// <param name="c"></param>
    /// <param name="radix"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    [System.Diagnostics.DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDigit(this char c, int radix, out int result)
    {
        int tmp;
        if ((uint)(c - '0') <= 9)
        {
            result = tmp = c - '0';
        }
        else if ((uint)(c - 'A') <= 'Z' - 'A')
        {
            result = tmp = c - 'A' + 10;
        }
        else if ((uint)(c - 'a') <= 'z' - 'a')
        {
            result = tmp = c - 'a' + 10;
        }
        else
        {
            result = -1;
            return false;
        }

        return tmp < radix;
    }

    public static readonly Regex EndNumber = new Regex(@"(\d+)$");
    public static readonly Regex EndNumber2 = new Regex(@"\((\d+)\)$");
    public static bool GetEndNumber(this string orignal, out int number)
    {
        number = default;
        var result = EndNumber.Match(orignal);

        if (result.Success)
        {
            number = int.Parse(result.Groups[1].Value);
            return true;
        }

        var result2 = EndNumber2.Match(orignal);
        if (result2.Success)
        {
            number = int.Parse(result2.Groups[1].Value);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 按照AaBbCc排序比较
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int CompareAaBbCc(this string a, string b)
    {
        if (string.Equals(a, b))
        {
            return 0;
        }

        var index = 0;
        while (true)
        {
            char ac = (char)0;
            if (index < a.Length)
            {
                ac = a[index];
            }

            var bc = (char)0;
            if (index < b.Length)
            {
                bc = b[index];
            }

            var acl = char.ToLower(ac);
            var bcl = char.ToLower(bc);
            if (acl != bcl)
            {
                return acl - bcl;
            }
            else
            {
                if (ac != bc)
                {
                    return ac - bc;
                }
            }

            index++;
        }
    }
}

namespace System.IO
{
    using static StringExtension_E68DD56066C94F2286AF4BD18126A406;

    public static class StringExtension_FA3412521A22430AAAF2BB15471A5277
    {
        /// <summary>
        /// 安全替换路径中的文件名，会检测是否已经存在。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public static string ReplaceFileName(this string path, string newFileName = "NewInstance")
        {
            var dir = Path.GetDirectoryName(path);
            var ex = Path.GetExtension(path);
            return dir.CreateFileName(newFileName, ex);
        }

        /// <summary>
        /// 如果文件命以数字或者括号数字结尾，则数字加一
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="namingScheme">数字后缀模式支持SpaceParenthesis Dot Underscore</param>
        /// <param name="namingDigits">数字长度,不足填充0</param>
        /// <returns></returns>
        public static string FileNameAddOne(this string fileName, string namingScheme = "SpaceParenthesis", int namingDigits = 1)
        {
            var result = EndNumber.Match(fileName);
            if (result.Success)
            {
                var oriStr = result.Groups[1].Value;
                var number = int.Parse(oriStr);
                var f = fileName.Substring(0, fileName.Length - result.Value.Length);
                f += (number + 1).ToString().PadLeft(oriStr.Length, oriStr[0]);
                return f;
            }

            var result2 = EndNumber2.Match(fileName);
            if (result2.Success)
            {
                var oriStr = result2.Groups[1].Value;
                var number = int.Parse(oriStr);
                var f = fileName.Substring(0, fileName.Length - result2.Value.Length);
                var numStr = (number + 1).ToString().PadLeft(oriStr.Length, oriStr[0]);
                f += $"({numStr})";
                return f;
            }

            fileName += CreateNamePostfix(1, namingScheme, namingDigits);
            return fileName;
        }

        public static string AutoFileName(this string fileName, string dir, string ex, string namingScheme = "SpaceParenthesis", int namingDigits = 1)
        {
            var path = Path.Combine(dir, $"{fileName}{ex}");
            if (!File.Exists(path))
            {
                return fileName;
            }

            string newFName = fileName.FileNameAddOne(namingScheme, namingDigits);
            path = Path.Combine(dir, $"{newFName}{ex}");
            while (File.Exists(path))
            {
                newFName = newFName.FileNameAddOne(namingScheme, namingDigits);
                path = Path.Combine(dir, $"{newFName}{ex}");
            }
            return newFName;
        }

        /// <summary>
        /// fileName 如果存在，自增
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="fileName"></param>
        /// <param name="ex">需要前面有 . </param>
        /// <param name="namingScheme">数字后缀模式支持SpaceParenthesis Dot Underscore</param>
        /// <param name="namingDigits">数字长度,不足填充0</param>
        /// <returns></returns>
        public static string CreateFileName(this string dir, string fileName, string ex, string namingScheme = "SpaceParenthesis", int namingDigits = 1)
        {
            var path = Path.Combine(dir, $"{fileName}{ex}");
            if (!File.Exists(path))
            {
                return path;
            }

            int cloneCount = 1;
            do
            {
                var namingSchemeStr = CreateNamePostfix(cloneCount, namingScheme, namingDigits);
                path = Path.Combine(dir, $"{fileName}{namingSchemeStr}{ex}");
                cloneCount++;
            } while (File.Exists(path));
            return path;
        }

        public static string CreateNamePostfix(int count, string namingScheme = "SpaceParenthesis", int namingDigits = 1)
        {
            var countStr = count.ToString().PadLeft(namingDigits, '0');
            var namingSchemeStr = $" ({countStr})";
            switch (namingScheme)
            {
                case "Dot":
                    namingSchemeStr = $".{countStr}";
                    break;
                case "Underscore":
                    namingSchemeStr = $"_{countStr}";
                    break;
                default:
                    break;
            }

            return namingSchemeStr;
        }


    }
}

