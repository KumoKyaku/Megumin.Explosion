using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameObjectExtension_044A46672EEE4ED5BDE291E8DEC3012E
{
    /// <summary>
    /// 取得一个组件，如果没有就添加这个组件
    /// </summary>
    /// <typeparam name="T">目标组件</typeparam>
    /// <param name="go"></param>
    /// <returns>目标组件</returns>
    public static T GetComponentOrAdd<T>(this GameObject go)
        where T : Component
    {
        var com = go.GetComponent<T>();
        if (!com)
        {
            com = go.AddComponent<T>();
        }

        return com;
    }

    /// <summary>
    /// 更改包括子的层级
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layer"></param>
    public static void SetLayerOnAll(this GameObject obj, int layer)
    {
        ///据说GetComponentsInChildren更快
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layer;
        }
    }

    /// <summary>
    /// 更改包括子的层级
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="tag"></param>
    public static void SetTagOnAll(this GameObject obj, string tag)
    {
        ///据说GetComponentsInChildren更快
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.tag = tag;
        }
    }
}
