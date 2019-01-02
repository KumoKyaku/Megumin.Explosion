using System.Collections;
using UnityEngine.Internal;

/// <summary>
/// Unity并没有实现这些接口，这些接口很多时候用于泛型约束,
/// 或者你有一个自定义接口，你的接口又想使用MonoBehaviour的某些东西时。
/// Todo: 这些接口以后会详细拆分
/// </summary>
namespace UnityEngine
{
    public interface IObject
    {
        string name { get; set; }
        HideFlags hideFlags { get; set; }
        int GetInstanceID();
    }

    public interface IComponent
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        string tag { get; set; }
        T GetComponent<T>();
    }

    public interface IBehaviour: IComponent
    {
        bool enabled { get; set; }
        bool isActiveAndEnabled { get; }
    }

    public interface IMonoBehaviour : IBehaviour
    {
        bool runInEditMode { get; set; }
        Coroutine StartCoroutine(string methodName);
        Coroutine StartCoroutine(IEnumerator routine);
        Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value);
        void StopAllCoroutines();
        void StopCoroutine(IEnumerator routine);
        void StopCoroutine(Coroutine routine);
        void StopCoroutine(string methodName);
    }
}
