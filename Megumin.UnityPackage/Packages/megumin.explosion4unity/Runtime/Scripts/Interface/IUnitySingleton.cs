using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;

namespace Megumin
{
    [Obsolete("不明原因编辑器崩溃", true)]
    public interface IUnitySingleton
    {
        public static bool DontCreate { get; set; } = false;

        /// <summary>
        /// 可能会调试崩溃
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hideFlags"></param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                    UnityEngine.Object.DontDestroyOnLoad(go);
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
    [Obsolete("不明原因编辑器崩溃", true)]
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
        /// <remarks>遇到个bug,通过接口默认实现静态函数,正常运行没事,一旦调试下断点,编辑器就会崩溃.</remarks>
        [System.Diagnostics.DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySetInstance(T i)
        {
            if (instance)
            {
                if (instance != i)
                {
                    UnityEngine.Object.Destroy(i);
                    return false;
                }
            }
            else
            {
                instance = i;
                UnityEngine.Object.DontDestroyOnLoad(i.gameObject);
            }

            return true;
        }
    }

    public class UniSingleton
    {
        public static bool DontCreate { get; set; } = false;
        public static HideFlags HideFlags { get; set; } = HideFlags.None;
        public static T CreateSingleton<T>(HideFlags? hideFlags = null)
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
                    UnityEngine.Object.DontDestroyOnLoad(go);
                }

                if (hideFlags.HasValue)
                {
                    go.hideFlags = hideFlags.Value;
                }
                else
                {
                    go.hideFlags = HideFlags;
                }

                var com = go.GetComponent<T>();
                if (!com)
                {
                    com = go.AddComponent<T>();
                }
                return com;
            }
        }
    }
}


