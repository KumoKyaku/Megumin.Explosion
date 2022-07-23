using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialExtension_7CAFE5AED3324A51BEC8445DE82B38C6
{
    public static int ColorID = Shader.PropertyToID("_Color");
    public static int BaseColorID = Shader.PropertyToID("_BaseColor");

    public static Color MainColor(this Material material, Color? defaultColor = null)
    {
        if (material)
        {
            if (material.HasColor(ColorID))
            {
                return material.color;
            }

            if (material.HasColor(BaseColorID))
            {
                return material.GetColor(BaseColorID);
            }
        }
        return defaultColor ?? Color.magenta;
    }

    public static bool SetMainColor(this Material material, Color color)
    {
        if (material)
        {
            if (material.HasColor(ColorID))
            {
                material.color = color;
                return true;
            }

            if (material.HasColor(BaseColorID))
            {
                material.SetColor(BaseColorID, color);
                return true;
            }
        }
        return false;
    }
}
