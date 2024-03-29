﻿using Model;
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
                client.Model.startCombo= new byte[4];
            }

            client.Model.gameState.Value = (GameState)reader.ReadByte();
            client.Model.currentTime.Value = reader.ReadInt32();
            client.Model.health.Value = reader.ReadInt16();
            client.Model.shield.Value = reader.ReadInt16();
            client.Model.oxygen.Value = reader.ReadByte();
            client.Model.speed.Value = reader.ReadByte();
            client.Model.petrol.Value = reader.ReadByte();

            client.Model.curPosition.Value = new Vector2Int(reader.ReadInt32(), reader.ReadInt32());
            client.Model.targetPosition.Value = new Vector2Int(reader.ReadInt32(), reader.ReadInt32());

            client.Model.startCombo[0] = reader.ReadByte();
            client.Model.startCombo[1] = reader.ReadByte();
            client.Model.startCombo[2] = reader.ReadByte();
            client.Model.startCombo[3] = reader.ReadByte();

            client.Model.iteration.Value = reader.ReadInt32();

            byte maneurComboValidStateSize = reader.ReadByte();
            client.Model.maneverComboValidState = new bool[maneurComboValidStateSize];
            for (int i = 0; i < maneurComboValidStateSize; i++)
            {
                client.Model.maneverComboValidState[i] = reader.ReadBoolean();
            }

            byte panelCount = reader.ReadByte();
            if (client.Model.panels == null)
            {
                client.Model.panels = new Panel[panelCount];
            }

            for (int i = 0; i < panelCount; i++)
            {
                var panel = new Panel();

                panel.id = reader.ReadByte();
                panel.ownerId = reader.ReadInt64();
                byte elementCount = reader.ReadByte();
                if (panel.inputElements == null)
                {
                    panel.inputElements = new InputElement[elementCount];
                }

                for (int j = 0; j < elementCount; j++)
                {
                    panel.inputElements[j] = new InputElement
                    {
                        inputType = (InputType)reader.ReadByte(),
                        id = reader.ReadByte(),
                        groupId = reader.ReadByte(),
                        maxValue = reader.ReadByte(),
                        inputValue = reader.ReadByte()
                    };
                }
                client.Model.panels[i] = panel;
            }

            if (client.Model.sectors == null)
            {
                byte secrotCount = reader.ReadByte();
                client.Model.sectors = new Sector[secrotCount];
                for (int i = 0; i < secrotCount; i++)
                {
                    client.Model.sectors[i] = new Sector
                    {
                        position = reader.ReadByte(),
                        sectorType = (SectorType)reader.ReadByte(),
                        health = reader.ReadByte(),
                        isFire = reader.ReadBoolean(),
                        isRepairing = reader.ReadBoolean(),
                };
                }
            }
        }
    }
}

