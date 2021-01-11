using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 常用工具
    /// </summary>
    public class MeguminUtility
    {
        /// <summary>
        /// 反射出类的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Detail<T>(T value = default)
        {
            Type type = typeof(T);
            string detail = "";
            detail += type.Name + "\n";
            
            if (type.IsValueType)
            {
                detail += "ValueType";
            }
            else
            {
                {
                    detail += "Static:   \n";
                    detail += DetailFieldProp(type, "    ", BindingFlags.Public | BindingFlags.Static);
                }

                if (value !=  null)
                {
                    detail += "       \n";
                    detail += "Instance:   \n";
                    detail += DetailFieldProp(type, "    ", BindingFlags.Public | BindingFlags.Instance);
                }
            }
            return detail;

            static string DetailFieldProp(Type type, string retract, BindingFlags binding)
            {
                string detail = "";
                detail += retract + "Field:   \n";
                var fields = type.GetFields(binding);

                foreach (var item in fields)
                {
                    detail += retract + "    " + item.Name + "  :  " + item.GetValue(null)?.ToString() + "\n";
                }

                detail += retract + "Propertie:   \n";
                var props = type.GetProperties(binding);

                foreach (var item in props)
                {
                    detail += retract + "    " + item.Name + ":    " + item.GetValue(null)?.ToString() + "\n";
                }

                return detail;
            }
        }
    }
}
