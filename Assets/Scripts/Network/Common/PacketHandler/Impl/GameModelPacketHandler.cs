using Model;
using System.IO;
using UnityEngine;

public class GameModelPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Client client)
        {
            if (client.Model == null)
            {
                client.Model = new GameModel();
                client.Model.startCombo = new byte[4];
            }

            client.Model.gameState = (GameState)reader.ReadByte();
            client.Model.currentTime = reader.ReadInt32();
            client.Model.health = reader.ReadByte();
            client.Model.shield = reader.ReadByte();
            client.Model.oxygen = reader.ReadByte();
            client.Model.speed = reader.ReadByte();
            client.Model.petrol = reader.ReadByte();

            client.Model.curPosition = new Vector2Int(reader.ReadInt32(), reader.ReadInt32());
            client.Model.targetPosition = new Vector2Int(reader.ReadInt32(), reader.ReadInt32());

            client.Model.startCombo[0] = reader.ReadByte();
            client.Model.startCombo[1] = reader.ReadByte();
            client.Model.startCombo[2] = reader.ReadByte();
            client.Model.startCombo[3] = reader.ReadByte();

            client.Model.iteration = reader.ReadInt32();

            byte panelCount = reader.ReadByte();
            if (client.Model.panels == null)
            {
                client.Model.panels = new Panel[panelCount];
            }

            for (int i = 0; i < panelCount; i++)
            {
                client.Model.panels[i].id = reader.ReadByte();
                client.Model.panels[i].ownerId = reader.ReadInt32();
                byte elementCount = reader.ReadByte();
                if (client.Model.panels[i].inputElements == null)
                {
                    client.Model.panels[i].inputElements = new InputElement[elementCount];
                }

                for (int j = 0; j < elementCount; j++)
                {
                    client.Model.panels[i].inputElements[j] = new InputElement
                    {
                        inputType = (InputType)reader.ReadByte(),
                        id = reader.ReadByte(),
                        groupId = reader.ReadByte(),
                        maxValue = reader.ReadByte(),
                        inputValue = reader.ReadByte()
                    };
                }
            }
        }
    }
}

