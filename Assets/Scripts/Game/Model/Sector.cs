
public enum SectorType : byte
{
    empty,
    health,
    shield,
    oxygen,
    petrol,
}

public class Sector
{
    public byte position;
    public SectorType sectorType;
}
