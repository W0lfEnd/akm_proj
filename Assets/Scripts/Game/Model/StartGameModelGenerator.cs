using System.Linq;
using UnityEngine;

namespace Model
{
    public static class StartGameModelGenerator
    {
        public static GameModel Generate(byte countPanel, byte countInputOnPanel, byte startValue = 100)
        {
            GameModel model = new GameModel
            {
                gameState      = new ObservedValue<GameState>( GameState.NONE ),
                shield         = new ObservedValue<ushort>( 800 ),
                health         = new ObservedValue<ushort>( 800 ),
                oxygen         = new ObservedValue<byte>( startValue ),
                petrol         = new ObservedValue<byte>( startValue ),
                curPosition    = new ObservedValue<Vector2Int>( new Vector2Int( 0, 0 ) ),
                targetPosition = new ObservedValue<Vector2Int>( new Vector2Int( 0, 0 ) ),
                speed          = new ObservedValue<byte>( 40 ),//TODO remove hardcoded speed by dynamicly changed
                currentTime    = new ObservedValue<int>( 0 ),
                iteration      = new ObservedValue<int>( 0 )
            };
            model.startCombo = Util.ShuffleList(Util.getSource(0, 4)).ToArray();
            
            model.panels = new Panel[countPanel];

            InputElement[] inputElements = createInputElements();
            var panelIds = new byte[]{ 0, 1, 2, 3, 4 };//Util.ShuffleList(Util.getSource(0, 5));

            for (byte i = 0; i < 3; i++)
            {
                InputElement[] inputs = inputElements.Skip(i * countInputOnPanel).Take(countInputOnPanel).ToArray();
                model.panels[i] = generatePanel(panelIds[i], inputs);
            }

            model.panels[3] = new Panel { id = panelIds[3], ownerId = -1, inputElements = new InputElement[] { new InputElement { id = 6, groupId = 100, inputType = InputType.Output, inputValue = 0, maxValue = 0 } } };
            model.panels[4] = new Panel { id = panelIds[4], ownerId = -1, inputElements = new InputElement[] { new InputElement { id = 7, groupId = 200, inputType = InputType.Output, inputValue = 0, maxValue = 0 } } };

            model.sectors = GetSectors();

            return model;
        }

        private static Sector[] GetSectors()
        {
            return new Sector[]
            {
                new Sector{ position = 0, sectorType = SectorType.empty, health = 100 },
                new Sector{ position = 1, sectorType = SectorType.empty, health = 100 },
                new Sector{ position = 2, sectorType = SectorType.empty, health = 100 },
                new Sector{ position = 3, sectorType = SectorType.empty, health = 100 },
                new Sector{ position = 4, sectorType = SectorType.oxygen, health = 100 },
                new Sector{ position = 5, sectorType = SectorType.empty, health = 100 },
                new Sector{ position = 6, sectorType = SectorType.shield, health = 100 },
                new Sector{ position = 7, sectorType = SectorType.empty, health = 100 },
            };
        }

        private static Panel generatePanel(byte id, InputElement[] inputElements )
        {
            Panel panel = new Panel
            {
                id = id,
                ownerId = -1,
                inputElements = inputElements
            };

            return panel;
        }

        private static InputElement[] createInputElements()
        {
            return new InputElement[]
            {
                new InputElement
                {
                    id = 0,
                    groupId = 2,
                    inputType = InputType.Toggle,
                    inputValue = 0,
                    maxValue = 1
                },
                new InputElement
                {
                    id = 1,
                    groupId = 2,
                    inputType = InputType.Toggle,
                    inputValue = 0,
                    maxValue = 1
                },
                new InputElement
                {
                    id = 2,
                    groupId = 2,
                    inputType = InputType.Toggle,
                    inputValue = 0,
                    maxValue = 1
                },
                new InputElement
                {
                    id = 3,
                    groupId = 2,
                    inputType = InputType.Toggle,
                    inputValue = 0,
                    maxValue = 1
                },

                new InputElement
                {
                    id = 4,
                    groupId = 2,
                    inputType = InputType.Button,
                    inputValue = 0,
                    maxValue = 1
                },

                new InputElement
                {
                    id = 5, // speed id
                    groupId = 0,
                    inputType = InputType.Slider,
                    inputValue = 0,
                    maxValue = 10
                },
                new InputElement // fire id
                {
                    id = 6,
                    groupId = 0,
                    inputType = InputType.Button,
                    inputValue = 0,
                    maxValue = 1
                },

                new InputElement
                {
                    id = 10,
                    groupId = 1,
                    inputType = InputType.Button,
                    inputValue = 0,
                    maxValue = 1
                },
                new InputElement
                {
                    id = 11,
                    groupId = 1,
                    inputType = InputType.Button,
                    inputValue = 0,
                    maxValue = 1
                },
                new InputElement
                {
                    id = 12,
                    groupId = 1,
                    inputType = InputType.Button,
                    inputValue = 0,
                    maxValue = 1
                },
                new InputElement
                {
                    id = 13,
                    groupId = 1,
                    inputType = InputType.Button,
                    inputValue = 0,
                    maxValue = 1
                },
                new InputElement
                {
                    id = 14,
                    groupId = 1,
                    inputType = InputType.Button,
                    inputValue = 0,
                    maxValue = 1
                },
                new InputElement
                {
                    id = 15,
                    groupId = 1,
                    inputType = InputType.Button,
                    inputValue = 0,
                    maxValue = 1
                }
            };
        }
    }
}
