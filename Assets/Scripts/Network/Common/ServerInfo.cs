public class ServerInfo
{
    public string serverId;
    public string host;
    public ushort port;
    public byte count;
    public byte maxCount;

    public void UpdateInfo(ServerInfo serverInfo)
    {
        count = serverInfo.count;
    }

    public override bool Equals(object obj)
    {
        ServerInfo other = obj as ServerInfo;
        if(other == null)
        {
            return false;
        }

        return other.host == this.host && other.port == this.port;
    }
}

