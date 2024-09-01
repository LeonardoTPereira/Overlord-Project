using System.Collections.Generic;
using Game.LevelGenerator.LevelSOs;
using Util;
using PlatformGame.Dungeon;

namespace Game.LevelManager.DungeonLoader
{
    public static class DungeonPartFactory
    {
       

        public static DungeonPart CreateDungeonRoomFromEARoom(Coordinates coordinates, string partCode, List<int> keyIDs, int treasure, int totalEnemies, int items, int npcs)
        {
            return new DungeonRoom(coordinates, partCode, keyIDs, treasure, totalEnemies, npcs);
        }

        public static DungeonPart CreateDungeonCorridorFromEACorridor(Coordinates coordinates, string partCode, List<int> lockIDs)
        {
            return partCode == Constants.RoomTypeString.Corridor ? new DungeonCorridor(coordinates, partCode) : new DungeonLockedCorridor(coordinates, lockIDs);
        }

        public static DungeonPart CreateDungeonPartFromDungeonFileSO(DungeonRoomData dungeonRoomData, Enums.GameType gameType)
        {
            if (dungeonRoomData.Type?.Equals(Constants.RoomTypeString.Corridor) ?? false)
                return new DungeonCorridor(dungeonRoomData.Coordinates, dungeonRoomData.Type);
            if ((dungeonRoomData.Locks?.Count ?? 0) == 0)
            {

                switch (gameType)
                {
                    case Enums.GameType.TopDown:
                       return new DungeonRoom(dungeonRoomData.Coordinates, dungeonRoomData.Type,
                            dungeonRoomData.Keys ?? new List<int>(),
                            dungeonRoomData.Treasures, dungeonRoomData.TotalEnemies, dungeonRoomData.Npcs);

                    case Enums.GameType.Platformer:
                        return new PlatformDungeonRoom(dungeonRoomData.Coordinates, dungeonRoomData.Type,
                            dungeonRoomData.Keys ?? new List<int>(),
                            dungeonRoomData.Treasures, dungeonRoomData.TotalEnemies, dungeonRoomData.Npcs);

                }
            }

            for (var i = 0; i < dungeonRoomData.Locks.Count; ++i)
            {
                if (IsLegacyLock(dungeonRoomData.Locks[i]))
                {
                    dungeonRoomData.Locks[i] = -dungeonRoomData.Locks[i];
                }
            }
            return new DungeonLockedCorridor(dungeonRoomData.Coordinates, dungeonRoomData.Locks);
        }

        private static bool IsLegacyLock(int lockId)
        {
            return lockId < 0;
        }
    }
}