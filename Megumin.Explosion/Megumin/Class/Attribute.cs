using System;
using System.ComponentModel;

namespace Megumin
{
    ///原则，能不定义的特性，就不要定义。能不定义的枚举，就不要定义。
    ///能不定义的接口，就不要定义。接口可能都是特例，而且需要最小原则，大多时找不到能代替的。
    ///使用反射先做个所有特性列表 枚举列表，生一个代码方便F1, F12，所有特性都new一个，看名字那个顺眼用哪个。
    ///冲突与语义，尽量选功能交叉比较小的特性去使用。避免功能冲突。
    ///尽量选择AttributeTargets.All  AllowMultiple = true, Inherited = true 一致的。
    ///很多特性用于传参，其实可以用字符串传参。这样原生特性选择性就大大提高。
    ///例如：
    ///<see cref="CategoryAttribute"/>
    ///<see cref="DescriptionAttribute"/>
    ///<see cref="AttributeProviderAttribute"/>
    ///<see cref="LookupBindingPropertiesAttribute"/>

    ///尽量删除自己的特性，改用原生特性。
    ///为了兼容第三方插件，尽量不要声明自己的特性，使用.net 原生特性（或者unity特性，尽可能不用）。
    ///unity 项目中或者unity插件就无法避免要从 PropertyAttribute，不过能用unity原生特性就尽量原生。
    ///第三方插件与行为树做兼容时就不必引用你的包。
    ///这样很多特性就不必定义了。少即是多。

    ///既然设计理念是一个特性可以用于不同的功能，那么为了方式冲突，特性中应该增几个一个识别字段 Feature,平时不用保持为空。
    ///冲突时可以增加一个标记用于检测。检测 Feature == null || Feature.Contain “FeatureName” 才确认执行特性。

    ///同理。通用枚举也尽量不要用，改用string或者int，跨模块传参时更方便，避免不必要耦合。

    ///接口则不在这个范围，定义接口通常都是十分确切的功能。除非功能和.net底层接口重合。
    ///C# 泛型没有成员约束，不然就不用定义接口了。

    /// <summary>
    /// 别名
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// <para>设计理念是一个特性可以用于不同的功能，冲突时这个标记用于检测。</para>
        /// 检测 <![CDATA[string.IsNullOrEmpty(Feature) || Feature.Contains("FeatureName")]]> 才确认执行特性。
        /// </summary>
        public string Feature { get; set; }

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

    [Obsolete("", true)]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ShortNameAttribute : Attribute
    {
        /// <summary>
        /// <para>设计理念是一个特性可以用于不同的功能，冲突时这个标记用于检测。</para>
        /// 检测 <![CDATA[string.IsNullOrEmpty(Feature) || Feature.Contains("FeatureName")]]> 才确认执行特性。
        /// </summary>
        public string Feature { get; set; }

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
        public static readonly SupportTypesAttribute Default = new SupportTypesAttribute();

        /// <summary>
        /// <para>设计理念是一个特性可以用于不同的功能，冲突时这个标记用于检测。</para>
        /// 检测 <![CDATA[string.IsNullOrEmpty(Feature) || Feature.Contains("FeatureName")]]> 才确认执行特性。
        /// </summary>
        public string Feature { get; set; }

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

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class SupportTypesAttribute<T> : SupportTypesAttribute
    {
        public SupportTypesAttribute()
        {
            Support = new Type[1];
            Support[0] = typeof(T);
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class SupportTypesAttribute<T1, T2> : SupportTypesAttribute
    {
        public SupportTypesAttribute()
        {
            Support = new Type[2];
            Support[0] = typeof(T1);
            Support[1] = typeof(T2);
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class SupportTypesAttribute<T1, T2, T3> : SupportTypesAttribute
    {
        public SupportTypesAttribute()
        {
            Support = new Type[3];
            Support[0] = typeof(T1);
            Support[1] = typeof(T2);
            Support[2] = typeof(T3);
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class SupportTypesAttribute<T1, T2, T3, T4> : SupportTypesAttribute
    {
        public SupportTypesAttribute()
        {
            Support = new Type[4];
            Support[0] = typeof(T1);
            Support[1] = typeof(T2);
            Support[2] = typeof(T3);
            Support[3] = typeof(T4);
        }
    }

    /// <summary>
    /// 生成名字时的生成方式.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class GenarateNameAttribute : Attribute
    {
        /// <summary>
        /// <para>设计理念是一个特性可以用于不同的功能，冲突时这个标记用于检测。</para>
        /// 检测 <![CDATA[string.IsNullOrEmpty(Feature) || Feature.Contains("FeatureName")]]> 才确认执行特性。
        /// </summary>
        public string Feature { get; set; }

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
        /// <summary>
        /// <para>设计理念是一个特性可以用于不同的功能，冲突时这个标记用于检测。</para>
        /// 检测 <![CDATA[string.IsNullOrEmpty(Feature) || Feature.Contains("FeatureName")]]> 才确认执行特性。
        /// </summary>
        public string Feature { get; set; }
    }

    /// <summary>
    /// <para>todo, 现在仅部分按键有效 </para>
    /// 在unity中,仅编辑器有效,回调标记的方法
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class OnKeyAttribute : Attribute
    {
        /// <summary>
        /// <para>设计理念是一个特性可以用于不同的功能，冲突时这个标记用于检测。</para>
        /// 检测 <![CDATA[string.IsNullOrEmpty(Feature) || Feature.Contains("FeatureName")]]> 才确认执行特性。
        /// </summary>
        public string Feature { get; set; }

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




