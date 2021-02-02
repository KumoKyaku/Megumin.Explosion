using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtension_E574E5EC
{
    /// <summary>
    /// 使用Html标签包裹对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="color"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static string Html<T>(this in Color color, T target)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{target}</color>";
    }

    /// <summary>
    /// 使用Html标签包裹对象
    /// </summary>
    /// <param name="orignal"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Html(this string orignal, in Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{orignal}</color>";
    }
}

