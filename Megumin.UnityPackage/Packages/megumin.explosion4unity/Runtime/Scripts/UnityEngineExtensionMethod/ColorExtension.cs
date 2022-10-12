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
    public static string Html<T>(this in Color color, T target, bool force = false)
    {
        return target.ToString().Html(color, force);
    }

    /// <inheritdoc cref="Html(string, in Color, bool)"/>
    public static string HtmlColor<T>(this T target, in Color color, bool force = false)
    {
        return target.ToString().Html(color, force);
    }

    public static string HtmlColor(this bool target, bool force = false)
    {
        return target.ToString().Html(target ? HexColor.DarkGreenX : HexColor.DarkRed, force);
    }

    /// <summary>
    /// 使用Html标签包裹对象
    /// </summary>
    /// <param name="orignal"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    /// <remarks>通常仅编辑器有效,运行时原样返回,因为编辑器支持颜色,运行时可能会输出到日志文件里.</remarks>
    public static string Html(this string orignal, in Color color, bool force = false)
    {

#if UNITY_EDITOR
        force = true;
#endif
        var result = orignal;
        if (force)
        {
            result = $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{result}</color>";
        }
        return result;
    }
}

