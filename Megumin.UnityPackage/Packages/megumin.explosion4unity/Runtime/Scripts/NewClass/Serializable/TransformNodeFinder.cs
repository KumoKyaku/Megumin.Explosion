using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    public interface IFinder<in K, out V>
    {
        V Find(K key);
    }

    public interface IRegister<in K, in V>
    {
        bool Regist(K key, V value);
    }

    /// <summary>
    /// 挂载点查找器
    /// </summary>
    public interface INodeFinder<in K, T> : IFinder<K, T>, IRegister<K, T>
    {

    }

    public interface ITransformNodeFinder<in K> : INodeFinder<K, Transform> { }

    //不要将多个Key和Transform包装成类型做值。那样是错误设计。多个Key可以对用同一个值。
    //不同类型的Key也不一定能完全对应。
    [Serializable]
    public class TransformNodeFinder
    {
        public Transform NotFound;
        public Dictionary<string, Transform> StringFinder = new Dictionary<string, Transform>();
        /// <summary>
        /// TODO,合并,多个name对应一个transfrom,Dic没法实现,需要使用DataSet.
        /// </summary>
        //public Dictionary<Transform, string> transFinder = new Dictionary<Transform, string>();

        public Transform Find(string key)
        {
            if (StringFinder.TryGetValue(key, out var iD))
            {
                return iD.transform;
            }
            else
            {
                Debug.LogWarning($"挂载点{key}没找到".HtmlColor(HexColor.CGRed));
            }

            return NotFound;
        }
    }
}

