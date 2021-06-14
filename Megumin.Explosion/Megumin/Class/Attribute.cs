using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 别名
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        public AliasAttribute()
        {

        }

        public AliasAttribute(string alias)
        {
            Alias = alias;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ShortNameAttribute : Attribute
    {
        /// <summary>
        /// 别名
        /// </summary>
        public string Name { get; set; }

        public ShortNameAttribute()
        {

        }

        public ShortNameAttribute(string name)
        {
            Name = name;
        }
    }


    public class SupportTypesAttribute : Attribute
    {
        public Type[] Support { get; set; }
        public SupportTypesAttribute(params Type[] types)
        {
            Support = types;
        }
    }
}




