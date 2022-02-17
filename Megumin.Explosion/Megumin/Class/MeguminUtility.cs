using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 常用工具
    /// </summary>
    [Obsolete("Use Megumin.Utility instead.")]
    public class MeguminUtility
    {
        /// <summary>
        /// 反射出类的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Obsolete("Use ToStringReflection Instead.")]
        public static string Detail<T>(T value = default)
            => value.ToStringReflection();

        void Test()
        {
            LinkedList<int> list = new LinkedList<int>();
        }
    }
}
