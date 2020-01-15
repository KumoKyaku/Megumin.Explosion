using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 左右
    /// </summary>
    public enum LeftOrRight
    {
        Left,
        Right,
        Center,
    }

    /// <summary>
    /// 顺时针 上右下左
    /// </summary>
    [Flags]
    public enum URDL
    {
        Up =  1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
    }


}
