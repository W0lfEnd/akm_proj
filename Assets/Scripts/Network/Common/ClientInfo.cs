using Lidgren.Network;
using System.Net;

public class ClientInfo
{
    public IPAddress IpAddress;
    public ushort Port;
    public long ConnectionId;
    public NetConnection NetConnection;

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        var info = obj as ClientInfo;
        if (info == null)
        {
            return false;
        }

        if (info.ConnectionId == ConnectionId) // for delete ???
        {
            return true;
        }

        return false;
    }

    public override int GetHashCode()
    { 
        var hashCode = ConnectionId.GetHashCode(); // for delete ??? 
        return hashCode;
    }
}