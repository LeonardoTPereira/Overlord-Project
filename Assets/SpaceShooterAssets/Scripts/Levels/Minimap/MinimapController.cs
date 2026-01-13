using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Overlord.LevelGenerator.LevelSOs;

public class MinimapController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform mapRoot;
    [SerializeField] private MinimapTile tilePrefab;
    [SerializeField] private float tileSize = 20f;

    [Header("Icons")]
    [SerializeField] private MinimapIcons icons;

    private Dictionary<Vector2Int, MinimapTile> tiles = new();
    private Vector2Int currentRoom;

    public static MinimapController Instance;

    void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public void Build(DungeonFileSo dungeon)
    {
        tiles.Clear();
        mapRoot.DetachChildren();

        foreach (var part in dungeon.Parts)
        {
            var coord = new Vector2Int(part.Coordinates.X, part.Coordinates.Y);
            var multipler = new Vector2(tileSize, tileSize);
            var tile = Instantiate(tilePrefab, mapRoot);

            tile.Init(part, icons);
            tile.Rect.anchoredPosition = coord * multipler;

            tile.SetVisible(false);
            tiles[coord] = tile;
        }
    }

    public void RevealRoom(DungeonRoomData room)
    {
        var pos = new Vector2Int(room.Coordinates.X, room.Coordinates.Y);

        if (tiles.TryGetValue(pos, out var tile))
        {
            tile.SetVisible(true);
            DungeonRuntimeData.VisitedRooms.Add(pos.ToString());
        }
    }

    public void SetCurrentRoom(DungeonRoomData room)
    {
        if (tiles.TryGetValue(currentRoom, out var oldTile))
            oldTile.SetCurrent(false);

        currentRoom = new Vector2Int(room.Coordinates.X, room.Coordinates.Y);

        if (tiles.TryGetValue(currentRoom, out var newTile))
        {
            newTile.SetCurrent(true);
            newTile.SetVisible(true);
        }
    }
}
