﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
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

            var clientInfos = new List<Tuple<long, string>>();
            foreach (KeyValuePair<long, string> item in server.clientInfos)
            {
                clientInfos.Add(new Tuple<long, string>(item.Key, item.Value));
            }

            var messageId = server._server.CreateMessage();
            var packetId = PacketFactory.CreatePacketByType(PacketType.S2C_Joined, clientInfos);
            messageId.Write(packetId.GetData());
            server._server.SendToAll(messageId, NetDeliveryMethod.ReliableOrdered);

            packetId.Dispose();
        }
    }
}
