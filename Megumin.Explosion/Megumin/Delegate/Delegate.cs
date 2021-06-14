using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 当值发生改变时被调用
    /// <para>因为 new 是关键字，所以参数带有下划线</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newValue">新的值</param>
    /// <param name="oldValue">旧的值</param>
    public delegate void OnValueChanged<in T>(T newValue, T oldValue);
}
