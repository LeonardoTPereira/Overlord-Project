using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

public static class DungeonRoomStateManager
{
    public static HashSet<int> CollectedKeys = new();
    public static HashSet<string> VisitedRooms = new();

    public static int TotalEnemiesInLevel;
    public static int TotalCollectiblesInLevel;
    public static int TotalRoomsInLevel;

    public static Dictionary<Coordinates, RoomProgressData> roomStates =
    new Dictionary<Coordinates, RoomProgressData>();

    public static void Init(int totalEnemiesInLevel, int totalCollectiblesInLevel, int totalRoomsInLevel)
    {
        TotalEnemiesInLevel = totalEnemiesInLevel;
        TotalCollectiblesInLevel = totalCollectiblesInLevel;
        TotalRoomsInLevel = totalRoomsInLevel;
    }

    public static bool HasKey(int keyId)
    {
        return CollectedKeys.Contains(keyId);
    }

    public static RoomProgressData EnterRoom(
    Coordinates coords,
    int initialEnemies,
    int initialCollectibles,
    int initialTools,
    int initialLore)
    {
        if (!roomStates.TryGetValue(coords, out var data))
        {
            // Primeira vez na sala
            data = new RoomProgressData(
                initialEnemies,
                initialCollectibles,
                initialTools,
                initialLore);

            roomStates.Add(coords, data);
        }
        else
        {
        }

        return data;
    }

    public static void UpdateRoomState(Coordinates coords, RoomProgressData updatedRoomProgress)
    {
        if (!roomStates.TryGetValue(coords, out var data))
        {
            Debug.LogError($"Tentando atualizar sala inexistente {coords}");
            return;
        }

        data.RemainingEnemies = updatedRoomProgress.RemainingEnemies;
        data.RemainingCollectibles = updatedRoomProgress.RemainingCollectibles;
        data.RemainingTools = updatedRoomProgress.RemainingTools;
        data.RemainingLoreItems = updatedRoomProgress.RemainingTools;

        // Segurança
        data.RemainingEnemies = Mathf.Max(0, data.RemainingEnemies);
        data.RemainingCollectibles = Mathf.Max(0, data.RemainingCollectibles);
        data.RemainingTools = Mathf.Max(0, data.RemainingTools);
        data.RemainingLoreItems = Mathf.Max(0, data.RemainingLoreItems);
    }

    public static bool HasRoom(Coordinates coords)
    {
        return roomStates.ContainsKey(coords);
    }
}