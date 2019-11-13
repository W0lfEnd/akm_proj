using Model;
using System.IO;
using UnityEngine;

public class GameModelPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Client client)
        {
            if (client.GameModel == null)
            {
                client.GameModel = new GameModel();
                client.GameModel.startCombo = new byte[4];
            }

            client.GameModel.gameState = (GameState)reader.ReadByte();
            client.GameModel.currentTime = reader.ReadInt32();
            client.GameModel.health = reader.ReadByte();
            client.GameModel.shield = reader.ReadByte();
            client.GameModel.oxygen = reader.ReadByte();
            client.GameModel.speed = reader.ReadByte();
            client.GameModel.petrol = reader.ReadByte();

            client.GameModel.curPosition = new Vector2Int(reader.ReadInt32(), reader.ReadInt32());
            client.GameModel.targetPosition = new Vector2Int(reader.ReadInt32(), reader.ReadInt32());

            client.GameModel.startCombo[0] = reader.ReadByte();
            client.GameModel.startCombo[1] = reader.ReadByte();
            client.GameModel.startCombo[2] = reader.ReadByte();
            client.GameModel.startCombo[3] = reader.ReadByte();

            client.GameModel.iteration = reader.ReadInt32();

            byte panelCount = reader.ReadByte();
            if (client.GameModel.panels == null)
            {
                client.GameModel.panels = new Panel[panelCount];
            }

            for (int i = 0; i < panelCount; i++)
            {
                client.GameModel.panels[i].id = reader.ReadByte();
                client.GameModel.panels[i].ownerId = reader.ReadInt32();
                byte elementCount = reader.ReadByte();
                if (client.GameModel.panels[i].inputElements == null)
                {
                    client.GameModel.panels[i].inputElements = new InputElement[elementCount];
                }

                for (int j = 0; j < elementCount; j++)
                {
                    client.GameModel.panels[i].inputElements[j] = new InputElement
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

