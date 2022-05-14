using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Megumin;

namespace System.Reflection
{
    public static class FieldInfoExtension_E5A239CE4DB34975A4A939379BA9A036
    {
        public static void TryAddType(Type type, SupportTypesAttribute ab, HashSet<Type> allTypes)
        {
            if (ab.IncludeChildInSameAssembly)
            {
                Assembly assembly = type.Assembly;
                Type[] types = assembly.GetTypes();
                foreach (var item in types)
                {
                    if (type.IsAssignableFrom(item))
                    {
                        TryAdd2allTypes(item, ab, allTypes);
                    }
                }
            }
            else
            {
                TryAdd2allTypes(type, ab, allTypes);
            }
        }

        public static void TryAdd2allTypes(Type type, SupportTypesAttribute ab, HashSet<Type> allTypes)
        {
            if (!ab.AllowInterface && type.IsInterface)
            {
                return;
            }

            if (!ab.AllowAbstract && type.IsAbstract)
            {
                return;
            }

            if (!ab.AllowGenericType && type.IsGenericType)
            {
                return;
            }

            allTypes.Add(type);
        }

        public static void TryAddType(Type type, bool includeChildInSameAssembly, bool allowInterface, bool allowAbstract, bool allowGenericType, HashSet<Type> allTypes)
        {
            if (includeChildInSameAssembly)
            {
                Assembly assembly = type.Assembly;
                Type[] types = assembly.GetTypes();
                foreach (var item in types)
                {
                    if (type.IsAssignableFrom(item))
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
        /// <param name="allTypes"></param>
        /// <param name="assemblyFilter"></param>
        public static void CollectSupportType(this FieldInfo fieldInfo,
                                            HashSet<Type> allTypes,
                                            Func<Assembly, bool> assemblyFilter = null)
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
                    if (ab.IncludeChildInOtherAssembly)
                    {
                        //包含所有程序集，搜索方式遍历所有程序集,可能会特别慢
                        Type[] supporttypes = null;
                        if (ab.Support == null || ab.Support.Length == 0 || ab.Support[0] == null)
                        {
                            var type = fieldInfo.FieldType;
                            supporttypes = new Type[] { type };
                        }
                        else
                        {
                            supporttypes = ab.Support;
                        }

                        var assemblys = AppDomain.CurrentDomain.GetAssemblies();
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

                            Type[] types = assembly.GetTypes();
                            foreach (var temptype in types)
                            {
                                if (supporttypes.Any(ele =>
                                {
                                    if (ele.IsAssignableFrom(temptype))
                                    {
                                        //测试类型能 赋值给 支持类型列表 中的任意一个。
                                        return true;
                                    }

                                    if (ele.IsSubclassOfRawGeneric(temptype))
                                    {
                                        //测试泛型
                                        return true;
                                    }
                                    return false;
                                }))
                                {
                                    TryAdd2allTypes(temptype, ab, allTypes);
                                }
                            }
                        }
                    }
                    else
                    {
                        //不包含其他程序集，逐个类型搜索
                        if (ab.Support == null || ab.Support.Length == 0 || ab.Support[0] == null)
                        {
                            var type = fieldInfo.FieldType;
                            TryAddType(type, ab, allTypes);
                        }
                        else
                        {
                            for (int i = 0; i < ab.Support.Length; i++)
                            {
                                var type = ab.Support[i];
                                if (type != null)
                                {
                                    TryAddType(type, ab, allTypes);
                                }
                            }
                        }
                    }
                }
            }

            if (abs.Count() == 0)
            {
                //没有特性标记时使用自身类型搜索一次
                TryAddType(fieldInfo.FieldType, SupportTypesAttribute.Default, allTypes);
            }
        }
    }
}



