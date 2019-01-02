using System;

namespace Megumin
{
    /// <summary>
    /// 
    /// </summary>
    public class CoolDownTime
    {
        /// <summary>
        /// 是否冷却完毕
        /// </summary>
        public bool CoolDown
        {
            get
            {
                if (DateTime.Now - Last > MinDelta)
                {
                    Last = DateTime.Now;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 是否冷却完毕
        /// </summary>
        /// <param name="time"></param>
        public static implicit operator bool(CoolDownTime time)
        {
            return time.CoolDown;
        }


        /// <summary>
        /// 上次返回冷却完毕的时间
        /// </summary>
        public DateTime Last { get; set; } = DateTime.MinValue;
        /// <summary>
        /// 最小间隔
        /// </summary>
        public TimeSpan MinDelta { get; set; } = TimeSpan.FromMilliseconds(15);
    }
}
