using System;
using UnityEngine;
using Util;

namespace Game.LevelGenerator.LevelSOs
{
    public class DungeonSOTester : MonoBehaviour
    {
        [SerializeField]
        DungeonFileSo currentDungeonSO;

        public void OnDrawGizmos()
        {
            foreach (var room in currentDungeonSO.rooms)
            {
                var size = new Vector3(3, 3, 0);
                // Draw a yellow sphere at the transform's position
                if (room.type == Constants.RoomTypeString.CORRIDOR)
                {
                    Gizmos.color = Color.gray;
                }
                else
                {
                    size = new Vector3(5, 5, 0);
                    Gizmos.color = room.type == Constants.RoomTypeString.START ? Color.white : Color.magenta;
                }
                var position = new Vector3(room.coordinates.X * 10, room.coordinates.Y * 10, 0);
                Gizmos.DrawCube(position, size);
                if (room.keys?.Count > 0)
                {
                    Gizmos.color = room.keys[0] switch
                    {
                        1 => Color.yellow,
                        2 => Color.blue,
                        3 => Color.green,
                        4 => Color.red,
                        5 => new Color(255, 165, 0),
                        6 => new Color(230, 230, 250),
                        _ => Color.cyan
                    };
                    position = new Vector3(room.coordinates.X * 10, room.coordinates.Y * 10, 5);
                    Gizmos.DrawSphere(position, 2);
                }

                if (room.locks?.Count <= 0) continue;
                if (room.locks != null)
                    Gizmos.color = Math.Abs(room.locks[0]) switch
                    {
                        1 => Color.yellow,
                        2 => Color.blue,
                        3 => Color.green,
                        4 => Color.red,
                        5 => new Color(255, 165, 0),
                        6 => new Color(230, 230, 250),
                        _ => Color.cyan
                    };
                Gizmos.DrawCube(position, size);
            }
        }
    }    
}
