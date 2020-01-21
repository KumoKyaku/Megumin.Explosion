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
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
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
            trans.position = tar.position;
            trans.eulerAngles = tar.eulerAngles;
            trans.localScale = tar.localScale;
        }
    }

    /// <summary>
    /// 递归查找一个子transform
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform FindFisrtChild(this Transform trans, string name)
    {
        return FastRecursive(trans, name);
    }

    /// <summary>
    /// 递归查找
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    static Transform FastRecursive(Transform trans, string name)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            var tempchild = trans.GetChild(i);
            if (tempchild.name == name)
            {
                return tempchild;
            }
            else
            {
                var res = FastRecursive(tempchild, name);
                if (res)
                {
                    return res;
                }
            }
        }
        return null;
    }



    /// <summary>
    /// 递归查找
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    /// <param name="temp"></param>
    static void FindAllRecursive(this Transform transform, string name, List<Transform> temp)
    {
        foreach (Transform item in transform)
        {
            if (item && item.name == name)
            {
                temp.Add(item);
            }

            FindAllRecursive(item, name, temp);
        }
    }

    public static Transform[] FindAll(this Transform transform, string name, bool recursive = true)
    {
        List<Transform> temp = ListPool<Transform>.Rent();
        if (recursive)
        {
            FindAllRecursive(transform, name, temp);
        }
        else
        {
            foreach (Transform item in transform)
            {
                if (item && item.name == name)
                {
                    temp.Add(item);
                }
            }
        }

        var res = temp.ToArray();
        ListPool<Transform>.Return(temp);
        return res;
    }

}
