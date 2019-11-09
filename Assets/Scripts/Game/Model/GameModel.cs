using UnityEngine;

namespace Model
{
    public class GameModel
    {
        public GameState gameState;

        public byte health;
        public byte shield;
        public byte oxygen;
        public byte speed;
        public byte petrol;

        public Vector2Int curPosition;
        public Vector2Int targetPosition;

        public Panel[] panels;
    }
}
