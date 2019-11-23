using System.Collections.Generic;
using System.Linq;

public static class Util
{
    public static List<T> ShuffleList<T>( List<T> source)
    {
        List<T> randomList = new List<T>();
        var r = new System.Random();
        int randomIndex = 0;

        while (source.Count > 0)
        {
            randomIndex = r.Next(0, source.Count);
            randomList.Add(source[randomIndex]);
            source.RemoveAt(randomIndex);
        }

        return randomList;
    }

    public static List<byte> getSource( byte start, byte count )
    {
        List<byte> source = new List<byte>();
        var range = Enumerable.Range(start, count);
        foreach (var item in range)
        {
            source.Add((byte)item);
        }

        return source;
    }
}

