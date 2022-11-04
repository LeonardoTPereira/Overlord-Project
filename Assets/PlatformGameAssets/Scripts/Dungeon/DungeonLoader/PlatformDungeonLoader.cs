using System.Collections;
using System.Collections.Generic;
using Game.LevelGenerator.LevelSOs;
using UnityEngine;

namespace PlatformGame.Dungeon.DungeonLoader
{
    public class PlatformDungeonLoader : Game.LevelManager.DungeonLoader.DungeonLoader
    {
        protected override void LoadDungeon(DungeonFileSo dungeonFileSo)
        {
            _dungeonMap = new PlatformMap(dungeonFileSo, null);
        }
    }    
}

