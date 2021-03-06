﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 左中右
    /// </summary>
    [Flags]
    public enum LeftOrRight
    {
        Left = 1 << 0,
        Right = 1 << 1,
        Center = 1 << 2,
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
    /// 九宫格 顺时针布局
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

    /// <summary>
    /// 九宫格 小键盘布局
    /// <para> 7 8 9 </para>
    /// <para> 4 5 6 </para>
    /// <para> 1 2 3 </para>
    /// </summary>
    [Flags]
    public enum KeypadSudoku : short
    {
        None = 0,
        /// <summary>
        /// 负中心，没什么实际意义，因为 第0位 空着，所以当作填充用。
        /// </summary>
        NegativeCenter = 1 << 0,
        LeftDown = 1 << 1,
        Down = 1 << 2,
        RightDown = 1 << 3,
        Left = 1 << 4,
        Center = 1 << 5,
        Right = 1 << 6,
        LeftUp = 1 << 7,
        Up = 1 << 8,
        RightUp = 1 << 9,
        All = 0b1111_1_1111_1,
    }
}
