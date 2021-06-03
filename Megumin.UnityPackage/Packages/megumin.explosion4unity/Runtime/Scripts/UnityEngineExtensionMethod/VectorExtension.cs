using Megumin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class VectorExtension_DC454F9ED17B4327A47F7EF4F0E76DAF
{
    /// <summary>
    /// 返回当前y=0的新的Vector3
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 GetMuteY(this in Vector3 v)
    {
        return new Vector3(v.x, 0, v.z);
    }

    public static Vector3Int ToVector3Int(this IXYZ<int> value)
    {
        if (value == null)
        {
            return default;
        }

        return new Vector3Int(value.X, value.Y, value.Z);
    }

    public static Vector3 ParseVector3(this string str)
    {
        var parts = str.Split('_');

        return new Vector3(float.Parse(parts[0]),
            float.Parse(parts[1]),
            float.Parse(parts[2]));
    }

    public static (int X, int Y) ToV2(this string str)
    {
        var parts = str.Split('_');

        return (int.Parse(parts[0]),
            int.Parse(parts[1]));
    }

    public static Vector3 GetPosition(this in Matrix4x4 matrix)
    {
        return new Vector3(matrix.m03, matrix.m13, matrix.m23);
        //return default;
    }

    public static readonly Quaternion TurnRightQ = Quaternion.Euler(0, 90, 0);
    public static readonly Quaternion TurnLeftQ = Quaternion.Euler(0, -90, 0);

    /// <summary>
    /// y轴转90
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 TurnRight(this in Vector3 vector)
    {
        return TurnRightQ * vector;
    }

    /// <summary>
    /// y轴转-90
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 TurnLeft(this in Vector3 vector)
    {
        return TurnLeftQ * vector;
    }

    /// <summary>
    /// 将相对位置转换为4方向混合动画参数
    /// </summary>
    /// <param name="orignal"></param>
    /// <returns></returns>
    public static Vector2 PosOffsetTo4DirPara(this in Vector2 orignal,
                                              float scale = 1,
                                              float maxAxisValue = 1,
                                              float deadzonex = 0.01f,
                                              float deadzoney = 0.01f)
    {
        var temp = orignal * scale;

        float CaxAxis(float v, float deadzone, float maxValue = 1)
        {
            var res = v;
            if (Mathf.Abs(res) < deadzone)
            {
                res = 0;
            }
            else
            {
                if (res > maxValue)
                {
                    res = 1;
                }
                else if (res < maxValue * -1)
                {
                    res = maxValue * -1;
                }
            }
            return res;
        }
        var x = CaxAxis(temp.x, deadzonex, maxAxisValue);
        var y = CaxAxis(temp.y, deadzoney, maxAxisValue);

        return new Vector2(x, y);
    }
}









