using System;
using System.Collections.Generic;
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
    }
}
