using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace UnityEngine
{
    public static class VectorExtension_DC454F9ED17B4327A47F7EF4F0E76DAF
    {
        static readonly Vector3 zeroY = new Vector3(1, 0, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ZeroY(this in Vector3 v)
        {
            return Vector3.Scale(v,zeroY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Scale(this ref Vector3 a, Vector3 b) => a = Vector3.Scale(a, b);
    }
}
