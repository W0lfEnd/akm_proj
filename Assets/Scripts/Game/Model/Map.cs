using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    public class Map
    {
        public MeteorData[] meteorsData;
        public Vector2Int[] coords;

        public Map()
        {
            coords = new Vector2Int[]
            {
                new Vector2Int(),
                new Vector2Int(),
                new Vector2Int(),
                new Vector2Int()
            };

            meteorsData = GenerateMeteors();
        }

        private MeteorData[] GenerateMeteors()
        {
            List<byte> ShuffleList(List<int> inputList)
            {
                List<byte> randomList = new List<byte>();

                var r = new System.Random();
                int randomIndex = 0;
                while (inputList.Count > 0)
                {
                    randomIndex = r.Next(0, inputList.Count);
                    randomList.Add((byte)inputList[randomIndex]);
                    inputList.RemoveAt(randomIndex);
                }

                return randomList;
            }

            byte[] createCombo(int meteorSize)
            {
                var size = 4;

                if (meteorSize == 1)
                {
                    size = 5;
                }
                else if (meteorSize == 2)
                {
                    size = 6;
                }

                var combo = Enumerable.Range(0, size + 1).ToList();

                return ShuffleList(combo).ToArray();
            }

            var data = new MeteorData[100];
            var time = 30;

            data[0] = new MeteorData { timeSeconds = time, size = 0, combo = createCombo(0) };

            for (int i = 1; i < 100; i++)
            {
                var random = new System.Random();
                var meteorSize = random.Next(0, 3);
                time += random.Next(20, 45);
                data[i] = new MeteorData { timeSeconds = time, size = (byte)meteorSize, combo = createCombo(meteorSize) };
            }

            return data;
        }
    }
}
