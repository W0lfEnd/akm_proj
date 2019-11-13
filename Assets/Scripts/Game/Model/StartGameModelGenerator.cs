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
                gameState = GameState.NONE,
                shield = startValue,
                oxygen = startValue,
                petrol = startValue,
                health = startValue,
                curPosition = new Vector2Int(0, 0),
                targetPosition = new Vector2Int(0, 0),
                speed = 0,
                startCombo = Util.ShuffleList(0, 4).ToArray(),
                currentTime = 0,
                iteration = 0
            };

            model.panels = new Panel[countPanel];

            for (byte i = 0; i < countPanel; i++)
            {
                model.panels[i] = generatePanel(i, countInputOnPanel);
            }

            return model;
        }

        private static Panel generatePanel(byte id, byte countInputOnPanel)
        {
            Panel panel = new Panel
            {
                id = id,
                ownerId = -1
            };

            return panel;
        }

        private static InputElement[] createControlPanel()
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
                    id = 4, // speed id
                    groupId = 0,
                    inputType = InputType.Slider,
                    inputValue = 0,
                    maxValue = 10
                },
                new InputElement // fire id
                {
                    id = 5,
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
