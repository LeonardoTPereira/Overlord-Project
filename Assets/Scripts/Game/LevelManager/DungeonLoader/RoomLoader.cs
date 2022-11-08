using Game.ExperimentControllers;
using Game.LevelManager.DungeonManager;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonLoader
{
    public class RoomLoader : MonoBehaviour
    {
        private static float roomSpacingX; //Spacing between rooms: X
        private static float roomSpacingY; //Spacing between rooms: Y

        [SerializeField] public GeneratorSettings settings;
        
        public void Start()
        {
            switch (settings.GameType)
            {
                case Enums.GameType.TopDown:
                    roomSpacingX = 30f;
                    roomSpacingY = 20f;
                    break;
                case Enums.GameType.Platformer:
                    roomSpacingX = 65f;
                    roomSpacingY = 65f;
                    break;
            }
        }
        
        public static RoomBhv InstantiateRoom(DungeonRoom dungeonRoom, RoomBhv roomPrefab)
        {
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