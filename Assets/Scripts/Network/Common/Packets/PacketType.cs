public enum PacketType : byte
{
    S2C_Debug = 0,

    S2C_Map = 1,
    S2C_Model = 2,
    S2C_Joined = 3,

    C2S_Input = 10,
    C2S_Join = 11,

}