using System;

public class PacketFactory
{
    public static Packet CreatePacketByType(PacketType packetType, object context)
    {
        switch (packetType)
        {
        }

        throw new Exception("NotSuported type of packet");
    }
}
