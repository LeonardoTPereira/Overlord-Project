using System.Collections;
using System.Collections.Generic;
using Game.LevelGenerator.LevelSOs;
using Game.LevelManager;
using Game.LevelManager.DungeonLoader;
using UnityEngine;
using Util;

namespace PlatformGame.Dungeon.DungeonLoader
{
    public class PlatformMap : Map
    {
        public PlatformMap(DungeonFileSo dungeonFileSo, string roomsFilePath = null, int mode = 0) : base(dungeonFileSo, roomsFilePath, mode){}
        
        
        protected override void BuildDefaultRooms()
        {
            Dimensions roomDimensions = new Dimensions(Constants.DefaultRoomSizeX, Constants.DefaultRoomSizeY);
            foreach (DungeonPart currentPart in DungeonPartByCoordinates.Values)
            {
                if (currentPart is DungeonRoom room)
                {
                    var roomInput = ScriptableObject.CreateInstance<RoomGeneratorInput>();
                    var doorList = CreateDoorList(room.Coordinates);
                    roomInput.Init(roomDimensions, doorList[0], doorList[1], doorList[2], doorList[3]);

                   ((PlatformDungeonRoom)room).CreateRoom(roomDimensions);
                }
            }
        }
        
    }
    
}
