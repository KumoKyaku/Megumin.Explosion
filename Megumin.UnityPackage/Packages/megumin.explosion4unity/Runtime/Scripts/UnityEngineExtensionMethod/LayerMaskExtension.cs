using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtension_33863E1A817A4BC4ACDF04E8E8192E92
{
    /// <summary>
    /// https://forum.unity.com/threads/get-the-layernumber-from-a-layermask.114553/#post-5890667
    /// </summary>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static int FirstSetLayer(this LayerMask mask)
    {
        int value = mask.value;
        if (value == 0) return 0;  // Early out
        for (int l = 1; l < 32; l++)
        {
            if ((value & (1 << l)) != 0)
            {
                return l;  // Bitwise
            }
        }

        return -1;  // This line won't ever be reached but the compiler needs it
    }

    /// <summary>
    /// Returns a list of values between [0;31].
    /// </summary>
    /// https://forum.unity.com/threads/get-the-layernumber-from-a-layermask.114553/#post-5144711
    public static List<int> GetAllLayerMaskInspectorIndex(this LayerMask mask)
    {
        List<int> layers = new List<int>();
        var bitmask = mask.value;
        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & bitmask) != 0)
            {
                layers.Add(i);
            }
        }
        return layers;
    }
}
