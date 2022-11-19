using Game.LevelManager.DungeonManager;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonLoader
{
    public class RoomLoader : MonoBehaviour
    {
        private static readonly float RoomSpacingX = 30f; //Spacing between rooms: X
        private static readonly float RoomSpacingY = 20f; //Spacing between rooms: Y
        
        public static RoomBhv InstantiateRoom(DungeonRoom dungeonRoom, RoomBhv roomPrefab)
        {
            var roomPosition = new Vector2(RoomSpacingX * dungeonRoom.Coordinates.X, -RoomSpacingY * dungeonRoom.Coordinates.Y);
            var newRoom = Instantiate(roomPrefab, roomPosition, roomPrefab.transform.rotation);
            newRoom.roomData = dungeonRoom;
            newRoom.westDoor = null;
            newRoom.eastDoor = null;
            newRoom.northDoor = null;
            newRoom.southDoor = null;
            return newRoom;
        }
    }
}