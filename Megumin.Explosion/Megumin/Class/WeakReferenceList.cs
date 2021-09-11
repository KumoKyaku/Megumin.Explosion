using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 弱引用集合,用于全局遍历
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeakReferenceList<T> : List<WeakReference<T>>
        where T : class
    {
        public WeakReference<T> Add(T value)
        {
            var weak = new WeakReference<T>(value);
            Add(weak);
            return weak;
        }

        public void ForEach(Action<T> action)
        {
            foreach (var item in this)
            {
                if (item.TryGetTarget(out var value))
                {
                    action?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// https://source.dot.net/#System.Private.CoreLib/List.cs,879
        /// RemoveAll 通过双指针实现,应该不影响效率,
        /// 但是每次使用Action会产生闭包,不一定比feache 和 RemoveAll 效率高
        /// </summary>
        /// <param name="action"></param>
        public void ForEachAutoRemoveNull(Action<T> action)
        {
            this.RemoveAll(item =>
            {
                if (item.TryGetTarget(out var value))
                {
                    action?.Invoke(value);
                    return false;
                }
                else
                {
                    return true;
                }
            });
        }
    }
}




