using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 线程安全List池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>内部使用<see cref="ConcurrentStack{T}"/>实现</remarks>
    public class ListPool<T>
    {
        static ConcurrentStack<List<T>> pool = new ConcurrentStack<List<T>>();
        /// <summary>
        /// 默认容量10
        /// </summary>
        public static int MaxSize { get; set; } = 10;

        public static List<T> Rent()
        {
            if (pool.TryPop(out var list))
            {
                if (list == null || list.Count != 0)
                {
                    return new List<T>();
                }
                return list;
            }
            else
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// 调用者需要保证归还后不在使用当前list
        /// <para/>虽然调用后list被赋值为null，但不能保证没有其他引用指向当前list，尤其小心被保存在Linq语句中的引用。
        /// </summary>
        /// <param name="list"></param>
        /// <remarks>使用ref 来保证list被置为null,防止出现数据错误.</remarks>
        public static void Return(ref List<T> list)
        {
            if (list == null)
            {
                return;
            }

            if (pool.Count < MaxSize)
            {
                list.Clear();
                pool.Push(list);
            }

            list = null;
        }

        public static void Clear()
        {
            pool.Clear();
        }

        public struct ListHandle : IDisposable
        {
            internal List<T> list;
            public List<T> List => list;
            public void Dispose()
            {
                Return(ref list);
            }

            public static implicit operator List<T>(ListHandle handle)
            {
                return handle.list;
            }
        }

        public static ListHandle RentAutoReturn()
        {
            ListHandle handle = default;
            handle.list = Rent();
            return handle;
        }

        static void Test2()
        {
            using (var auto = RentAutoReturn())
            {

            }
        }
    }
}
