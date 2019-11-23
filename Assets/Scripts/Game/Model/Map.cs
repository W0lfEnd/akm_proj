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
                new Vector2Int( 300, 300 ),
                new Vector2Int( 4000, 1000 ),
                new Vector2Int( 900, 3700 ),
                new Vector2Int( 4000, 3500 ),
                new Vector2Int( 2500, 400 ),
                new Vector2Int( 1000, 2000 ),
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

                var result = Util.ShuffleList( Util.getSource(10, 6)).Take(size).ToArray();
                return result;
            }

            var data = new MeteorData[150];
            var time = 20;

            data[0] = new MeteorData { timeSeconds = time, size = 0, combo = createCombo(0) };

            for (int i = 1; i < data.Length; i++)
            {
                var random = new System.Random();
                var meteorSize = random.Next(0, 3);
                time += random.Next(30, 45);
                data[i] = new MeteorData { timeSeconds = time, size = (byte)meteorSize, combo = createCombo(meteorSize) };
            }

            return data;
        }
    }
}
