using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 对枚举类的扩展
    /// </summary>
    public static class EnumExtension_5108BEA26E8C44D9A538646E15B6B175
    {
        #if NETSTANDARD2_0
        /// <summary>
        /// 检查是否有FlagsAttribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool CheckFlags<T>() where T : Enum
        {
            bool result = Attribute.GetCustomAttribute(typeof(T), typeof(FlagsAttribute)) != null;
            if (!result)
            {
                throw new ArgumentException("类型不含有FlagsAttribute");
            }
            return result;
        }
#endif
    }
}
