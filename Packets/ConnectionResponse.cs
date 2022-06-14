using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Новая_попытка._Enums_;

namespace Новая_попытка.Packets
{
    public class ConnectionResponse
    {
        #region Properties

        public PacketId Id => PacketId.ConnectionResponse;

        public ResultCodes Result { get; }

        public string Reason { get; }

        #endregion Properties

        #region Constructors

        public ConnectionResponse(byte[] packet)
        {
            int offset = 1;
            Result = (ResultCodes)BufferPrimitives.GetUint8(packet, ref offset);
            Reason = Encoding.UTF8.GetString(packet, offset, packet.Length - offset);
        }

        public ConnectionResponse(ResultCodes result, string reason)
        {
            Result = result;
            Reason = reason;
        }

        #endregion Constructors

        #region Methods

        public byte[] GetBytes()
        {
            if (string.IsNullOrEmpty(Reason))
                return new[] { (byte)Id, (byte)Result };

            byte[] reason = Encoding.UTF8.GetBytes(Reason);
            byte[] packet = new byte[reason.Length + 2];

            int offset = 0;
            BufferPrimitives.SetUint8(packet, ref offset, (byte)Id);
            BufferPrimitives.SetUint8(packet, ref offset, (byte)Result);
            BufferPrimitives.SetBytes(packet, ref offset, reason, 0, reason.Length);

            return packet;
        }

        #endregion Methods
    }
}

