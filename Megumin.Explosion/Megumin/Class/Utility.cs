using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

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
        /// <param name="actualType">默认true通过value取得实际类型,false取得泛型类型,T可能是接口类型</param>
        /// <returns></returns>
        public static string ToStringReflection<T>(T value = default, bool actualType = true)
        {
            Type type = typeof(T);
            if (actualType && value != null)
            {
                type = value.GetType();
            }
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
            detail += $"Type  :  {type.Name}";

            if (value is string str)
            {
                detail += $"    {str}";
                return detail;
            }

            if (type.IsPrimitive)
            {
                detail += $"    {value}";
                return detail;
            }

            if (type.IsValueType)
            {
                detail += "    ValueType";
            }

            if (value.TryGetName(out var instanName))
            {
                detail += $"    {instanName}\n";
            }
            else
            {
                detail += "\n";
            }

            detail += "       \n";

            detail += ToStringReflectionFieldPropertiesStatic(type, value, flags);
            detail += ToStringReflectionFieldPropertiesInstance(type, value, flags);

            return detail;
        }

        public static string ToStringReflectionFieldPropertiesStatic<T>(Type type, T value, BindingFlags? flags)
        {
            var detail = "";
            {
                BindingFlags staticflag = BindingFlags.Public | BindingFlags.Static;
                if (flags.HasValue)
                {
                    staticflag |= flags.Value;
                }

                detail += "Static:   \n";
                detail += ToStringReflectionFieldProperties(type, "    ", staticflag, value);
            }

            return detail;
        }

        public static string ToStringReflectionFieldPropertiesInstance<T>(Type type, T value, BindingFlags? flags = null)
        {
            var detail = "";
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

        /// <summary>
        /// 反射拿名字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool TryGetName<T>(this T value, out string name)
        {
            name = null;
            if (value != null)
            {
                var ms = value.GetType().GetMembers(
                    BindingFlags.Public
                    | BindingFlags.Instance
                    | BindingFlags.NonPublic).OrderBy(m => m.Name.Length);

                foreach (var item in ms)
                {
                    if (String.Equals(item.Name, "Name", StringComparison.CurrentCultureIgnoreCase)
                        || String.Equals(item.Name, "DisplayName", StringComparison.CurrentCultureIgnoreCase)
                        || String.Equals(item.Name, "FriendlyName", StringComparison.CurrentCultureIgnoreCase)
                        )
                    {
                        if (item is FieldInfo field)
                        {
                            var temp = field.GetValue(value);
                            if (temp is string v)
                            {
                                name = v;
                                return true;
                            }
                        }
                        else if (item is PropertyInfo property)
                        {
                            var temp = property.GetValue(value);
                            if (temp is string v)
                            {
                                name = v;
                                return true;
                            }
                        }
                    }
                }
            }
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
                try
                {
                    detail += $"{retract}    {item.Name}  :  {item.GetValue(value)?.ToString()}\n";
                    detail += LogListDic(retract + "        ", item.GetValue(value));
                }
                catch (Exception e)
                {
                    detail += $"{retract}    {item.Name}  :  {e.GetType().Name}|{e.Message}\n";
                }
            }

            detail += retract + "Propertie:   \n";
            var props = type.GetProperties(binding);

            foreach (var item in props)
            {
                try
                {
                    detail += $"{retract}    {item.Name}:    {item.GetValue(value)?.ToString()}\n";
                    detail += LogListDic(retract + "        ", item.GetValue(value));
                }
                catch (Exception e)
                {
                    detail += $"{retract}    {item.Name}  :  {e.GetType().Name}|{e.Message}\n";
                }
            }

            return detail;
        }

        /// <summary>
        /// 打印详细集合
        /// </summary>
        /// <param name="retract"></param>
        /// <param name="fo"></param>
        /// <returns></returns>
        public static string LogListDic(string retract, object fo)
        {
            string detail = "";
            try
            {
                if (fo is IList list)
                {
                    foreach (var item in list)
                    {
                        detail += retract + item.GetType().Name + "  :  " + item?.ToString() + "\n";
                    }
                }

                if (fo is IDictionary dictionary)
                {
                    foreach (IDictionaryEnumerator item in dictionary)
                    {
                        detail += retract + item.Key.ToString() + "  :  " + item.Value.ToString() + "\n";
                    }
                }
            }
            catch (Exception e)
            {
                detail += $"{retract}    {e.GetType().Name}|{e.Message}\n";
            }

            return detail;
        }
    }

    /// <summary>
    /// 因为由扩展方法和静态方法重名, this扩展不能有默认值,
    /// </summary>
    public static class Utility2
    {
        /// <summary>
        /// 通过反射获得属性值字符串,一般用于打印到控制台.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="actualType">默认true通过value取得实际类型,false取得泛型类型,T可能是接口类型</param>
        /// <returns></returns>
        public static string ToStringReflection<T>(this T value, bool actualType = true)
        {
            return Utility.ToStringReflection(value, actualType);
        }
    }
}
