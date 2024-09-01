using UnityEngine;

namespace Game.LevelManager.DungeonManager
{
    public class TileBhv : MonoBehaviour
    {
        public void SetPosition(float x, float y, Transform parent)
        {
            Transform tileTransform = transform;
            tileTransform.SetParent(parent);
            tileTransform.localPosition = new Vector2(x, y);
        }
    }
}