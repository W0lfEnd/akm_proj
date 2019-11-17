using System.IO;
using UnityEngine;

public class SendIdPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Client client)
        {
            client.Id = reader.ReadInt32();
            Debug.Log($"SendIdPacketHandler: {client.Id}");
        }
    }
}

