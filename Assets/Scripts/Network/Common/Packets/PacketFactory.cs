using Model;
using System;

public class PacketFactory
{
    public static Packet CreatePacketByType(PacketType packetType, object context)
    {
        var packet = new Packet(packetType);
        switch (packetType)
        {
            case PacketType.S2C_Map:
                var map = context as Map;
                if (map == null)
                {
                    throw new Exception($"{nameof(context)} is not type {nameof(Map)}");
                }
                saveMapToPacket(packet, map);
                break;
            case PacketType.S2C_Model:
                var model = context as GameModel;
                if (model == null)
                {
                    throw new Exception($"{nameof(context)} is not type {nameof(GameModel)}");
                }
                saveModelToPacket(packet, model);
                break;
            case PacketType.C2S_Input:
                var playerInput = context as PlayerInput;
                if (playerInput == null)
                {
                    throw new Exception($"{nameof(context)} is not type {nameof(PlayerInput)}");
                }
                savePalyerInput(packet, playerInput);
                break;
            case PacketType.S2C_SendId:
                if (!int.TryParse( context.ToString(), out int id))
                {
                    
                }
                packet.Buffer.Write(id);
                break;
            default: throw new Exception("NotSuported type of packet");

        }
        return packet;
    }

    private static void saveMapToPacket(Packet packet, Map map)
    {
        packet.Buffer.Write((byte)map.coords.Length); // byte
        for (int i = 0; i < map.coords.Length; i++)
        {
            packet.Buffer.Write(map.coords[i].x); // int
            packet.Buffer.Write(map.coords[i].y); // int
        }

        packet.Buffer.Write((byte)map.meteorsData.Length); // byte
        for (int i = 0; i < map.meteorsData.Length; i++) 
        {
            packet.Buffer.Write(map.meteorsData[i].timeSeconds); // int
            packet.Buffer.Write(map.meteorsData[i].size); // byte

            packet.Buffer.Write((byte)map.meteorsData[i].combo.Length); // byte
            for (int j = 0; j < map.meteorsData[i].combo.Length; j++) 
            {
                packet.Buffer.Write(map.meteorsData[i].combo[j]); // byte
            }
        }
    }

    private static void saveModelToPacket(Packet packet, GameModel model)
    {
        packet.Buffer.Write((byte)model.gameState.Value);
        packet.Buffer.Write(model.currentTime.Value);
        packet.Buffer.Write(model.health.Value);
        packet.Buffer.Write(model.shield.Value);
        packet.Buffer.Write(model.oxygen.Value);
        packet.Buffer.Write(model.speed.Value);
        packet.Buffer.Write(model.petrol.Value);

        packet.Buffer.Write(model.curPosition.Value.x);
        packet.Buffer.Write(model.curPosition.Value.y);
        packet.Buffer.Write(model.targetPosition.Value.x);
        packet.Buffer.Write(model.targetPosition.Value.y);

        packet.Buffer.Write(model.startCombo[0]);
        packet.Buffer.Write(model.startCombo[1]);
        packet.Buffer.Write(model.startCombo[2]);
        packet.Buffer.Write(model.startCombo[3]);

        packet.Buffer.Write(model.iteration.Value);

        packet.Buffer.Write((byte)model.panels.Length);
        for (int i = 0; i < model.panels.Length; i++)
        {
            packet.Buffer.Write(model.panels[i].id);
            packet.Buffer.Write(model.panels[i].ownerId);

            packet.Buffer.Write((byte)model.panels[i].inputElements.Length);
            for (int j = 0; j < model.panels[i].inputElements.Length; j++)
            {
                packet.Buffer.Write((byte)model.panels[i].inputElements[j].inputType);
                packet.Buffer.Write(model.panels[i].inputElements[j].id);
                packet.Buffer.Write(model.panels[i].inputElements[j].groupId);
                packet.Buffer.Write(model.panels[i].inputElements[j].maxValue);
                packet.Buffer.Write(model.panels[i].inputElements[j].inputValue);
            }
        }

        packet.Buffer.Write((byte)model.sectors.Length);
        for (int i = 0; i < model.sectors.Length; i++)
        {
            packet.Buffer.Write(model.sectors[i].position);
            packet.Buffer.Write((byte)model.sectors[i].sectorType);
        }
    }

    private static void savePalyerInput(Packet packet, PlayerInput playerInput)
    {
        packet.Buffer.Write(playerInput.ownerId);
        packet.Buffer.Write((byte)playerInput.actionType);
        packet.Buffer.Write(playerInput.panelId);
        packet.Buffer.Write(playerInput.inputElementId);
        packet.Buffer.Write(playerInput.inputValue);
        packet.Buffer.Write(playerInput.targetPosition.x);
        packet.Buffer.Write(playerInput.targetPosition.y);
    }
}
