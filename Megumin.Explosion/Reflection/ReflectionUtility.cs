using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Megumin;

namespace System.Reflection
{
    public static class ReflectionUtility
    {
        public const BindingFlags DefaultFlags
            = BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.Static;

        public static bool TryGetValue(object obj, string memberName, out object result)
        {
            if (obj != null)
            {
                var members = obj.GetType().GetMember(memberName, DefaultFlags);
                foreach (var m in members)
                {
                    if (m is FieldInfo field)
                    {
                        try
                        {
                            var temp = field.GetValue(obj);
                            result = temp;
                            return true;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        
                    }
                    else if (m is PropertyInfo property)
                    {
                        var temp = property.GetValue(obj);
                        result = temp;
                        return true;
                    }
                    else if (m is MethodInfo method)
                    {
                        try
                        {
                            var temp = method.Invoke(obj, null);
                            result = temp;
                            return true;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
            result = null;
            return false;
        }

        public static void GetConstString(
            Type type,
            out string[] optionShow,
            out string[] optionValue,
            out (string Field, string Value, string Show)[] myValue,
            bool sort = false)
        {
            var selected = (from field in type.GetFields()
                            where field.FieldType == typeof(string)
                            && field.IsPublic && field.IsStatic
                            select field);

            if (sort)
            {
                selected = from f in selected
                           orderby f.Name
                           select f;
            }

            var fields = selected.ToArray();

            optionShow = new string[fields.Length];
            optionValue = new string[fields.Length];
            myValue = new (string Field, string Value, string Show)[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var value = field.GetValue(null) as string;
                //var show = value; 使用字段名字而不是值，因为写代码比较时用的也是字段名
                var show = field.Name;
                foreach (var attri in field.GetCustomAttributes(typeof(AliasAttribute), true))
                {
                    if (attri is AliasAttribute alias)
                    {
                        show += $" [{alias.Alias}]";
                    }
                }

                optionShow[i] = show;
                optionValue[i] = value;
                myValue[i] = (field.Name, value, show);
            }
        }

        public static (string Field, string Value, string Show)[] GetConstString(Type type, bool sort = false)
        {
            GetConstString(type, out var _, out var _, out var res, sort);
            return res;
        }
    

    }
}
