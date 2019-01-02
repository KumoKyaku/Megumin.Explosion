using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 插件状态
    /// </summary>
    public enum PluginState
    {
        /// <summary>
        /// 插件为空
        /// </summary>
        Null,
        /// <summary>
        /// 正在初始化
        /// </summary>
        Initing,
        /// <summary>
        /// 初始化失败已停止工作
        /// </summary>
        InitErrorAndStop,
        /// <summary>
        /// 重新初始化
        /// </summary>
        ReIniting,
        /// <summary>
        /// 初始化完成
        /// </summary>
        InitFinish,
        /// <summary>
        /// 开启
        /// </summary>
        Open,
        /// <summary>
        /// 关闭
        /// </summary>
        Close,
        /// <summary>
        /// 已释放
        /// </summary>
        Dispose
    }
}
