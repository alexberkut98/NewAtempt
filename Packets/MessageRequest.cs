using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.Packets
{
    public class MessageRequest
    {

        #region Properties
        public PacketId Id => PacketId.MessageRequest;
        public string Message { get; }

        #endregion Properties

        #region Constructors

        public MessageRequest(byte[] packet)
        {
            int offset = 1;
            Message = Encoding.UTF8.GetString(packet, offset, packet.Length - offset);
        }

        public MessageRequest(string message)
        {
            Message = message;
        }

        #endregion Constructors

        #region Methods

        public byte[] GetBytes()
        {
            if (string.IsNullOrEmpty(Message))
                return new[] { (byte)Id };

            byte[] message = Encoding.UTF8.GetBytes(Message);
            byte[] packet = new byte[message.Length + 1];

            int offset = 0;
            BufferPrimitives.SetUint8(packet, ref offset, (byte)Id);
            BufferPrimitives.SetBytes(packet, ref offset, message, 0, message.Length);

            return packet;
        }

        #endregion Methods
    }
}
