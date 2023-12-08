using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace System
{
    /// <summary>
    /// 对象池核心容器 
    /// 参考了UnityEngine.Rendering.ObjectPool,并做了优化.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPoolCore<T>
    {
        /// <summary>
        /// 从池中租用一个元素
        /// </summary>
        /// <returns></returns>
        T Rent();

        /// <summary>
        /// 将一个元素返回池,调用者需要保证归还后不在使用当前list
        /// <para/>虽然调用后list被赋值为null，但不能保证没有其他引用指向当前list，尤其小心被保存在Linq语句中的引用。
        /// </summary>
        /// <param name="element">使用ref 来保证list被置为null,防止出现数据错误.</param>
        /// <param name="forceSafeCheck">安全检查,检测最大保留个数,数据结构检测重复元素,有性能开销</param>
        void Return(ref T element, bool forceSafeCheck = false);

        /// <summary>
        /// 自动归还句柄,推荐使用using语法
        /// </summary>
        public struct ReturnHandle : IDisposable
        {
            T element;
            IPoolCore<T> poolCore;
            public bool ForceSafeCheck;

            public T Element => element;

            internal ReturnHandle(T value, IPoolCore<T> pool, bool forceSafeCheck = false)
            {
                element = value;
                poolCore = pool;
                this.ForceSafeCheck = forceSafeCheck;
            }

            /// <summary>
            /// Disposable pattern implementation.
            /// </summary>
            public void Dispose()
            {
                if (poolCore != null && element != null)
                {
                    poolCore.Return(ref element, ForceSafeCheck);
                }
                element = default;
                poolCore = null;
            }

            public static implicit operator T(ReturnHandle handle)
            {
                return handle.element;
            }
        }

        /// <summary>
        /// 使用using自动归还
        /// </summary>
        /// <returns></returns>
        /// <param name="element"></param>
        /// <param name="forceSafeCheck"></param>
        ReturnHandle Rent(out T element, bool forceSafeCheck = false);
    }

    public abstract class PoolCoreBase<T> : IPoolCore<T>
    {
        /// <summary>
        /// 在Pop后一刻调用,如果返回false,则构造新元素.
        /// </summary>
        internal protected Func<T, bool> PostRentPop;
        internal protected Action<T> OnRent;
        internal protected Action<T> OnReturn;
        /// <summary>
        /// 在Push前一刻调用,如果返回false,则不Push.
        /// </summary>
        internal protected Func<T, bool> PreReturnPush;
        public int CountAll { get; protected set; }
        public bool SafeCheck { get; set; } = false;
        /// <summary>
        /// 池中最大保留元素个数,默认为10
        /// </summary>
        public static int MaxSize { get; set; } = 10;

        public IPoolCore<T>.ReturnHandle Rent(out T element, bool forceSafeCheck = false)
        {
            var e = Rent();
            IPoolCore<T>.ReturnHandle handle = new IPoolCore<T>.ReturnHandle(e, this, forceSafeCheck);
            element = e;
            return handle;
        }

        public abstract T Rent();
        public abstract void Return(ref T element, bool forceSafeCheck = false);

        public static bool IsSafeCollection<C>(C collection)
            where C : ICollection
        {
            if (collection == null || collection.Count != 0)
            {
                return false;
            }
            return true;
        }

        public static bool IsSafeCollection<C, E>(C collection)
            where C : ICollection<E>
        {
            if (collection == null || collection.Count != 0)
            {
                return false;
            }
            return true;
        }

        public static void ClearCollection<C>(C collection)
            where C : IList
        {
            collection?.Clear();
        }

        public static void ClearCollection<C, E>(C collection)
            where C : ICollection<E>
        {
            collection?.Clear();
        }
    }

    public class StackPoolCore<T> : PoolCoreBase<T>
        where T : new()
    {
        Stack<T> stack = new Stack<T>();
        public int CountActive { get { return CountAll - CountInactive; } }
        public int CountInactive { get { return stack.Count; } }


        public StackPoolCore() { }
        public StackPoolCore(Func<T, bool> postRentPop,
                        Action<T> onRent,
                        Action<T> onReturn,
                        Func<T, bool> preReturnPush)
        {
            PostRentPop = postRentPop;
            OnRent = onRent;
            OnReturn = onReturn;
            PreReturnPush = preReturnPush;
        }

        public override T Rent()
        {
            T element;
            if (stack.Count == 0)
            {
                element = Construct();
            }
            else
            {
                element = stack.Pop();
                if (PostRentPop?.Invoke(element) ?? true)
                {

                }
                else
                {
                    //从栈中弹出的如果不能通过检测, 构造一个新的.
                    element = Construct();
                }
            }

            OnRent?.Invoke(element);
            return element;
        }

        public virtual T Construct()
        {
            var element = new T();
            CountAll++;
            return element;
        }

        public override void Return(ref T element, bool forceSafeCheck = false)
        {
            if (forceSafeCheck || SafeCheck)
            {
                if (stack.Count > 0)
                {
                    if (stack.Count >= MaxSize)
                    {
                        CountAll--;
                        //超出保留数,舍去
                        element = default;
                        return;
                    }

                    if (stack.Contains(element))
                    {
                        //引用重复, 可能是用户误用,归还2次,忽略
                        element = default;
                        return;
                    }
                }
            }

            OnReturn?.Invoke(element);
            var push = PreReturnPush?.Invoke(element) ?? true;
            if (push)
            {
                stack.Push(element);
            }
            else
            {
                CountAll--;
            }
            element = default;
        }

        public void Clear()
        {
            stack.Clear();
            CountAll = 0;
        }
    }

    public class ConcurrentStackPoolCore<T> : PoolCoreBase<T>, IPoolCore<T>
        where T : new()
    {
        ConcurrentStack<T> stack = new ConcurrentStack<T>();

        public int CountActive { get { return CountAll - CountInactive; } }
        public int CountInactive { get { return stack.Count; } }


        public ConcurrentStackPoolCore() { }
        public ConcurrentStackPoolCore(Func<T, bool> postRentPop,
                        Action<T> onRent,
                        Action<T> onReturn,
                        Func<T, bool> preReturnPush)
        {
            PostRentPop = postRentPop;
            OnRent = onRent;
            OnReturn = onReturn;
            PreReturnPush = preReturnPush;
        }

        public override T Rent()
        {
            T element;
            if (stack.TryPop(out element))
            {
                if (PostRentPop?.Invoke(element) ?? true)
                {

                }
                else
                {
                    //从栈中弹出的如果不能通过检测, 构造一个新的.
                    element = Construct();
                }
            }
            else
            {
                element = Construct();
            }

            OnRent?.Invoke(element);
            return element;
        }

        public virtual T Construct()
        {
            var element = new T();
            CountAll++;
            return element;
        }

        public override void Return(ref T element, bool forceSafeCheck = false)
        {
            if (forceSafeCheck || SafeCheck)
            {
                if (stack.Count > 0)
                {
                    if (stack.Count >= MaxSize)
                    {
                        CountAll--;
                        //超出保留数,舍去
                        element = default;
                        return;
                    }

                    if (stack.Contains(element))
                    {
                        //引用重复, 可能是用户误用,归还2次,忽略
                        element = default;
                        return;
                    }
                }
            }

            OnReturn?.Invoke(element);
            var push = PreReturnPush?.Invoke(element) ?? true;
            if (push)
            {
                stack.Push(element);
            }
            else
            {
                CountAll--;
            }
            element = default;
        }

        public void Clear()
        {
            stack.Clear();
            CountAll = 0;
        }
    }

    [Obsolete("Template", true)]
    internal static class Pool<T>
        where T : new()
    {
        /// <summary>
        /// 不要继承静态自动,父类和子类共有可能存在隐藏bug
        /// </summary>
        public static readonly StackPoolCore<T> poolCore = new StackPoolCore<T>();

        /// <summary>
        /// <inheritdoc cref="IPoolCore{T}.Rent()"/>
        /// </summary>
        /// <returns></returns>
        public static T Rent()
        {
            return poolCore.Rent();
        }

        /// <summary>
        /// <inheritdoc cref="IPoolCore{T}.Rent(out T, bool)"/>
        /// </summary>
        /// <returns></returns>
        public static IPoolCore<T>.ReturnHandle Rent(out T element, bool forceSafeCheck = false)
        {
            return poolCore.Rent(out element, forceSafeCheck);
        }

        /// <summary>
        /// <inheritdoc cref="IPoolCore{T}.Return(ref T, bool)"/>
        /// </summary>
        /// <returns></returns>
        public static void Return(ref T element, bool safeCheck = false)
        {
            poolCore.Return(ref element, safeCheck);
        }

        public static void Clear()
        {
            poolCore.Clear();
        }
    }

    /// <summary>
    /// 线程不安全List池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>内部使用<see cref="Stack{T}"/>实现</remarks>
    public class ListPool<T> : StackPoolCore<List<T>>
    {
        public static ListPool<T> Shared { get; } =
            new() { PostRentPop = IsSafeCollection, OnReturn = ClearCollection };
    }

    /// <summary>
    /// 线程安全List池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>内部使用<see cref="ConcurrentStack{T}"/>实现</remarks>
    public class ConcurrentListPool<T> : ConcurrentStackPoolCore<List<T>>
    {
        public static ConcurrentListPool<T> Shared { get; } =
            new() { PostRentPop = IsSafeCollection, OnReturn = ClearCollection };
    }

    /// <summary>
    /// 线程不安全HashSet池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>内部使用<see cref="Stack{T}"/>实现</remarks>
    public class HashSetPool<T> : StackPoolCore<HashSet<T>>
    {
        public static HashSetPool<T> Shared { get; } =
            new() { PostRentPop = IsSafeCollection<HashSet<T>, T>, OnReturn = ClearCollection<HashSet<T>, T> };
    }

    /// <summary>
    /// 线程安全HashSet池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>内部使用<see cref="ConcurrentStack{T}"/>实现</remarks>
    public class ConcurrentHashSetPool<T> : ConcurrentStackPoolCore<HashSet<T>>
    {
        public static ConcurrentHashSetPool<T> Shared { get; } =
            new() { PostRentPop = IsSafeCollection<HashSet<T>, T>, OnReturn = ClearCollection<HashSet<T>, T> };
    }
}


