using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
                        var temp = field.GetValue(obj);
                        result = temp;
                        return true;
                    }
                    else if (m is PropertyInfo property)
                    {
                        var temp = property.GetValue(obj);
                        result = temp;
                        return true;
                    }
                    else if (m is MethodInfo method)
                    {
                        var temp = method.Invoke(obj, null);
                        result = temp;
                        return true;
                    }
                }
            }
            result = null;
            return false;
        }
    }
}
