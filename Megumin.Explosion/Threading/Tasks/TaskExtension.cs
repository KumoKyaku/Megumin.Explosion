using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Threading.Tasks
{
    public static class TaskExtension_49A548505C7242BEBD1AD43D876BC1B0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async static Task<(T result, bool complete)> WaitAsync<T>(this Task<T> task, int millisecondsTimeout)
        {
            var complete = await Task.Run(() => task.Wait(millisecondsTimeout));
            return (complete ? task.Result : default, complete);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async static Task<bool> WaitAsync(this Task task, int millisecondsTimeout)
        {
            return await Task.Run(() => task.Wait(millisecondsTimeout));
        }

        //https://source.dot.net/#Microsoft.AspNetCore.Testing/TaskExtensions.cs,25cb46407f55bdbb

        [SuppressMessage("ApiDesign", "RS0026:Do not add multiple public overloads with optional parameters", Justification = "Required to maintain compatibility")]
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout,
        [CallerFilePath] string filePath = null,
        [CallerLineNumber] int lineNumber = default)
        {
            // Don't create a timer if the task is already completed
            // or the debugger is attached
            if (task.IsCompleted || Debugger.IsAttached)
            {
                return await task.ConfigureAwait(false);
            }
#if NET6_0_OR_GREATER
            try
            {
                return await task.WaitAsync(timeout).ConfigureAwait(false);
            }
            catch (TimeoutException ex) when (ex.Source == typeof(TaskExtensions).Namespace)
            {
                throw new TimeoutException(CreateMessage(timeout, filePath, lineNumber));
            }
#else
            var cts = new CancellationTokenSource();
            if (task == await Task.WhenAny(task, Task.Delay(timeout, cts.Token)).ConfigureAwait(false))
            {
                cts.Cancel();
                return await task.ConfigureAwait(false);
            }
            else
            {
                throw new TimeoutException(CreateMessage(timeout, filePath, lineNumber));
            }
#endif
        }

        [SuppressMessage("ApiDesign", "RS0026:Do not add multiple public overloads with optional parameters", Justification = "Required to maintain compatibility")]
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = default)
        {
            // Don't create a timer if the task is already completed
            // or the debugger is attached
            if (task.IsCompleted || Debugger.IsAttached)
            {
                await task.ConfigureAwait(false);
                return;
            }
#if NET6_0_OR_GREATER
            try
            {
                await task.WaitAsync(timeout).ConfigureAwait(false);
            }
            catch (TimeoutException ex) when (ex.Source == typeof(TaskExtensions).Namespace)
            {
                throw new TimeoutException(CreateMessage(timeout, filePath, lineNumber));
            }
#else
            var cts = new CancellationTokenSource();
            if (task == await Task.WhenAny(task, Task.Delay(timeout, cts.Token)).ConfigureAwait(false))
            {
                cts.Cancel();
                await task.ConfigureAwait(false);
            }
            else
            {
                throw new TimeoutException(CreateMessage(timeout, filePath, lineNumber));
            }
#endif
        }

        private static string CreateMessage(TimeSpan timeout, string filePath, int lineNumber)
            => string.IsNullOrEmpty(filePath)
            ? $"The operation timed out after reaching the limit of {timeout.TotalMilliseconds}ms."
            : $"The operation at {filePath}:{lineNumber} timed out after reaching the limit of {timeout.TotalMilliseconds}ms.";
    }
}
