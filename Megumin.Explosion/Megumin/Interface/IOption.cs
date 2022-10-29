using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Megumin
{
    /// <summary>
    /// 超时选项。
    /// </summary>
    public interface ITimeoutOption
    {
        /// <summary>
        /// 指定毫秒后超时，-1表示永不超时。
        /// <seealso cref="CancellationTokenSource.CancelAfter(int)"/>
        /// </summary>
        int MillisecondsTimeout { get; }
    }

    public interface ICancellationTokenOption
    {
        CancellationToken CancellationToken { get; }
    }

    public interface ICancellationTokenSourceOption
    {
        CancellationTokenSource CancellationTokenSource { get; }
    }
}




