using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    public class Map
    {
        public MeteorData[] meteorsData;
        public Vector2Int[] coords;

        public const int MAP_SIZE = 5000;
        public Map()
        {
            coords = new Vector2Int[]
            {
                new Vector2Int( MAP_SIZE / 5, MAP_SIZE / 5 ),
                new Vector2Int( MAP_SIZE / 8, MAP_SIZE / 10 ),
                new Vector2Int( MAP_SIZE / 3, MAP_SIZE / 6 ),
                new Vector2Int( MAP_SIZE / 54, MAP_SIZE / 23 )
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

                var result = Util.ShuffleList(10, 6).Take(size).ToArray();
                return result;
            }

            var data = new MeteorData[5];
            var time = 30;

            data[0] = new MeteorData { timeSeconds = time, size = 0, combo = createCombo(0) };

            for (int i = 1; i < data.Length; i++)
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
