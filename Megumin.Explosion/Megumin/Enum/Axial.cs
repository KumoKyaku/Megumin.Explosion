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
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2,
    }
}
