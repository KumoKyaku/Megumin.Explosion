using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 联机模式/网络可连接状态
    /// </summary>
    /// <![CDATA[
    /// if(CurrentLineMode > LineMode.LAN)
    /// {
    ///     //当前有公网连接
    /// }
    /// ]]>
    [Flags]
    public enum LineMode
    {
        /// <summary>
        /// 离线/无网络连接
        /// </summary>
        Offline = 1 << 0,
        /// <summary>
        /// 单机模式/与本机可连接
        /// </summary>
        Single = 1 << 1,
        /// <summary>
        /// 局域网/局域网内可连接
        /// </summary>
        LAN = 1 << 2,
        /// <summary>
        /// 在线/可与公网连接
        /// </summary>
        Online = 1 << 3,
    }
}
