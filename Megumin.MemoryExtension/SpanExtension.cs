using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Buffers
{
    public static class SpanByteEX_3451DB8C29134366946FF9D778779EEC
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteTo(this int num, Span<byte> span)
            => BinaryPrimitives.WriteInt32BigEndian(span, num);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteTo(this ushort num, Span<byte> span)
            => BinaryPrimitives.WriteUInt16BigEndian(span, num);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteTo(this short num, Span<byte> span)
            => BinaryPrimitives.WriteInt16BigEndian(span, num);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteTo(this long num, Span<byte> span)
            => BinaryPrimitives.WriteInt64BigEndian(span, num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt(this ReadOnlySpan<byte> span)
            => BinaryPrimitives.ReadInt32BigEndian(span);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUshort(this ReadOnlySpan<byte> span)
            => BinaryPrimitives.ReadUInt16BigEndian(span);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadShort(this ReadOnlySpan<byte> span)
            => BinaryPrimitives.ReadInt16BigEndian(span);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLong(this ReadOnlySpan<byte> span)
            => BinaryPrimitives.ReadInt64BigEndian(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt(this Span<byte> span)
            => BinaryPrimitives.ReadInt32BigEndian(span);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUshort(this Span<byte> span)
            => BinaryPrimitives.ReadUInt16BigEndian(span);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadShort(this Span<byte> span)
            => BinaryPrimitives.ReadInt16BigEndian(span);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLong(this Span<byte> span)
            => BinaryPrimitives.ReadInt64BigEndian(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt(this Memory<byte> span)
            => BinaryPrimitives.ReadInt32BigEndian(span.Span);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUshort(this Memory<byte> span)
            => BinaryPrimitives.ReadUInt16BigEndian(span.Span);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadShort(this Memory<byte> span)
            => BinaryPrimitives.ReadInt16BigEndian(span.Span);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLong(this Memory<byte> span)
            => BinaryPrimitives.ReadInt64BigEndian(span.Span);
    }
}
