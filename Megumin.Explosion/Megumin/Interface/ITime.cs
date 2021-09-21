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
}





