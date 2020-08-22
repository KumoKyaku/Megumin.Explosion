using Megumin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class VectorExtension_DC454F9ED17B4327A47F7EF4F0E76DAF
{
    static readonly Vector3 zeroY = new Vector3(1, 0, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZeroY(this in Vector3 v)
    {
        return new Vector3(v.x, 0, v.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Vector3 a, Vector3 b) => a = Vector3.Scale(a, b);

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
}
