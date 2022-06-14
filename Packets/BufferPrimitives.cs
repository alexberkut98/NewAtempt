using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.Packets
{
    public static class BufferPrimitives
    {
        #region Methods

        public static byte[] GetBytes(byte[] buffer, int offset, int count)
        {
            return GetBytes(buffer, ref offset, count);
        }

        public static byte[] GetBytes(byte[] buffer, ref int offset, int count)
        {
            var temp = new byte[count];
            for (var i = 0; i < count; ++i) temp[i] = buffer[offset++];
            return temp;
        }

        public static ushort GetUint16(byte[] buffer, ref int offset)
        {
            return (ushort)GetVarious(buffer, ref offset, sizeof(ushort));
        }

        public static ushort GetUint16(byte[] buffer, int offset)
        {
            return (ushort)GetVarious(buffer, ref offset, sizeof(ushort));
        }

        public static uint GetUint32(byte[] buffer, ref int offset)
        {
            return (uint)GetVarious(buffer, ref offset, sizeof(uint));
        }

        public static uint GetUint32(byte[] buffer, int offset)
        {
            return (uint)GetVarious(buffer, ref offset, sizeof(uint));
        }

        public static ulong GetUint64(byte[] buffer, ref int offset)
        {
            return GetVarious(buffer, ref offset, sizeof(ulong));
        }

        public static ulong GetUint64(byte[] buffer, int offset)
        {
            return GetVarious(buffer, ref offset, sizeof(ulong));
        }

        public static byte GetUint8(byte[] buffer, ref int offset)
        {
            return (byte)GetVarious(buffer, ref offset, sizeof(byte));
        }

        public static byte GetUint8(byte[] buffer, int offset)
        {
            return (byte)GetVarious(buffer, ref offset, sizeof(byte));
        }

        public static ulong GetVarious(byte[] buffer, int offset, int count)
        {
            return GetVarious(buffer, ref offset, count);
        }

        public static ulong GetVarious(byte[] buffer, ref int offset, int count)
        {
            ulong result = 0;
            for (var i = 0; i < count; ++i) result = (result << 8) | buffer[offset++];
            return result;
        }

        public static void SetBytes(byte[] buffer, ref int offset, byte[] data)
        {
            SetBytes(buffer, ref offset, data, 0, data.Length);
        }

        public static void SetBytes(byte[] buffer, int offset, byte[] data)
        {
            SetBytes(buffer, ref offset, data, 0, data.Length);
        }

        public static void SetBytes(byte[] buffer, int offset, byte[] data, int index, int count)
        {
            SetBytes(buffer, ref offset, data, index, count);
        }

        public static void SetBytes(byte[] buffer, ref int offset, byte[] data, int index, int count)
        {
            for (var i = 0; i < count; ++i) buffer[offset++] = data[index + i];
        }

        public static void SetUint16(byte[] buffer, ref int offset, ushort data)
        {
            SetVarious(buffer, ref offset, data, sizeof(ushort));
        }

        public static void SetUint16(byte[] buffer, int offset, ushort data)
        {
            SetVarious(buffer, ref offset, data, sizeof(ushort));
        }

        public static void SetUint32(byte[] buffer, ref int offset, uint data)
        {
            SetVarious(buffer, ref offset, data, sizeof(uint));
        }

        public static void SetUint32(byte[] buffer, int offset, uint data)
        {
            SetVarious(buffer, ref offset, data, sizeof(uint));
        }

        public static void SetUint64(byte[] buffer, ref int offset, ulong data)
        {
            SetVarious(buffer, ref offset, data, sizeof(ulong));
        }

        public static void SetUint64(byte[] buffer, int offset, ulong data)
        {
            SetVarious(buffer, ref offset, data, sizeof(ulong));
        }

        public static void SetUint8(byte[] buffer, ref int offset, byte data)
        {
            SetVarious(buffer, ref offset, data, sizeof(byte));
        }

        public static void SetUint8(byte[] buffer, int offset, byte data)
        {
            SetVarious(buffer, ref offset, data, sizeof(byte));
        }

        public static void SetVarious(byte[] buffer, int offset, long data, int count)
        {
            SetVarious(buffer, ref offset, data, count);
        }

        public static void SetVarious(byte[] buffer, int offset, ulong data, int count)
        {
            SetVarious(buffer, ref offset, data, count);
        }

        public static void SetVarious(byte[] buffer, ref int offset, ulong data, int count)
        {
            for (var i = 0; i < count; ++i) buffer[offset++] = (byte)(data >> (8 * (count - i - 1)));
        }

        public static void SetVarious(byte[] buffer, ref int offset, long data, int count)
        {
            for (var i = 0; i < count; ++i) buffer[offset++] = (byte)(data >> (8 * (count - i - 1)));
        }

        #endregion
    }
}
