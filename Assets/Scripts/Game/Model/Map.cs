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
                new Vector2Int(0, 0),
                new Vector2Int(6000, 6000),
                new Vector2Int(4500, 7500),
                new Vector2Int(3000, 8000)
            };

            meteorsData = GenerateMeteors();
        }

        private MeteorData[] GenerateMeteors()
        {
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

                return Util.ShuffleList(10, 15).Take(size).ToArray();
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
