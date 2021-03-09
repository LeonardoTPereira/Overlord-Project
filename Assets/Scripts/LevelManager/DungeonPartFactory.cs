using LevelGenerator;
using System.Collections.Generic;

public class DungeonPartFactory
{
    int partType;

    public static DungeonPart CreateDungeonPartFromFile(MapFileHandler mapFileHandler)
    {
        Coordinates coordinates = mapFileHandler.GetNextDungeonPartCoordinates();
        string dungeonPartCode = mapFileHandler.GetNextDungeonPartCode();
        if (dungeonPartCode == DungeonPart.Type.CORRIDOR)
            return new DungeonCorridor(coordinates, dungeonPartCode);
        else if (int.Parse(dungeonPartCode) < 0)
        {
            List<int> lockIDs;
            lockIDs = mapFileHandler.GetNextLockIDs();
            return new DungeonLockedCorridor(coordinates, lockIDs);
        }
        else
        {
            List<int> keyIDs;
            int difficulty, treasure;
            difficulty = mapFileHandler.GetNextDifficulty();
            treasure = mapFileHandler.GetNextTreasure();
            keyIDs = mapFileHandler.GetNextKeyIDs();
            return new DungeonRoom(coordinates: coordinates, code: dungeonPartCode, keyIDs: keyIDs, difficulty: difficulty, treasure: treasure);
        }
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
