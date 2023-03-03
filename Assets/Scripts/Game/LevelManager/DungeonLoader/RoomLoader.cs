using Game.ExperimentControllers;
using Game.LevelManager.DungeonManager;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonLoader
{
    public class RoomLoader : MonoBehaviour
    {
        public static RoomBhv InstantiateRoom(DungeonRoom dungeonRoom, RoomBhv roomPrefab, Enums.GameType gameType)
        {
	        float roomSpacingX;
	        float roomSpacingY;
	        if (gameType == Enums.GameType.Platformer)
	        {
		        roomSpacingX = 65f;
		        roomSpacingY = 65f;
	        }
	        else
	        {
		        roomSpacingX = 30f;
		        roomSpacingY = 20f;
	        }
            var roomPosition = new Vector2(roomSpacingX * dungeonRoom.Coordinates.X, -roomSpacingY * dungeonRoom.Coordinates.Y);
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