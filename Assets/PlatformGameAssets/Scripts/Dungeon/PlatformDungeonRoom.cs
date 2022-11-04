using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Game.LevelManager;
using Game.LevelManager.DungeonLoader;
using UnityEngine;
using Util;

namespace PlatformGame.Dungeon
{
    public class PlatformDungeonRoom : DungeonRoom
    {
        public PlatformDungeonRoom(Coordinates coordinates, string code, List<int> keyIDs, int treasure, int totalEnemies, int npc) : base(coordinates, code, keyIDs, treasure, totalEnemies, npc) {} 
        
        public override void CreateRoom(Dimensions roomDimensions, RoomGeneratorInput roomGeneratorInput = null)
        {
            Dimensions = roomDimensions;
            InitializeTiles(); // aloca memória para os tiles
            int roomType = RandomSingleton.GetInstance().Random.Next((int)Enums.RoomPatterns.COUNT);
            if (roomGeneratorInput == null)
            {
                PlatformDefaultRoomCreator.CreateRoomOfType(this, roomType);
            }

        }


        
        
        
    }
}