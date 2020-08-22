using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin
{
    public class Cache
    {
        #region MyRegion
        static List<ICalculable> cacheValues = null;
        static readonly object innerlock = new object();

        public static void CollectField<T>()
            where T:Cache
        {
            var collection = (from field in typeof(T).GetFields()
                     where field.FieldType == typeof(ICalculable)
                     || field.FieldType.IsSubclassOf(typeof(ICalculable))
                     select field.GetValue(null) as ICalculable);

            lock (innerlock)
            {
                if (cacheValues == null)
                {
                    cacheValues = new List<ICalculable>();
                }
            }

            foreach (var item in collection)
            {
                if (!cacheValues.Contains(item))
                {
                    cacheValues.Add(item);
                }
            }
        }

        /// <summary>
        /// 如果你继承了Cache类，第一次调用此方法前应该调用<see cref="CollectField{T}"/>,否则反射收集字段时
        /// 无法获得子类型的字段。
        /// </summary>
        public static void CalculateImmediate()
        {
            if (cacheValues == null)
            {
                CollectField<Cache>();
            }

            foreach (var item in cacheValues)
            {
                if (item != null)
                {
                    item.Calculate();
                }
            }
        }

        #endregion

        /// <summary>
        /// 缓存的当前时间，通常每帧计算一次，用于不精确计算
        /// </summary>
        public static readonly Cache<DateTime> Now = new Cache<DateTime>(() => DateTime.Now);
    }


    public sealed class Cache<T> : IDisposable, ICalculable, ICalculable<T>
    {
        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Calculate(false);
                return value;
            }
        }
        private Func<T> getValue;
        /// <summary>
        /// 上一次求值时的帧数
        /// </summary>
        private long lastCalculateFrame;
        private T value;

        public Cache(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException();
            }
            this.getValue = func;
        }

        public void Calculate()
        {
            Calculate(true);
        }

        /// <summary>
        /// 重新求值的间隔帧数
        /// </summary>
        public int DeltaFrameCount { get; set; } = 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Calculate(bool force = true)
        {
            if (!force)
            {
                if (Time.frameCount - lastCalculateFrame >= DeltaFrameCount)
                {
                    return value;
                }
            }

            this.lastCalculateFrame = Time.frameCount;
            if (getValue == null)
            {
                throw new ObjectDisposedException(nameof(Cache<T>));
            }
            else
            {
                value = getValue();
            }
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(Cache<T> flag)
        {
            return flag.Value;
        }

        public void Dispose()
        {
            getValue = null;
        }
    }
}
