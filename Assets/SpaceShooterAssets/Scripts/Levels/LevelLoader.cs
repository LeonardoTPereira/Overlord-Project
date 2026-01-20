using Overlord.LevelGenerator.LevelSOs;
using Overlord.NarrativeGenerator.NPCs;
using Overlord.NarrativeGenerator.Quests;
using System.Collections.Generic;
using UnityEngine;
using Util;
using static Util.Constants;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private Transform _roomRoot;
    [SerializeField] private Transform _player;

    private Dictionary<Vector2Int, DungeonRoomData> _dungeonMap;
    private RoomController _currentRoom;
    private QuestLineList _currentQuestlineList;

    void Awake() => Instance = this;

    public void Load(QuestLineList questLineList, DungeonFileSo dungeon)
    {
        _currentQuestlineList = questLineList;

        questLineList.ConvertDataForCurrentDungeon(dungeon.Parts);

        _dungeonMap = new();
        foreach (var room in dungeon.Parts)
            _dungeonMap[new Vector2Int(room.Coordinates.X, room.Coordinates.Y)] = room;

        ResetNpcCoordinates();

        var startRoom = dungeon.Parts.Find(r => r.Type == RoomTypeString.Start);
        MoveToRoom(startRoom);
    }

    public void MoveToRoom(DungeonRoomData data)
    {
        if (_currentRoom != null)
            Destroy(_currentRoom.gameObject);

        var roomGO = Instantiate(_roomPrefab, _roomRoot);
        _currentRoom = roomGO.GetComponent<RoomController>();
        _currentRoom.Init(_currentQuestlineList, data);

        _player.position = _currentRoom.GetSpawnPosition().position;
        _currentRoom.OnPlayerEnter();
    }

    public List<DungeonRoomData> GetNeighbors(string roomType, DungeonRoomData room)
    {
        List<DungeonRoomData> result = new();

        Vector2Int pos = new(room.Coordinates.X, room.Coordinates.Y);
        Vector2Int[] dirs = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };

        foreach (var d in dirs)
            if (_dungeonMap.TryGetValue(pos + d, out var neighbor))
                if (neighbor.Type.Contains(roomType) == true)
                    result.Add(neighbor);

        return result;
    }

    public DungeonRoomData GetRoomBeyondCorridor(DungeonRoomData fromRoom, DungeonRoomData corridor)
    {
        Vector2Int fromPos = new(fromRoom.Coordinates.X, fromRoom.Coordinates.Y);
        Vector2Int corridorPos = new(corridor.Coordinates.X, corridor.Coordinates.Y);

        Vector2Int dir = corridorPos - fromPos;
        Vector2Int targetPos = corridorPos + dir;

        if (_dungeonMap.TryGetValue(targetPos, out var targetRoom))
            return targetRoom;

        return null;
    }

    private void ResetNpcCoordinates()
    {
        foreach (NpcSo npc in _currentQuestlineList.NpcSos)
        {
            npc.RoomCoordinates = new Coordinates(-1, -1);
        }
    }
}
