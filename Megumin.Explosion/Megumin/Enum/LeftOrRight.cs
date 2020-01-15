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

    /// <summary>
    /// 九宫格
    /// <para> 7 0 1 </para>
    /// <para> 6 8 2 </para>
    /// <para> 5 4 3 </para>
    /// </summary>
    [Flags]
    public enum Sudoku
    {
        Up = 1 << 0,
        RightUp = 1 << 1,
        Right = 1 << 2,
        RightDown = 1 << 3,

        Down = 1 << 4,
        LeftDown = 1 << 5,
        Left = 1 << 6,
        LeftUp = 1 << 7,
        
        Center = 1 << 8,
    }
}
