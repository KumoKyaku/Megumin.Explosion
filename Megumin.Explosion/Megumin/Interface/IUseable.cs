using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{

    /// <summary>
    /// 可使用的.  
    /// <para/>包括冷却型的和充能型的,物品堆叠型的
    /// <para>支持充能点数</para>
    /// </summary>
    /// <remarks>
    /// 设计目的: 对模型层一个抽象,View层显示图标状态使用这个接口访问模型层数据信息.
    /// 是否可用 比 CD相关功能范围更大
    /// 比如猎空黑影技能
    /// </remarks>
    public interface IUseable
    {
        /// <summary>
        /// 当前是不是可用，至少含有1点。
        /// </summary>
        bool CanUse { get; }

        /// <summary>
        /// 正在使用中
        /// </summary>
        /// <remarks>
        /// 比如黑影的隐身,猎空的闪回,使用是一个持续过程
        /// </remarks>
        bool IsUsing { get; }

        /// <summary>
        /// 使用掉一个可用点数
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

    /// <inheritdoc/>
    public interface IUseable<Unit> : IUseable
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
}







