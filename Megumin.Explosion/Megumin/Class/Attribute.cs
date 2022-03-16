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

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class SupportTypesAttribute : Attribute
    {
        public Type[] Support { get; set; }

        /// <summary>
        /// 是否包含其他程序集的子类型,默认false,支持，谨慎使用，可能会导致编辑器卡顿.
        /// </summary>
        public bool IncludeChildInOtherAssembly { get; set; } = false;

        /// <summary>
        /// 是否包含同一程序集的子类型,默认true
        /// </summary>
        /// <remarks></remarks>
        public bool IncludeChildInSameAssembly { get; set; } = true;

        /// <summary>
        /// 是否允许抽象类型,默认false
        /// </summary>
        public bool AllowAbstract { get; set; } = false;

        /// <summary>
        /// 是否允许接口类型,默认false
        /// </summary>
        public bool AllowInterface { get; set; } = false;

        /// <summary>
        /// 是否允许泛型类型,默认true
        /// </summary>
        public bool AllowGenericType { get; set; } = true;

        public SupportTypesAttribute(params Type[] types)
        {
            Support = types;
        }

        public SupportTypesAttribute(Type type)
        {
            Support = new Type[1];
            Support[0] = type;
        }

        public SupportTypesAttribute()
        {
            //空的化支持标记的类型。
        }
    }

    /// <summary>
    /// 生成名字时的生成方式.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class GenarateNameAttribute : Attribute
    {
        /// <summary>
        /// None,
        /// Fixed,
        /// Dynamic
        /// </summary>
        public string PrefixType { get; set; } = "None";
        public string Prefix { get; set; }

        public string PostfixType { get; set; } = "None";
        public string Postfix { get; set; }
    }

    /// <summary>
    /// 在unity中,仅编辑器有效,回调标记的方法
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class OnF1Attribute : Attribute
    {

    }

    /// <summary>
    /// <para>todo, 现在仅部分按键有效 </para>
    /// 在unity中,仅编辑器有效,回调标记的方法
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class OnKeyAttribute : Attribute
    {
        public ConsoleKey Key { get; set; }

        public OnKeyAttribute()
        {
        }

        public OnKeyAttribute(ConsoleKey key)
        {
            Key = key;
        }
    }
}




