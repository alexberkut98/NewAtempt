using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.Packets
{
    public enum PacketId : byte
    {
        ConnectionRequest = 0x01,
        ConnectionResponse = 0x02,
        MessageRequest = 0x03,
        MessageBroadcast = 0x04,
    }
}
