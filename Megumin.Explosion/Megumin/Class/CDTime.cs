using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 时间控制
    /// </summary>
    public interface ITimeControlable
    {
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        int Pause(object option = null);

        /// <summary>
        /// 继续
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        int Resume(object option = null);
    }

    /// <summary>
    /// 冷却计时器
    /// <para>支持充能点数</para>
    /// </summary>
    public interface ICDTimer : ITimeControlable
    {
        /// <summary>
        /// 当前是不是可用，至少含有1点。
        /// </summary>
        bool CanUse { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        bool TryUse(int count = 1);

        /// <summary>
        /// 最大可用点数
        /// </summary>
        int MaxCanUseCount { get; }
        /// <summary>
        /// 当前剩余可用点数
        /// </summary>
        int ResidualCanUseCount { get; }
        /// <summary>
        /// 强制增加当前可用点数
        /// </summary>
        /// <param name="count"></param>
        void ForceAddResidualCanUseCount(int count = 1);
    }

    /// <summary>
    /// 冷却计时器
    /// </summary>
    /// <typeparam name="Unit"></typeparam>
    public interface ICDTimer<Unit> : ICDTimer
    {

        /// <summary>
        /// 无论当前可用点数是多少，距离下一次冷却完成的时长，返回值总是[0-PerSpan]
        /// </summary>
        /// <remarks>想象一下最大可用5，当前可用2，然后倒计时扇形显示的是到下个可用点的时间，而不是到最大可用的时间</remarks>
        Unit NextCanUse { get; }

        /// <summary>
        /// 每完成一个冷却点数所需的时间
        /// </summary>
        Unit PerSpan { get; set; }
    }

    public interface ICDTicker<Unit>
    {
        Unit Now { get; }
        /// <summary>
        /// 检查计时器当前时刻与记录时刻比较，是不是大于perSpan, 
        /// </summary>
        /// <param name="stamp">记录时刻</param>
        /// <param name="perSpan">间隔时长</param>
        /// <returns>
        /// 完成间隔次数,最后检查点，距离下次完成时长
        /// </returns>
        (int CompleteCount, Unit CheckStamp, Unit Next) Check(Unit stamp, Unit perSpan);
    }

    public class CDTimer<Unit> : ICDTimer<Unit>
    {
        static readonly List<CDTimer<Unit>> pool = new List<CDTimer<Unit>>();
        static readonly List<WeakReference<CDTimer<Unit>>> pool2
            = new List<WeakReference<CDTimer<Unit>>>();

        public static CDTimer<Unit> Create(int maxCanUse = 1, int defaultCanUse = 1, bool weak = true)
        {
            CDTimer<Unit> timer = new CDTimer<Unit>();
            timer.MaxCanUseCount = maxCanUse;
            timer.ResidualCanUseCount = defaultCanUse;

            if (weak)
            {
                lock (pool2)
                {
                    var weakref = new WeakReference<CDTimer<Unit>>(timer);
                    pool2.Add(weakref);
                }
            }
            else
            {
                lock (pool)
                {
                    pool.Add(timer);
                }
            }

            return timer;
        }

        public static void TickAll(ICDTicker<Unit> ticker)
        {
            lock (pool)
            {
                pool.RemoveAll(ele => ele.Valid == false);

                foreach (var item in pool)
                {
                    item.Tick(ticker);
                }
            }

            lock (pool2)
            {
                pool2.RemoveAll(ele =>
                {
                    if (ele.TryGetTarget(out var timer))
                    {
                        return !timer.Valid;
                    }
                    else
                    {
                        return true;
                    }
                });

                foreach (var item in pool2)
                {
                    if (item.TryGetTarget(out var timer))
                    {
                        timer.Tick(ticker);
                    }
                }
            }
        }

        bool Valid = true;
        Unit Stamp;
        bool IsCDing = false;
        public void Tick(ICDTicker<Unit> ticker)
        {
            if (ResidualCanUseCount >= MaxCanUseCount)
            {
                IsCDing = false;
                NextCanUse = default;
                return;
            }
            else
            {
                if (IsCDing)
                {
                    var (CompleteCount, CheckStamp, Next) = ticker.Check(Stamp, PerSpan);

                    if (CompleteCount > 0)
                    {
                        Stamp = CheckStamp;
                    }

                    var newCount = ResidualCanUseCount + CompleteCount;
                    NextCanUse = newCount >= MaxCanUseCount ? default : Next;
                    ResidualCanUseCount = Math.Min(newCount, MaxCanUseCount);
                }
                else
                {
                    //没有在CD中
                    Stamp = ticker.Now;
                    NextCanUse = PerSpan;
                    IsCDing = true;
                    return;
                }
            }
        }

        public void Dispose()
        {
            Valid = false;
        }



        public Unit NextCanUse { get; protected set; }
        public Unit PerSpan { get; set; }
        public bool CanUse => ResidualCanUseCount > 0;

        public bool TryUse(int count = 1)
        {
            if (ResidualCanUseCount >= count)
            {
                ResidualCanUseCount -= count;
                return true;
            }

            return false;
        }

        public int MaxCanUseCount { get; protected set; } = 1;
        public int ResidualCanUseCount { get; protected set; } = 1;

        public void ForceAddResidualCanUseCount(int count = 1)
        {
            ResidualCanUseCount += count;
        }

        public virtual int Pause(object option = null)
        {
            throw new NotImplementedException();
        }

        public virtual int Resume(object option = null)
        {
            throw new NotImplementedException();
        }


    }
}



