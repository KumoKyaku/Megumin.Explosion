using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    public interface IUnitySingleton
    {
        public static bool DontCreate { get; set; } = false;

        protected static T Singleton<T>(HideFlags hideFlags = HideFlags.None)
            where T : Component
        {
            if (DontCreate)
            {
                return default;
            }
            else
            {
                var name = typeof(T).Name;
                GameObject go = GameObject.Find(name);
                if (!go)
                {
                    go = new GameObject(typeof(T).Name);
                    Object.DontDestroyOnLoad(go);
                }

                go.hideFlags = hideFlags;
                var com = go.GetComponent<T>();
                if (!com)
                {
                    com = go.AddComponent<T>();
                }
                return com;
            }
        }
    }

    /// <summary>
    /// Unity组件自动创建单例. 慎用单例,能不用就不用.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnitySingleton<T> : IUnitySingleton
        where T : Component
    {
        //默认接口实现只能通过接口调用,没有意义
        //public T Instance2 => IUniySingleton<T>.Instance;

        private static T instance;
        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    instance = Singleton<T>();
                }
                return instance;
            }
        }

        /// <summary>
        /// 尝试将对象设置长instance,如果失败,则销毁对象
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool TrySetInstance(T i)
        {
            if (instance)
            {
                if (instance != i)
                {
                    Object.Destroy(i);
                    return false;
                }
            }
            else
            {
                instance = i;
                Object.DontDestroyOnLoad(i.gameObject);
            }

            return true;
        }
    }
}


