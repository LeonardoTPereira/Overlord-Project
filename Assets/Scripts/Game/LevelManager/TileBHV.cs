using UnityEngine;

namespace Game.LevelManager
{
    public class TileBHV : MonoBehaviour
    {
        private int x;
        private int y;
        private int id;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Id { get => id; set => id = value; }

        public void SetPosition(int x, int y, float centerX, float centerY)
        {
            X = x;
            Y = y;
            transform.localPosition = new Vector2(centerX, centerY);
        }
    }
}