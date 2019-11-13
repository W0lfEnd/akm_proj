using System.Collections.Generic;
using System.Linq;

public static class Util
{
    public static List<byte> ShuffleList(byte start, byte end)
    {
        List<byte> source = Enumerable.Range(start, end + 1).Cast<byte>().ToList();
        List<byte> randomList = new List<byte>();
        var r = new System.Random();
        int randomIndex = 0;

        while (source.Count > 0)
        {
            randomIndex = r.Next(0, source.Count);
            randomList.Add((byte)source[randomIndex]);
            source.RemoveAt(randomIndex);
        }

        return randomList;
    }
}

