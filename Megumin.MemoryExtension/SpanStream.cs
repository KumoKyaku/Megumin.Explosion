using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Buffers
{
    /// <summary>
    /// ReadOnlyMemory的流包装器， 立刻使用，立刻丢弃，不应该保存。
    /// 这个类用于在 不支持Span的第三方API调用过程中转换参数，随着第三方类库的支持完成这类会删除。
    /// <para></para>
    /// https://gist.github.com/GrabYourPitchforks/4c3e1935fd4d9fa2831dbfcab35dffc6
    /// 参考第五条规则
    /// </summary>
    public class SpanStream : Stream
    {
        /// 事实上，无法支持Span的类库永远都会存在，所以这个类可能永远不会废除……

        private ReadOnlyMemory<byte> memory;

        public SpanStream(ReadOnlyMemory<byte> memory)
        {
            this.memory = memory;
        }

        public override void Flush()
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                //   T:System.ArgumentNullException:
                //     buffer is null.
                throw new ArgumentNullException();
            }

            var curCount = Length - Position;
            if (curCount <= 0)
            {
                return 0;
            }

            //   T:System.ObjectDisposedException:
            //     Methods were called after the stream was closed.

            if (buffer.Length - offset < count)
            {
                //   T:System.ArgumentException:
                //     The sum of offset and count is larger than the buffer length.
                throw new ArgumentException();
            }

            int copyCount = curCount >= count ? count : (int)curCount;

            memory.Span.Slice((int)Position, copyCount).CopyTo(buffer.AsSpan(offset, copyCount));
            Position += copyCount;
            return copyCount;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var tar = 0L;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    tar = 0 + offset;
                    break;
                case SeekOrigin.Current:
                    tar = Position + offset;
                    break;
                case SeekOrigin.End:
                    tar = Length + offset;
                    break;
                default:
                    break;
            }

            if (tar >= 0 && tar < Length)
            {
                Position = offset;
            }

            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => memory.Length;
        public override long Position { get; set; } = 0;

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    memory = null;
                }
            }
            finally
            {
                // Call base.Close() to cleanup async IO resources
                base.Dispose(disposing);
            }
        }
    }
}
