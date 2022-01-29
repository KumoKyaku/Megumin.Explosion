using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 线程不安全List池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>内部使用<see cref="Stack{T}"/>实现</remarks>
    public static class ListPool<T>
    {
        /// <summary>
        /// 不要继承静态自动,父类和子类共有可能存在隐藏bug
        /// </summary>
        public static readonly StackPoolCore<List<T>> poolCore = new StackPoolCore<List<T>>();

        /// <summary>
        /// <inheritdoc cref="IPoolCore{T}.Rent()"/>
        /// </summary>
        /// <returns></returns>
        public static List<T> Rent()
        {
            return poolCore.Rent();
        }

        /// <summary>
        /// <inheritdoc cref="IPoolCore{T}.Return(ref T, bool)"/>
        /// </summary>
        /// <returns></returns>
        public static void Return(ref List<T> element, bool safeCheck = false)
        {
            poolCore.Return(ref element, safeCheck);
        }

        /// <summary>
        /// <inheritdoc cref="IPoolCore{T}.Rent(out T)"/>
        /// </summary>
        /// <returns></returns>
        public static IPoolCore<List<T>>.ReturnHandle Rent(out List<T> element)
        {
            return poolCore.Rent(out element);
        }
    }

    /// <summary>
    /// 线程安全List池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>内部使用<see cref="ConcurrentStack{T}"/>实现</remarks>
    public static class ConcurrentListPool<T>
    {
        /// <summary>
        /// 不要继承静态自动,父类和子类共有可能存在隐藏bug
        /// </summary>
        public static readonly ConcurrentStackPoolCore<List<T>> poolCore
             = new ConcurrentStackPoolCore<List<T>>();

        /// <summary>
        /// <inheritdoc cref="IPoolCore{T}.Rent()"/>
        /// </summary>
        /// <returns></returns>
        public static List<T> Rent()
        {
            return poolCore.Rent();
        }

        /// <summary>
        /// <inheritdoc cref="IPoolCore{T}.Return(ref T, bool)"/>
        /// </summary>
        /// <returns></returns>
        public static void Return(ref List<T> element, bool safeCheck = false)
        {
            poolCore.Return(ref element, safeCheck);
        }

        /// <summary>
        /// <inheritdoc cref="IPoolCore{T}.Rent(out T)"/>
        /// </summary>
        /// <returns></returns>
        public static IPoolCore<List<T>>.ReturnHandle Rent(out List<T> element)
        {
            return poolCore.Rent(out element);
        }
    }
}
