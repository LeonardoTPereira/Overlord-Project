using System.Collections.Generic;

public static class DungeonRuntimeData
{
    public static HashSet<int> CollectedKeys = new();
    public static HashSet<string> VisitedRooms = new();

    public static bool HasKey(int keyId)
    {
        return CollectedKeys.Contains(keyId);
    }
}