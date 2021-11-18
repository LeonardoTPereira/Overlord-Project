using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Game.LevelManager
{
    public static class DungeonPartFactory
    {
       

        public static DungeonPart CreateDungeonRoomFromEARoom(Coordinates coordinates, string partCode, List<int> keyIDs, int treasure, int totalEnemies, int items, int npcs)
        {
            return new DungeonRoom(coordinates, partCode, keyIDs, treasure, totalEnemies, npcs);
        }

        public static DungeonPart CreateDungeonCorridorFromEACorridor(Coordinates coordinates, string partCode, List<int> lockIDs)
        {
            if (partCode == DungeonPart.PartType.CORRIDOR)
                return new DungeonCorridor(coordinates, partCode);
            return new DungeonLockedCorridor(coordinates, lockIDs);
        }

        public static DungeonPart CreateDungeonPartFromDungeonFileSO(SORoom dungeonRoom)
        {
            if (dungeonRoom.type?.Equals("c") ?? false)
                return new DungeonCorridor(dungeonRoom.coordinates, dungeonRoom.type);
            if (dungeonRoom.locks.Count <= 0)
                return new DungeonRoom(dungeonRoom.coordinates, dungeonRoom.type, dungeonRoom.keys ?? new List<int>(),
                    dungeonRoom.Treasures, dungeonRoom.TotalEnemies, dungeonRoom.Npcs);
            for (var i = 0; i < dungeonRoom.locks.Count; ++i)
            {
                if (IsLegacyLock(dungeonRoom.locks[i]))
                {
                    dungeonRoom.locks[i] = -dungeonRoom.locks[i];
                }
            }
            return new DungeonLockedCorridor(dungeonRoom.coordinates, dungeonRoom.locks);
        }

        private static bool IsLegacyLock(int lockId)
        {
            return lockId < 0;
        }
    }
}