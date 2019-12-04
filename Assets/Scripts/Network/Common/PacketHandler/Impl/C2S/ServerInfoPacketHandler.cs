using System.IO;

public class ServerInfoPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Client client)
        {
            ServerInfo serverInfo = new ServerInfo();

            var serverIdSize = reader.ReadByte();
            var serverId = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(serverIdSize));
            serverInfo.serverId = serverId;
            var hostSize = reader.ReadByte();
            var host = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(hostSize));
            serverInfo.host = host;
            serverInfo.port = reader.ReadUInt16();
            serverInfo.count = reader.ReadByte();
            serverInfo.maxCount = reader.ReadByte();

            ServerInfo exist = client.availableServers.Find(si => si.Equals(serverInfo));
            if (exist == null)
            {
                client.availableServers.Add(serverInfo);
            }
            else
            {
                exist.UpdateInfo(serverInfo);
            }
        }
    }
}
