using Lidgren.Network;
using System;
using System.IO;
using UnityEngine;

public class JoinPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Server server)
        {
            var id = reader.ReadInt64();
            var nameSize = reader.ReadByte();
            var name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(nameSize));

            Debug.Log($"JOIN id: {id} name: {name}");

            var messageId = server._server.CreateMessage();
            var packetId = PacketFactory.CreatePacketByType(PacketType.S2C_Joined, new Tuple<long, string>(id, name) );
            messageId.Write(packetId.GetData());
            server._server.SendToAll(messageId, NetDeliveryMethod.ReliableOrdered);
            packetId.Dispose();
        }
    }
}
