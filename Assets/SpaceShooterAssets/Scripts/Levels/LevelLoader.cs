using Overlord.LevelGenerator.LevelSOs;
using Overlord.NarrativeGenerator.Quests;
using System.Collections.Generic;
using UnityEngine;
using static Util.Constants;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Transform roomRoot;
    [SerializeField] private Transform player;

    private Dictionary<Vector2Int, DungeonRoomData> dungeonMap;
    private RoomController currentRoom;

    void Awake() => Instance = this;

    public void Load(QuestLineList questLineList, DungeonFileSo dungeon)
    {
        questLineList.ConvertDataForCurrentDungeon(dungeon.Parts);

        dungeonMap = new();
        foreach (var room in dungeon.Parts)
            dungeonMap[new Vector2Int(room.Coordinates.X, room.Coordinates.Y)] = room;

        var startRoom = dungeon.Parts.Find(r => r.Type == RoomTypeString.Start);
        MoveToRoom(startRoom);
    }

    public void MoveToRoom(DungeonRoomData data)
    {
        if (currentRoom != null)
            Destroy(currentRoom.gameObject);

        var roomGO = Instantiate(roomPrefab, roomRoot);
        currentRoom = roomGO.GetComponent<RoomController>();
        currentRoom.Init(data);

        player.position = currentRoom.GetSpawnPosition().position;
        currentRoom.OnPlayerEnter();
    }

    public List<DungeonRoomData> GetNeighbors(DungeonRoomData room)
    {
        List<DungeonRoomData> result = new();

        Vector2Int pos = new(room.Coordinates.X, room.Coordinates.Y);
        Vector2Int[] dirs = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };

        foreach (var d in dirs)
            if (dungeonMap.TryGetValue(pos + d, out var neighbor))
                if (neighbor.Type.Contains("Corridor") == false)
                    result.Add(neighbor);

        return result;
    }
}
