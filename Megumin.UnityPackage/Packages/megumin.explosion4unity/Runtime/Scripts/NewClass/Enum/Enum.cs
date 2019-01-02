using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 刷新类型
    /// </summary>
    [Flags]
    public enum UpdateType
    {
        Update = 1 << 0,
        LateUpdate = 1 << 1,
        FixedUpdate = 1 << 2,
    }

    /// <summary>
    /// 插值类型
    /// </summary>
    [Flags]
    public enum LerpType
    {
        Lerp = 1 << 0,
        LerpUnclamped = 1 << 1,
        Slerp = 1 << 2,
        SlerpUnclamped = 1 << 3,
    }
}
