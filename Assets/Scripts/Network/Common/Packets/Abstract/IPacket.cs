using System.IO;

public interface IPacket
{
    PacketType PacketType { get; }
    BinaryWriter Buffer { get; }
    byte[] GetData();
}
