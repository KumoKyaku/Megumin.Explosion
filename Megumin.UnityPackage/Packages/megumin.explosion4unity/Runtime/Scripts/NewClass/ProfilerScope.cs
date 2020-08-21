using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// 性能分析区域，用来解决采样区间return导致采样Begin End不匹配问题
/// todo 开Deep 是采样错乱
/// </summary>
public struct ProfilerScope : IDisposable
{
    public ProfilerScope(string name)
    {
        Profiler.BeginSample(name);
    }

    public void Dispose()
    {
        Profiler.EndSample();
    }

    /// <summary>
    /// 会引入额外开销并导致Deep采样器错乱
    /// </summary>
    public static ProfilerScope CurrentMethod
    {
        get
        {
            //Profiler.enabled = false;  //不起作用
            StackTrace stack = new StackTrace(1);
            string name = stack.GetFrame(0).GetMethod().Name;
            //Profiler.enabled = true;
            return new ProfilerScope(name);
        }
    }

    /// <summary>
    /// 会引入额外开销并导致Deep采样器错乱
    /// </summary>
    public static ProfilerScope CurrentLine
    {
        get
        {
            //Profiler.SetAreaEnabled(ProfilerArea.CPU, false); //不起作用
            StackTrace stack = new StackTrace(1);
            var frame = stack.GetFrame(0);
            string name = $"{frame.GetMethod().Name}--{frame.GetFileLineNumber()}"; //行号不起作用
            //Profiler.SetAreaEnabled(ProfilerArea.CPU, true);
            return new ProfilerScope(name);
        }
    }
}
