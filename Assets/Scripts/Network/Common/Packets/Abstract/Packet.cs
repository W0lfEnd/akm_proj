using System;
using System.IO;

public class Packet : IPacket, IDisposable
{
    private PacketType _packetType;
    private MemoryStream _memoryStream;
    private BinaryWriter _buffer;

    public PacketType PacketType => _packetType;
    public BinaryWriter Buffer => _buffer;

    public Packet(PacketType cmd = PacketType.S2C_Debug)
    {
        _packetType = cmd;
        _memoryStream = new MemoryStream();
        _buffer = new BinaryWriter(_memoryStream);
        _buffer.Write((byte)_packetType);
    }

    public byte[] GetData()
    {
        return _memoryStream.ToArray();
    }

    public void Dispose()
    {
        _memoryStream.Dispose();
        _buffer.Dispose();
    }
}
