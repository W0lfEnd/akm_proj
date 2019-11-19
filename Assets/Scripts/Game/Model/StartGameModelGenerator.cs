﻿using System.Linq;
using UnityEngine;

namespace Model
{
    public static class StartGameModelGenerator
    {
        private static byte id;

        public static GameModel Generate(byte countPanel, byte countInputOnPanel, byte startValue = 100)
        {
            GameModel model = new GameModel
            {
                gameState      = new ObservedValue<GameState>( GameState.NONE ),
                shield         = new ObservedValue<byte>( startValue ),
                oxygen         = new ObservedValue<byte>( startValue ),
                petrol         = new ObservedValue<byte>( startValue ),
                health         = new ObservedValue<byte>( startValue ),
                curPosition    = new ObservedValue<Vector2Int>( new Vector2Int( 0, 0 ) ),
                targetPosition = new ObservedValue<Vector2Int>( new Vector2Int( 0, 0 ) ),
                speed          = new ObservedValue<byte>( 0 ),
                currentTime    = new ObservedValue<int>( 0 ),
                iteration      = new ObservedValue<int>( 0 )
            };
            model.startCombo = Util.ShuffleList(0, 4).ToArray();

            model.panels = new Panel[countPanel];

            InputElement[] inputElements = createInputElements();

            for (byte i = 0; i < countPanel; i++)
            {
                InputElement[] inputs = inputElements.Skip(i * countInputOnPanel).Take(countInputOnPanel).ToArray();
                model.panels[i] = generatePanel(i, inputs);
            }

            model.sectors = GetSectors();

            return model;
        }

        private static Sector[] GetSectors()
        {
            return new Sector[]
            {
                new Sector{ position = 0, sectorType = SectorType.empty },
                new Sector{ position = 1, sectorType = SectorType.health },
                new Sector{ position = 2, sectorType = SectorType.oxygen },
                new Sector{ position = 3, sectorType = SectorType.empty },
                new Sector{ position = 4, sectorType = SectorType.petrol },
                new Sector{ position = 5, sectorType = SectorType.empty },
                new Sector{ position = 6, sectorType = SectorType.shield },
                new Sector{ position = 7, sectorType = SectorType.empty },
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
