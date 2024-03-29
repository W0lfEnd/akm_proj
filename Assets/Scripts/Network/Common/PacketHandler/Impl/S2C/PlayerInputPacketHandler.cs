﻿using System.IO;
using UnityEngine;

public class PlayerInputPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Server server)
        {
            PlayerInput playerInput = new PlayerInput()
            {
                ownerId = reader.ReadInt64(),
                actionType = (PlayerInput.ActionType)reader.ReadByte(),
                panelId = reader.ReadByte(),
                inputElementId = reader.ReadByte(),
                inputValue = reader.ReadByte(),
                targetPosition = new Vector2Int(reader.ReadInt32(), reader.ReadInt32()),
                sectorPosition = reader.ReadByte(),
            };

            server.GameController.ApplyPlayerInput(playerInput);
        }
    }
}
