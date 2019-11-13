using UnityEngine;

namespace Model
{
    public class GameModel
    {
        public GameState gameState;

        public int currentTime;

        public byte health;
        public byte shield;
        public byte oxygen;
        public byte speed;
        public byte petrol;

        public Vector2Int curPosition;
        public Vector2Int targetPosition;

        public byte[] startCombo;
        public int iteration;

        public Panel[] panels;
    }
}
