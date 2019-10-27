using System.IO;

public interface IPacketHandler
{
    void HandlerPacket(BinaryReader reader, object source); // todo change to concrete interface
} 
