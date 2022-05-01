using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 轴向
    /// </summary>
    [Flags]
    public enum Axial
    {
        None = 0x0,
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2,
    }

    /// <summary>
    /// 带符号的轴向
    /// </summary>
    public enum AxialAigned
    {
        None = 0x0,
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2,
        /// <summary>
        /// 负x
        /// </summary>
        Xn = 1 << 3,
        /// <summary>
        /// 负y
        /// </summary>
        Yn = 1 << 4,
        /// <summary>
        /// 负z
        /// </summary>
        Zn = 1 << 5,
    }
}
