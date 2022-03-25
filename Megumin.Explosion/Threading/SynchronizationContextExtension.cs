using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Threading
{
    public static class SynchronizationContextExtension_F196DAE75A234F72A3AC998F76A66F1A
    {
        public struct Awaitable
        {
            bool IsCompletedAwaitable;
            internal SynchronizationContext Context;

            public static readonly Awaitable CompletedTask = new Awaitable() { IsCompletedAwaitable = true };

            public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
            {
                private Awaitable awaitable;

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Awaiter(Awaitable awaitable)
                {
                    this.awaitable = awaitable;
                }

                public bool IsCompleted
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get
                    {
                        //永远返回false，总是强制挂起。
                        return awaitable.IsCompletedAwaitable;
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public void GetResult()
                {
                    //不必进行任何操作，此处已经是切换线程后。
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public void UnsafeOnCompleted(Action continuation)
                {
                    awaitable.Context.Post(state => { continuation?.Invoke(); }, null);
                }

                public void OnCompleted(Action continuation)
                {
                    throw new NotImplementedException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Awaiter GetAwaiter()
            {
                return new Awaiter(this);
            }
        }

        public static Awaitable Switch(this SynchronizationContext context)
        {
            Awaitable awaitable = new Awaitable();
            awaitable.Context = context;
            return awaitable;
        }
    }
}
