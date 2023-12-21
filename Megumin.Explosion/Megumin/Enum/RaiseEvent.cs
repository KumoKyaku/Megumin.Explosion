using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 触发事件类型
    /// </summary>
    public enum RaiseEvent
    {
        Ignore = -1,
        Default = 0,
        Auto = 1,
        Smart = 11,
        /// <summary>
        /// 强制触发
        /// </summary>
        ForceRaise = 101,
    }
}



