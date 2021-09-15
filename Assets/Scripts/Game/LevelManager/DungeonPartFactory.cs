using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelManager
{
    public class DungeonPartFactory
    {
        protected DungeonPartFactory() { }

        public static DungeonPart CreateDungeonPartFromFile(MapFileHandler mapFileHandler)
        {
            Coordinates coordinates = mapFileHandler.GetNextDungeonPartCoordinates();
            string dungeonPartCode = mapFileHandler.GetNextDungeonPartCode();
            if (dungeonPartCode == DungeonPart.PartType.CORRIDOR)
            {
#if UNITY_EDITOR
                Debug.Log($"Code is a Corridor: {dungeonPartCode}");
#endif
                return new DungeonCorridor(coordinates, dungeonPartCode);
            }
            try
            {
                if (int.Parse(dungeonPartCode) < 0)
                {
                    List<int> lockIDs;
                    lockIDs = mapFileHandler.GetNextLockIDs();
#if UNITY_EDITOR
                    Debug.Log($"We have a locked corridor. Lock IDs:");
                    foreach (int lockID in lockIDs)
                    {
                        Debug.Log($"Lock: {lockID}");
                    }
#endif
                    return new DungeonLockedCorridor(coordinates, lockIDs);
                }
            }
            catch (FormatException e)
            {
                Debug.Log("Format Exception when parsing dungeon room code");
                Debug.LogException(e);
            }

            List<int> keyIDs;
            int difficulty, treasure, enemyType, npc;
#if UNITY_EDITOR
            Debug.Log($"We have a room:");
#endif
            if (mapFileHandler.IsStart(dungeonPartCode) || mapFileHandler.IsFinal(dungeonPartCode))
                difficulty = mapFileHandler.GetNextDifficulty();
            else
                difficulty = int.Parse(dungeonPartCode);
#if UNITY_EDITOR
            Debug.Log($"Difficulty: {difficulty}");
#endif
            treasure = mapFileHandler.GetNextTreasure();
            enemyType = mapFileHandler.GetNextEnemyType();
            npc = mapFileHandler.GetNextNpc();
#if UNITY_EDITOR
            Debug.Log($"Treasure: {treasure}");
#endif
            keyIDs = mapFileHandler.GetNextKeyIDs();
#if UNITY_EDITOR
            Debug.Log($"Do we have keys in the room? Key Count: {keyIDs.Count}");
            foreach (int keyID in keyIDs)
            {
                Debug.Log($"Key: {keyID}");
            }
#endif
            return new DungeonRoom(coordinates: coordinates, code: dungeonPartCode, keyIDs: keyIDs, difficulty: difficulty, treasure: treasure, enemyType: enemyType, npc: npc);
        }

        public static DungeonPart CreateDungeonRoomFromEARoom(Coordinates coordinates, string partCode, List<int> keyIDs, int difficulty, int treasure, int enemyType, int items, int npcs)
        {
            return new DungeonRoom(coordinates, partCode, keyIDs, difficulty, treasure, enemyType, npcs);
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
            if (dungeonRoom.locks.Count > 0)
            {
                for (int i = 0; i < dungeonRoom.locks.Count; ++i)
                    dungeonRoom.locks[i] = -dungeonRoom.locks[i];
                return new DungeonLockedCorridor(dungeonRoom.coordinates, dungeonRoom.locks);
            }
            return new DungeonRoom(dungeonRoom.coordinates, dungeonRoom.type, dungeonRoom?.keys ?? new List<int>(), dungeonRoom.Enemies, dungeonRoom.Treasures, dungeonRoom.EnemiesType, dungeonRoom.Npcs);
        }
    }
}