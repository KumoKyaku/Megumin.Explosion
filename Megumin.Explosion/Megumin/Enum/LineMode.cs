using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 联机模式
    /// </summary>
    [Flags]
    public enum LineMode
    {
        /// <summary>
        /// 单机模式
        /// </summary>
        Single = 1 << 0,
        /// <summary>
        /// 在线
        /// </summary>
        Online = 1 << 1,
        /// <summary>
        /// 离线
        /// </summary>
        Offline = 1 << 2,
        /// <summary>
        /// 局域网
        /// </summary>
        LAN = 1 << 3,
    }
}
