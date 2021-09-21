using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    public class CDTimer<Unit> : IUseable<Unit>, ITimeControlable
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

        public bool IsUsing { get; set; }
    }
}



