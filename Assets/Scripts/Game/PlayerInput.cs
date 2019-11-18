using UnityEngine;

public class PlayerInput
{
    public enum ActionType : byte
    {
        ChangePanel,
        PressButton,
        ChangeTarget
    }

    public int ownerId;
    public ActionType actionType;
    public byte panelId;
    public byte inputElementId;
    public byte inputValue;
    public Vector2Int targetPosition;
}
