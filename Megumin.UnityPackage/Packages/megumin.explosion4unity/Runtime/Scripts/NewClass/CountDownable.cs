using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface ICountDownable
{
    /// <summary>
    /// CD时长
    /// </summary>
    TimeSpan CDTimeSpan { get; }
    /// <summary>
    /// 距离结束时刻时长
    /// </summary>
    TimeSpan Delta2Finish { get; }
    /// <summary>
    /// 结束时刻
    /// </summary>
    DateTimeOffset FinishTime { get; }

    /// <summary>
    /// <para/> -2 ：没有Start
    /// <para/> -1 ：计时被取消
    /// <para/> 0 ：计时完成
    /// </summary>
    Task<int> FinishTask { get; }
    /// <summary>
    /// 显示为指定时间
    /// </summary>
    /// <param name="delta2End"></param>
    void Show(TimeSpan delta2End);
    void SetCancellationToken(CancellationToken token);
    Task<int> ReStartCD();
    Task<int> StartCD(DateTimeOffset finishTime, TimeSpan? cdTime = null, CancellationToken token = default);
    Task<int> StartCD(TimeSpan cdTime, DateTimeOffset? startTime = null, CancellationToken token = default);
}

public abstract class BaseCountDown : MonoBehaviour, ICountDownable
{
    public DateTimeOffset FinishTime { get; private set; }
    public TimeSpan Delta2Finish { get; private set; }
    public TimeSpan CDTimeSpan { get; private set; }
    CancellationToken cancellationToken = default;
    private TaskCompletionSource<int> source;
    public Task<int> FinishTask => source?.Task ?? Task.FromResult(-2);

    public Task<int> StartCD(DateTimeOffset finishTime, TimeSpan? cdTime = null, CancellationToken token = default)
    {
        source?.TrySetResult(-1);
        source = new TaskCompletionSource<int>();
        cancellationToken = token;

        FinishTime = finishTime;
        if (cdTime.HasValue)
        {
            CDTimeSpan = (TimeSpan)cdTime;
        }
        else
        {
            CDTimeSpan = finishTime - DateTimeOffset.UtcNow;
        }

        Tick(DateTimeOffset.UtcNow);
        return source.Task;
    }

    public Task<int> StartCD(TimeSpan cdTime, DateTimeOffset? startTime = null, CancellationToken token = default)
    {
        source?.TrySetResult(-1);
        source = new TaskCompletionSource<int>();
        cancellationToken = token;

        CDTimeSpan = cdTime;
        if (startTime.HasValue)
        {
            FinishTime = (DateTimeOffset)startTime + cdTime;
        }
        else
        {
            FinishTime = DateTimeOffset.UtcNow + cdTime;
        }

        Tick(DateTimeOffset.UtcNow);
        return source.Task;
    }

    public Task<int> ReStartCD()
    {
        return StartCD(CDTimeSpan);
    }

    public void SetCancellationToken(CancellationToken token)
    {
        cancellationToken = token;
    }

    protected void Tick(DateTimeOffset now)
    {
        var cd = FinishTime - now;
        Delta2Finish = cd;
        Show(cd);
        if (cd.TotalSeconds <= 0)
        {
            source?.TrySetResult(0);
        }
        else
        {
            if (cancellationToken.IsCancellationRequested)
            {
                source?.TrySetResult(-1);
            }
        }
    }

    public abstract void Show(TimeSpan delta2End);
}

