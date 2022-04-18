using Game.LevelManager;
using UnityEngine;
using Util;

namespace Game.GameManager.DungeonManager
{
    public class RoomLoader : MonoBehaviour
    {
        public static float roomSpacingX = 30f; //Spacing between rooms: X
        public static float roomSpacingY = 20f; //Spacing between rooms: Y
        public static Transform roomsParent;  //Transform to hold rooms for leaner hierarchy view
        
        public static RoomBhv InstantiateRoom(DungeonRoom dungeonRoom, RoomBhv roomPrefab)
        {
            var newRoom = Instantiate(roomPrefab, roomsParent);
            newRoom.roomData = dungeonRoom;
            Coordinates targetCoordinates;
            newRoom.westDoor = null;
            newRoom.eastDoor = null;
            newRoom.northDoor = null;
            newRoom.southDoor = null;

            //Sets room transform position
            newRoom.gameObject.transform.position = 
                new Vector2(roomSpacingX * dungeonRoom.Coordinates.X, -roomSpacingY * dungeonRoom.Coordinates.Y);
            return newRoom;
        }
        

        

    }
}