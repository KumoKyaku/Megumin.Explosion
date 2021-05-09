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
        Unit PerSpan { get; }
    }

    public interface ICDTicker<Unit>
    {
        Unit Now { get; }

        /// <summary>
        /// 检查当前时刻到记录时刻是不是大于perSpan, 
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="perSpan"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        bool CheckNow(Unit stamp, Unit perSpan, out Unit next);
    }

    public class CDTimer<Unit> : ICDTimer<Unit>
    {
        static readonly List<CDTimer<Unit>> pool = new List<CDTimer<Unit>>();
        public static CDTimer<Unit> Create(int maxCanUse = 1, int defaultCanUse = 1)
        {
            lock (pool)
            {
                CDTimer<Unit> timer = new CDTimer<Unit>();
                timer.MaxCanUseCount = maxCanUse;
                timer.ResidualCanUseCount = defaultCanUse;

                pool.Add(timer);
                return timer;
            }
        }

        public static void TickAll(ICDTicker<Unit> ticker)
        {
            lock (pool)
            {
                foreach (var item in pool)
                {
                    item.Tick(ticker);
                }

                pool.RemoveAll(ele => ele.Valid == false);
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
                    bool comp = ticker.CheckNow(Stamp, PerSpan, out var next);
                    if (comp)
                    {
                        ResidualCanUseCount++;
                        Stamp = ticker.Now;
                        NextCanUse = default;
                    }
                    else
                    {
                        NextCanUse = next;
                    }
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
        public Unit PerSpan { get; }
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



