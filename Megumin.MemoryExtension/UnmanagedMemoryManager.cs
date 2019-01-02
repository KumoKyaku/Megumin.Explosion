using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Buffers
{
    public class UnmanagedMemoryManager<T> : MemoryManager<T>
    {
        private readonly int perSize;
        private readonly IntPtr ptr;
        public int Length { get; protected set; } = 0;
        public UnmanagedMemoryManager(int length)
        {
            this.Length = length;
            perSize = Marshal.SizeOf<T>();
            if (Length > 0)
            {
                this.ptr = Marshal.AllocHGlobal(Length * perSize);
                ///*申请到的内存可能不是干净的，也许需要0填充。
                GetSpan().Clear();
            }
            else
            {
                this.ptr = default;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (ptr != default && Length > 0)
            {
                Marshal.FreeHGlobal(ptr);
                Length = -1;
            }
        }

        public override Span<T> GetSpan()
        {
            if (Length > 0)
            {
                unsafe
                {
                    return new Span<T>(ptr.ToPointer(), Length);
                }
            }
            else if (Length < 0)
            {
                throw new ObjectDisposedException(nameof(UnmanagedMemoryManager<T>));
            }
            else
            {
                return Span<T>.Empty;
            }
        }

        public unsafe override MemoryHandle Pin(int elementIndex = 0)
        {
            if (elementIndex > Length)
            {
                throw new IndexOutOfRangeException();
            }
            return new MemoryHandle(IntPtr.Add(ptr, elementIndex * perSize).ToPointer());
        }

        public override void Unpin()
        {

        }
    }
}
