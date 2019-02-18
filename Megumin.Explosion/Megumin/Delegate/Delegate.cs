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
    /// <param name="new_">新的值</param>
    /// <param name="old_">旧的值</param>
    public delegate void OnValueChanged<in T>(T new_, T old_);
}
