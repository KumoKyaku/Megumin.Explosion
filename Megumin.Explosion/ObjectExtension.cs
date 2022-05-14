using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 反射复制字段属性的值,近似匹配
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="useNameFindMode"></param>
        /// <param name="flags"></param>
        public static void SimilarityCopyTo(this object source,
                                            object target,
                                            bool useNameFindMode = true,
                                            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
        {
            if (source is null)
            {
                return;
            }

            if (target is null)
            {
                return;
            }

            var targetType = target.GetType();
            var ms = targetType.GetMembers(flags);
            foreach (var item in ms)
            {
                try
                {
                    if (item is FieldInfo field)
                    {
                        object oriValue = null;
                        try
                        {
                            oriValue = field.GetValue(source);
                        }
                        catch (Exception)
                        {
                            if (useNameFindMode)
                            {
                                oriValue = source.GetType().GetField(field.Name, flags)?.GetValue(source);
                            }
                        }

                        if (oriValue is not null)
                        {
                            field.SetValue(target, oriValue);
                        }
                    }

                    if (item is PropertyInfo property)
                    {
                        object oriValue = null;
                        try
                        {
                            oriValue = property.GetValue(source);
                        }
                        catch (Exception)
                        {
                            if (useNameFindMode)
                            {
                                oriValue = source.GetType().GetProperty(property.Name, flags)?.GetValue(source);
                            }
                        }

                        if (oriValue is not null)
                        {
                            property.SetValue(target, oriValue);
                        }
                    }
                }
                catch (System.Exception)
                {
                }
            }
        }
    }
}



