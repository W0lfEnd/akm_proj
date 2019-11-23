using Model;
using System.IO;
using UnityEngine;

public class MapPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Client client)
        {
            client.Map = new Map();

            byte coordsSize = reader.ReadByte();
            client.Map.coords = new Vector2Int[coordsSize];
            for (int i = 0; i < coordsSize; i++)
            {
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();
                client.Map.coords[i] = new Vector2Int(x, y);
            }

            Debug.Log("Map coord point count: " + coordsSize);

            byte meteorDataCount = reader.ReadByte();
            client.Map.meteorsData = new MeteorData[meteorDataCount];
            for (int i = 0; i < meteorDataCount; i++)
            {
                int seconds = reader.ReadInt32();
                byte meteorSize = reader.ReadByte();

                byte comboSize = reader.ReadByte();
                byte[] combo = new byte[comboSize];

                for (int j = 0; j < comboSize; j++)
                {
                    combo[j] = reader.ReadByte();
                }

                client.Map.meteorsData[i] = new MeteorData { timeSeconds = seconds, size = meteorSize, combo = combo };
            }

            Debug.Log("Map meteor count: " + meteorDataCount);
        }
    }
}