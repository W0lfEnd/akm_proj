using System.IO;
using UnityEngine;

public class JoinedPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Client client)
        {
            var id = reader.ReadInt64();
            if( client.Id == id )
            {
                Debug.Log($"JOINED id: {id}");
                return;
            }

            var nameSize = reader.ReadByte();
            var name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(nameSize));
            client.otherClients[id] = name;

            Debug.Log($"JOINED id: {id} name: {name}");
        }
    }
}

