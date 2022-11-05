using System.Collections;
using System.Collections.Generic;
using Game.ExperimentControllers;
using Game.LevelManager.DungeonLoader;
using PlatformGame.Dungeon;
using UnityEngine;
using Util;

namespace PlatformGame.Arena
{
    public class PlatformArenaController : ArenaController
    {
        protected override DungeonRoom InstantiateDungeonRoom()
        {
            return new PlatformDungeonRoom(new Coordinates(0, 0), Constants.RoomTypeString.Normal, new List<int>(), 0, TotalEnemies, 0);
        }

        protected override void LoadGameUI()
        {
            return;
        }
    }
}