using System.Collections.Generic;
using System.IO;

public class PacketHandlerManager
{
    private readonly Dictionary<PacketType, IPacketHandler> _packetHandlers = new Dictionary<PacketType, IPacketHandler>();

    public bool AddHandler(PacketType packetType, IPacketHandler handler)
    {
        if (!_packetHandlers.ContainsKey(packetType))
        {
            _packetHandlers.Add(packetType, handler);
            return true;
        }

        return false;
    }

    public bool RemoveHandlerByType(PacketType command)
    {
        return _packetHandlers.Remove(command);
    }

    public bool HasHandlerPacketForType(PacketType packetType)
    {
        bool result = _packetHandlers.ContainsKey(packetType) && _packetHandlers[packetType] != null;
        return result;
    }

    public void RunPacketHandler(byte[] message, object source) // todo change to concrete interface
    {
        using (var stream = new MemoryStream(message))
        {
            using (var reader = new BinaryReader(stream))
            {
                var packetType = (PacketType)reader.ReadByte();
                if (HasHandlerPacketForType(packetType))
                {
                    _packetHandlers[packetType].HandlerPacket(reader, source);
                }
            }
        }
    }
}