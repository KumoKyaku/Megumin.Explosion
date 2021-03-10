using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MonoBehaviourExtension_DA182CC20A33453FA684CD22CE5B97DC
{
    /// <summary>
    /// 协同轮询predicate的结果，直到结果为true，执行callback；
    /// </summary>
    /// <param name="script"></param>
    /// <param name="predicate">断言方法</param>
    /// <param name="callback">回调</param>
    public static void StartCoroutine(this MonoBehaviour script, Func<bool> predicate, Action callback)
    {
        script.StartCoroutine(WaitUntil(predicate, callback));
    }

    /// <summary>
    /// 等待一定时间，然后执行回调
    /// </summary>
    /// <param name="script"></param>
    /// <param name="waittime"></param>
    /// <param name="callback">回调</param>
    public static void StartCoroutine(this MonoBehaviour script, float waittime, Action callback)
    {
        script.StartCoroutine(WaitTime(waittime, callback));
    }

    private static IEnumerator WaitTime(float waittime, Action callback)
    {
        yield return new WaitForSeconds(waittime);
        if (callback != null)
        {
            callback();
        }
    }

    private static IEnumerator WaitUntil(Func<bool> predicate, Action callback)
    {
        yield return new WaitUntil(predicate);
        if (callback != null)
        {
            callback();
        }
    }

    /// <summary>
    /// 取得一个组件，如果没有就添加这个组件
    /// </summary>
    /// <typeparam name="T">目标组件</typeparam>
    /// <param name="monoBehaviour"></param>
    /// <returns>目标组件</returns>
    public static T GetComponentOrAdd<T>(this Behaviour monoBehaviour)
        where T : Component
    {
        var com = monoBehaviour.GetComponent<T>();
        if (com == null)
        {
            com = monoBehaviour.gameObject.AddComponent<T>();
        }

        return com;
    }

    /// <summary>
    /// 取得一个组件，如果没有就添加这个组件
    /// </summary>
    /// <typeparam name="T">目标组件</typeparam>
    /// <param name="monoBehaviour"></param>
    /// <param name="isNewAdd">返回是不是新添加的</param>
    /// <returns>目标组件</returns>
    public static T GetComponentOrAdd<T>(this Behaviour monoBehaviour, out bool isNewAdd)
        where T : Component
    {
        isNewAdd = false;
        var com = monoBehaviour.GetComponent<T>();
        if (com == null)
        {
            com = monoBehaviour.gameObject.AddComponent<T>();
            isNewAdd = true;
        }

        return com;
    }

    /// <summary>
    /// MonoBehaviour自身和继承的属性名字列表
    /// </summary>
    public static readonly string[] InnerPropertiesNames = new string[]
    {
            "gameObject","tag","name","hideFlags","transform",
            "useGUILayout","enabled","isActiveAndEnabled","runInEditMode",
            "animation","audio","camera","collider","collider2D","constantForce","guiElement",
            "guiText","guiTexture","hingeJoint","light","networkView","particleEmitter",
            "particleSystem","renderer","rigidbody","rigidbody2D",
    };

    /// <summary>
    /// 这里插入一个EditorUpdate达到刷效果，否则编辑器模式下脚本Update调用不及时。
    /// </summary>
    /// <param name="behaviour"></param>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void InspectorForceUpdate(this UnityEngine.Object behaviour)
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
#endif

    }
}
