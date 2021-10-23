﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 工具
    /// </summary>
    /// <remarks>MeguminUtility名字太长,即时Utility和别的命名空间Utility冲突,使用全名称限定就可以了.</remarks>
    public static class Utility
    {
        public static string ToStringReflection(Type type)
        {
            return ToStringReflection<object>(type, null);
        }

        /// <summary>
        /// 通过反射获得属性值字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringReflection<T>(T value = default)
        {
            Type type = typeof(T);
            return ToStringReflection(type, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="flags">允许设定<see cref="BindingFlags.NonPublic"/></param>
        /// <returns></returns>
        public static string ToStringReflection<T>(Type type, T value, BindingFlags? flags = null)
        {
            string detail = "";
            if (value.TryGetName(out var instanName))
            {
                detail += $"{type.Name }    {instanName}\n";
            }
            else
            {
                detail += type.Name + "\n";
            }


            if (type.IsValueType)
            {
                detail += "ValueType";
            }

            detail += "       \n";
            {
                BindingFlags staticflag = BindingFlags.Public | BindingFlags.Static;
                if (flags.HasValue)
                {
                    staticflag |= flags.Value;
                }

                detail += "Static:   \n";
                detail += ToStringReflectionFieldProperties(type, "    ", staticflag, value);
            }

            if (value != null)
            {
                BindingFlags instanceflag = BindingFlags.Public | BindingFlags.Instance;
                if (flags.HasValue)
                {
                    instanceflag |= flags.Value;
                }

                detail += "       \n";
                detail += "Instance:   \n";
                detail += ToStringReflectionFieldProperties(type, "    ", instanceflag, value);
            }

            return detail;
        }

        internal static bool TryGetName<T>(this T value, out string name)
        {
            name = null;
            //try
            //{
            //    dynamic d = value;
            //    name = d.Name;
            //    return true;
            //}
            //catch (Exception)
            //{
            //}

            //if (string.IsNullOrEmpty(name))
            //{
            //    try
            //    {
            //        dynamic d = value;
            //        name = d.name;
            //        return true;
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}
            return false;
        }

        public static string ToStringReflectionFieldProperties<T>(Type type, string retract, BindingFlags binding, T value)
        {
            string detail = "";
            detail += retract + "Field:   \n";
            var fields = type.GetFields(binding);

            foreach (var item in fields)
            {
                detail += retract + "    " + item.Name + "  :  " + item.GetValue(value)?.ToString() + "\n";
            }

            detail += retract + "Propertie:   \n";
            var props = type.GetProperties(binding);

            foreach (var item in props)
            {
                detail += retract + "    " + item.Name + ":    " + item.GetValue(value)?.ToString() + "\n";
            }

            return detail;
        }
    }

    /// <summary>
    /// 因为由扩展方法和静态方法重名
    /// </summary>
    public static class Utility2
    {
        /// <summary>
        /// 通过反射获得属性值字符串,一般用于打印到控制台.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringReflection<T>(this T value)
        {
            return Utility.ToStringReflection(value);
        }
    }
}
