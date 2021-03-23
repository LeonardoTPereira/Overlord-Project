using LevelGenerator;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPartFactory
{
    int partType;

    public static DungeonPart CreateDungeonPartFromFile(MapFileHandler mapFileHandler)
    {
        Coordinates coordinates = mapFileHandler.GetNextDungeonPartCoordinates();
        string dungeonPartCode = mapFileHandler.GetNextDungeonPartCode();
        if (dungeonPartCode == DungeonPart.Type.CORRIDOR)
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
        catch (FormatException)
        { }

        List<int> keyIDs;
        int difficulty, treasure;
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
        return new DungeonRoom(coordinates: coordinates, code: dungeonPartCode, keyIDs: keyIDs, difficulty: difficulty, treasure: treasure);
    }

    public static DungeonPart CreateDungeonRoomFromEARoom(Coordinates coordinates, string partCode, List<int> keyIDs, int difficulty, int treasure)
    {
        return new DungeonRoom(coordinates, partCode, keyIDs, difficulty, treasure);
    }

    public static DungeonPart CreateDungeonCorridorFromEACorridor(Coordinates coordinates, string partCode, List<int> lockIDs)
    {
        if (partCode == DungeonPart.Type.CORRIDOR)
            return new DungeonCorridor(coordinates, partCode);
        return new DungeonLockedCorridor(coordinates, lockIDs);
    }
}
