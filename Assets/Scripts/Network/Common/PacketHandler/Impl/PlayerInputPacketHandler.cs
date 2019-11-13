using System.IO;

public class PlayerInputPacketHandler : IPacketHandler
{
    public void HandlerPacket(BinaryReader reader, object source)
    {
        if (source is Server server)
        {
            PlayerInput playerInput = new PlayerInput()
            {
                ownerId = reader.ReadInt32(),
                actionType = (PlayerInput.ActionType)reader.ReadByte(),
                panelId = reader.ReadByte(),
                inputElementId = reader.ReadByte(),
                inputValue = reader.ReadByte()
            };

            server.GameController.ApplyPlayerInput(playerInput);
        }
    }
}
