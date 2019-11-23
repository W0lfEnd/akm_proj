
public enum SectorType : byte
{
    empty,
    shield,
    oxygen,
}

public class Sector
{
    public byte position;
    public SectorType sectorType;
    public byte health;
    public bool isFire;
    public bool isRepairing;
}
