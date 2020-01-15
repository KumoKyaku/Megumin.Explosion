using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 匹配器
    /// </summary>
    /// <typeparam name="T"><peparam>
    /// <typeparam name="K"><peparam>
    public interface IMatcher<in T, in K>
    {
        bool Match(T input, K target);
    }
}
