using System.IO;

public class JoinedPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Client client)
        {
            var count = reader.ReadByte();

            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt64();
                var nameSize = reader.ReadByte();
                var name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(nameSize));
                if (client.Id != id)
                {
                    client.otherClients[id] = name;
                }
            }
        }
    }
}

