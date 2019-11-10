﻿public class PlayerInput
{
    public enum ActionType : byte
    {
        ChangePanel,
        PressButton
    }

    public int ownerId;
    public ActionType actionType;
    public byte panelId;
    public byte inputElementId;
    public byte inputValue;
}
