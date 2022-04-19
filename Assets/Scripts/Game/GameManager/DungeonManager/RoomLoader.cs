using Game.LevelManager;
using UnityEngine;
using Util;

namespace Game.GameManager.DungeonManager
{
    public class RoomLoader : MonoBehaviour
    {
        private static readonly float RoomSpacingX = 30f; //Spacing between rooms: X
        private static readonly float RoomSpacingY = 20f; //Spacing between rooms: Y
        
        public static RoomBhv InstantiateRoom(DungeonRoom dungeonRoom, RoomBhv roomPrefab)
        {
            var newRoom = Instantiate(roomPrefab);
            newRoom.roomData = dungeonRoom;
            newRoom.westDoor = null;
            newRoom.eastDoor = null;
            newRoom.northDoor = null;
            newRoom.southDoor = null;

            //Sets room transform position
            newRoom.gameObject.transform.position = 
                new Vector2(RoomSpacingX * dungeonRoom.Coordinates.X, -RoomSpacingY * dungeonRoom.Coordinates.Y);
            return newRoom;
        }
        

        

    }
}