using UnityEngine;

namespace Assets.Scripts.Game.LevelGenerator.LevelSOs
{
    public class DungeonSOTester : MonoBehaviour
    {
        [SerializeField]
        DungeonFileSO currentDungeonSO;


        public void OnDrawGizmos()
        {
            foreach (SORoom room in currentDungeonSO.rooms)
            {
                Vector3 size = new Vector3(3, 3, 0);
                // Draw a yellow sphere at the transform's position
                if (room.type == "c")
                {
                    Gizmos.color = Color.gray;
                }
                else
                {
                    size = new Vector3(5, 5, 0);
                    if (room.type == "s")
                    {
                        Gizmos.color = Color.white;
                    }
                    else
                    {
                        Gizmos.color = Color.magenta;
                    }
                }
                Vector3 position = new Vector3(room.coordinates.X * 10, room.coordinates.Y * 10, 0);
                Gizmos.DrawCube(position, size);
                if (room.keys.Count > 0)
                {
                    if (room.keys[0] == 1)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    else if (room.keys[0] == 2)
                    {
                        Gizmos.color = Color.blue;
                    }
                    else if (room.keys[0] == 3)
                    {
                        Gizmos.color = Color.green;
                    }
                    else if (room.keys[0] == 4)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.cyan;
                    }
                    position = new Vector3(room.coordinates.X * 10, room.coordinates.Y * 10, 5);
                    Gizmos.DrawSphere(position, 2);
                }
                if (room.locks.Count > 0)
                {
                    if (room.locks[0] == -1)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    else if (room.locks[0] == -2)
                    {
                        Gizmos.color = Color.blue;
                    }
                    else if (room.locks[0] == -3)
                    {
                        Gizmos.color = Color.green;
                    }
                    else if (room.locks[0] == -4)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.cyan;
                    }
                    Gizmos.DrawCube(position, size);
                }
            }
        }
    }

    
}
