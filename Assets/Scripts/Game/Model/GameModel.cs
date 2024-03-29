﻿using UnityEngine;

namespace Model
{
    public class GameModel
    {
        public ObservedValue<GameState> gameState = new ObservedValue<GameState>();

        public ObservedValue<int> currentTime = new ObservedValue<int>();

        public ObservedValue<short> health = new ObservedValue<short>();
        public ObservedValue<short> shield = new ObservedValue<short>();
        public ObservedValue<byte> oxygen = new ObservedValue<byte>();
        public ObservedValue<byte> speed  = new ObservedValue<byte>();
        public ObservedValue<byte> petrol = new ObservedValue<byte>();

        public ObservedValue<Vector2Int> curPosition = new ObservedValue<Vector2Int>();
        public ObservedValue<Vector2Int> targetPosition = new ObservedValue<Vector2Int>();

        public byte[] startCombo;
        public ObservedValue<int> iteration = new ObservedValue<int>();
        public bool[] maneverComboValidState;

        public Panel[] panels;

        public Sector[] sectors;
    }
}
