using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 计时（服务器用）
    /// </summary>
    public class UtilTime
    {
        private DateTime prev;

        public UtilTime()
        {
            prev = DateTime.Now;
        }

        /// <summary>
        /// 上一个时间间隔长度（毫秒）
        /// </summary>
        public double DeltaTime { get;private set; }
        /// <summary>
        /// 获取以整毫秒数和毫秒的小数部分表示的当前 System.TimeSpan 结构的值。
        /// </summary>
        public double TotalMilliseconds { get; private set; }

        /// <summary>
        /// 更新时间间隔
        /// </summary>
        /// <returns></returns>
        public double Update()
        {
            var now = DateTime.Now;
            TotalMilliseconds = now.TimeOfDay.TotalMilliseconds;
            var delta = now - prev;
            DeltaTime = delta.TotalMilliseconds;
            prev = now;
            return DeltaTime;
        }
    }
}
