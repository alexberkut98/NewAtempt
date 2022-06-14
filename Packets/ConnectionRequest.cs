using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.Packets
{
    public class ConnectionRequest
    {
        #region Properties

        public PacketId Id => PacketId.ConnectionRequest;

        public string Login { get; }

        #endregion Properties

        #region Constructors

        public ConnectionRequest(byte[] packet)
        {
            int offset = 1;
            Login = Encoding.UTF8.GetString(packet, offset, packet.Length - offset);
        }

        public ConnectionRequest(string login)
        {
            Login = login;
        }

        #endregion Constructors

        #region Methods

        public byte[] GetBytes()
        {
            if (string.IsNullOrEmpty(Login))
                return new[] { (byte)Id };

            byte[] login = Encoding.UTF8.GetBytes(Login);
            byte[] packet = new byte[login.Length + 1];

            int offset = 0;
            BufferPrimitives.SetUint8(packet, ref offset, (byte)Id);
            BufferPrimitives.SetBytes(packet, ref offset, login, 0, login.Length);

            return packet;
        }

        #endregion Methods
    }
}
