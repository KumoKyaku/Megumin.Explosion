﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Megumin;

namespace System.Reflection
{
    public static class FieldInfoExtension_E5A239CE4DB34975A4A939379BA9A036
    {
        public static void TryAddType(HashSet<Type> allTypes, SupportTypesAttribute ab, Type[] supporttypes, Assembly assembly)
        {
            if (ab.IncludeChildInSameAssembly)
            {
                Type[] types = assembly.GetTypes();
                foreach (var temptype in types)
                {
                    if (supporttypes.Any(ele => Match(ele, temptype)))
                    {
                        TryAdd2allTypes(temptype, ab, allTypes);
                    }
                }
            }
            else
            {
                foreach (var item in supporttypes)
                {
                    TryAdd2allTypes(item, ab, allTypes);
                }
            }
        }

        public static void TryAddTypeFromAssembly(HashSet<Type> allTypes, SupportTypesAttribute ab, Type supporttypes, Assembly assembly)
        {
            if (ab.IncludeChildInSameAssembly)
            {
                Type[] types = assembly.GetTypes();
                foreach (var temptype in types)
                {
                    if (Match(supporttypes, temptype))
                    {
                        TryAdd2allTypes(temptype, ab, allTypes);
                    }
                }
            }
            else
            {
                TryAdd2allTypes(supporttypes, ab, allTypes);
            }
        }

        static bool Match(this Type parent, Type toCheck)
        {
            if (parent.IsAssignableFrom(toCheck))
            {
                //测试类型能 赋值给 支持类型列表 中的任意一个。
                return true;
            }

            if (parent.IsSubclassOfRawGeneric(toCheck))
            {
                //测试泛型
                return true;
            }
            return false;
        }

        public static void TryAddType(Type type, SupportTypesAttribute ab, HashSet<Type> allTypes)
        {
            TryAddType(type, ab.IncludeChildInSameAssembly, ab.AllowInterface, ab.AllowAbstract, ab.AllowGenericType, allTypes);
        }

        public static void TryAdd2allTypes(Type type, SupportTypesAttribute ab, HashSet<Type> allTypes)
        {
            TryAdd2allTypes(type, ab.AllowInterface, ab.AllowAbstract, ab.AllowGenericType, allTypes);
        }

        public static void TryAddType(Type type, bool includeChildInSameAssembly, bool allowInterface, bool allowAbstract, bool allowGenericType, HashSet<Type> allTypes)
        {
            if (includeChildInSameAssembly)
            {
                Assembly assembly = type.Assembly;
                Type[] types = assembly.GetTypes();
                foreach (var item in types)
                {
                    if (type.Match(item))
                    {
                        TryAdd2allTypes(item, allowInterface, allowAbstract, allowGenericType, allTypes);
                    }
                }
            }
            else
            {
                TryAdd2allTypes(type, allowInterface, allowAbstract, allowGenericType, allTypes);
            }
        }

        public static void TryAdd2allTypes(Type type, bool allowInterface, bool allowAbstract, bool allowGenericType, HashSet<Type> allTypes)
        {
            if (!allowInterface && type.IsInterface)
            {
                return;
            }

            if (!allowAbstract && type.IsAbstract)
            {
                return;
            }

            if (!allowGenericType && type.IsGenericType)
            {
                return;
            }

            allTypes.Add(type);
        }

        /// <summary>
        /// 反射查找<see cref="SupportTypesAttribute"/>设定中支持的类型。
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="allTypes">结果集</param>
        /// <param name="assemblyFilter">程序集过滤回调</param>
        /// <param name="collectSelfTypeIfNoAttribute">没有SupportTypesAttribute时，使用自身类型作为支持类型</param>
        /// <param name="useGenericArgumentsTypeInsteadCollectionType">遇到泛型集合时，使用泛型参数类型代替集合类型</param>
        /// <param name="extraCheck">额外搜索的程序集</param>
        public static void CollectSupportType(this FieldInfo fieldInfo,
                                            HashSet<Type> allTypes,
                                            Func<Assembly, bool> assemblyFilter = null,
                                            bool collectSelfTypeIfNoAttribute = false,
                                            bool useGenericArgumentsTypeInsteadCollectionType = true,
                                            IEnumerable<Assembly> extraCheck = null)
        {
            var customattributes = fieldInfo.GetCustomAttributes(true);
            var abs = from cab in customattributes
                      where cab is SupportTypesAttribute
                      let sa = cab as SupportTypesAttribute
                      select sa;

#if MEGUMIN_DEV_PROJECT
            var debug = abs.ToList();
#endif

            foreach (var ab in abs)
            {
                if (ab != null)
                {
                    //包含所有程序集，搜索方式遍历所有程序集,可能会特别慢
                    Type[] supporttypes = null;
                    if (ab.Support == null || ab.Support.Length == 0 || ab.Support[0] == null)
                    {
                        var type = fieldInfo.FieldType.GetInnerType(useGenericArgumentsTypeInsteadCollectionType);
                        supporttypes = new Type[] { type };
                    }
                    else
                    {
                        supporttypes = ab.Support;
                    }

                    IEnumerable<Assembly> assemblys = null;
                    if (ab.IncludeChildInOtherAssembly)
                    {
                        assemblys = AppDomain.CurrentDomain.GetAssemblies();
                    }
                    else
                    {
                        var set = new HashSet<Assembly>();
                        foreach (var item in supporttypes)
                        {
                            set.Add(item.Assembly);
                        }
                        assemblys = set;
                        if (extraCheck != null)
                        {
                            assemblys = assemblys.Concat(extraCheck);
                        }
                    }

                    foreach (var assembly in assemblys)
                    {
                        //过滤掉一些，不然肯能太卡
                        if (assemblyFilter != null)
                        {
                            if (!assemblyFilter.Invoke(assembly))
                            {
                                //过滤一些程序集
                                continue;
                            }
                        }

                        TryAddType(allTypes, ab, supporttypes, assembly);
                    }
                }
            }

            if (collectSelfTypeIfNoAttribute && abs.Count() == 0)
            {
                //没有特性标记时使用自身类型搜索一次
                var fieldType = fieldInfo.FieldType;
                Type type = fieldType.GetInnerType(useGenericArgumentsTypeInsteadCollectionType);

                TryAddType(type, SupportTypesAttribute.Default, allTypes);
                if (extraCheck != null)
                {
                    foreach (var item in extraCheck)
                    {
                        TryAddTypeFromAssembly(allTypes, SupportTypesAttribute.Default, type, item);
                    }
                }
            }
        }
    }
}



