using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 字节数据扩展
/// </summary>
public static class ByteArrayExtension_293399541C8D4E38A03A6340FA15498E
{
    /// <summary>
    /// 这个数组是不是以BOM头开始的
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static bool StartWithBOM(this byte[] buffer)
    {
        if (buffer.Length >= 3)
        {
            if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 如果含有BOM头则去掉，不含有返回原数组
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static byte[] SkipBOMIfHave(this byte[] buffer)
    {
        if (buffer.StartWithBOM())
        {
            return buffer.Skip(3).ToArray();
        }
        else
        {
            return buffer;
        }
    }
}



