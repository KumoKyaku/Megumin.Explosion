using Megumin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class TransformExtension_1356FE83A31E4D0ABE20837814D1C94D
{
    /// <summary>
    /// 重置local Vector3.zero Quaternion.identity Vector3.one
    /// </summary>
    /// <param name="trans"></param>
    public static void ResetLocal(this Transform trans)
    {
        trans.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        trans.localScale = Vector3.one;
    }

    /// <summary>
    /// 重置位置和旋转
    /// </summary>
    /// <param name="trans"></param>
    public static void ResetPosAndRot(this Transform trans)
    {
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// 位置重合
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="tar"></param>
    public static void AlignFrom(this Transform trans, Transform tar)
    {
        if (tar)
        {
            trans.SetPositionAndRotation(tar.position, tar.rotation);
            trans.localScale = tar.localScale;
        }
    }

    /// <summary>
    /// 切换第一个子的开启关闭
    /// </summary>
    /// <param name="transform"></param>
    public static void ToggleChild0(this Transform transform)
    {
        if (transform && transform.childCount > 0)
        {
            var child0 = transform.GetChild(0);
            if (child0)
            {
                child0.gameObject.SetActive(!child0.gameObject.activeSelf);
            }
        }
    }
}
