using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

public static class TypeExtension_7AE0B2E4B4124A53AE87CE8D95431431
{
    /// <summary>
    /// 尝试取得第一个指定属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T FirstAttribute<T>(this Type type)
        where T : Attribute
    {
        if (type == null)
        {
            return default;
        }
        return type.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
    }


    //https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class

    /// <summary>
    /// https://stackoverflow.com/a/457708/15201132
    /// </summary>
    /// <param name="generic"></param>
    /// <param name="toCheck"></param>
    /// <returns></returns>
    public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }

    /// <summary>
    /// https://stackoverflow.com/a/18828085/15201132
    /// </summary>
    /// <param name="child"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static bool IsSubClassOfGeneric(this Type child, Type parent)
    {
        if (child == parent)
            return false;

        if (child.IsSubclassOf(parent))
            return true;

        var parameters = parent.GetGenericArguments();
        var isParameterLessGeneric = !(parameters != null && parameters.Length > 0 &&
            ((parameters[0].Attributes & TypeAttributes.BeforeFieldInit) == TypeAttributes.BeforeFieldInit));

        while (child != null && child != typeof(object))
        {
            var cur = GetFullTypeDefinition(child);
            if (parent == cur || (isParameterLessGeneric && cur.GetInterfaces().Select(i => GetFullTypeDefinition(i)).Contains(GetFullTypeDefinition(parent))))
                return true;
            else if (!isParameterLessGeneric)
                if (GetFullTypeDefinition(parent) == cur && !cur.IsInterface)
                {
                    if (VerifyGenericArguments(GetFullTypeDefinition(parent), cur))
                        if (VerifyGenericArguments(parent, child))
                            return true;
                }
                else
                    foreach (var item in child.GetInterfaces().Where(i => GetFullTypeDefinition(parent) == GetFullTypeDefinition(i)))
                        if (VerifyGenericArguments(parent, item))
                            return true;

            child = child.BaseType;
        }

        return false;
    }

    private static Type GetFullTypeDefinition(Type type)
    {
        return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
    }

    private static bool VerifyGenericArguments(Type parent, Type child)
    {
        Type[] childArguments = child.GetGenericArguments();
        Type[] parentArguments = parent.GetGenericArguments();
        if (childArguments.Length == parentArguments.Length)
            for (int i = 0; i < childArguments.Length; i++)
                if (childArguments[i].Assembly != parentArguments[i].Assembly || childArguments[i].Name != parentArguments[i].Name || childArguments[i].Namespace != parentArguments[i].Namespace)
                    if (!childArguments[i].IsSubclassOf(parentArguments[i]))
                        return false;

        return true;
    }

    public static string RelationWith(this Type lhs, Type rhs, bool useFullName = false)
    {
        var str = "";
        var lhsName = lhs.Name;
        var rhsName = rhs.Name;
        if (useFullName)
        {
            lhsName = lhs.FullName;
            rhsName = rhs.FullName;
        }
        str += $"{lhsName}.IsAssignableFrom({rhsName}) : {lhs.IsAssignableFrom(rhs)} \n";
        str += $"{lhsName}.IsSubclassOf({rhsName}) : {lhs.IsSubclassOf(rhs)} \n";
        str += $"{lhsName}.IsEquivalentTo({rhsName}) : {lhs.IsEquivalentTo(rhs)} \n";
        str += $"{lhsName}.IsDefined({rhsName},true) : {lhs.IsDefined(rhs, true)} \n";
        str += $"{lhsName}.IsDefined({rhsName},false) : {lhs.IsDefined(rhs, false)} \n";
        str += $"{lhsName}.IsSubclassOfRawGeneric({rhsName}) : {lhs.IsSubclassOfRawGeneric(rhs)} \n";
        str += $"{lhsName}.IsSubClassOfGeneric({rhsName}) : {lhs.IsSubClassOfGeneric(rhs)} \n";
        return str;
    }
}


