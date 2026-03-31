using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Overlord.LevelGenerator.LevelSOs;

public class MinimapController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform mapRoot;
    [SerializeField] private RectTransform gridContainer;
    [SerializeField] private MinimapTile tilePrefab;

    [SerializeField] private float tileSize = 16f;

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
        gridContainer.DetachChildren();

        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var part in dungeon.Parts)
        {
            minX = Mathf.Min(minX, part.Coordinates.X);
            maxX = Mathf.Max(maxX, part.Coordinates.X);
            minY = Mathf.Min(minY, part.Coordinates.Y);
            maxY = Mathf.Max(maxY, part.Coordinates.Y);
        }

        currentRoom = new Vector2Int(minX, minY);

        float width = (maxX - minX + 1) * tileSize;
        float height = (maxY - minY + 1) * tileSize;

        gridContainer.sizeDelta = new Vector2(width, height);

        foreach (var part in dungeon.Parts)
        {
            var coord = new Vector2Int(part.Coordinates.X, part.Coordinates.Y);
            var tile = Instantiate(tilePrefab, gridContainer);
            tile.Init(part, icons);

            tile.Rect.sizeDelta = Vector2.one * tileSize;

            Vector2 localPos = new Vector2(
                (coord.x - currentRoom.x) * tileSize,
                (coord.y - currentRoom.y) * tileSize
            );

            tile.Rect.anchoredPosition = localPos;
            tile.SetVisible(false);

            tiles[coord] = tile;
        }

        gridContainer.anchoredPosition = -gridContainer.sizeDelta * 0.5f;
    }

    public void RevealRoom(DungeonRoomData room)
    {
        var pos = new Vector2Int(room.Coordinates.X, room.Coordinates.Y);

        if (tiles.TryGetValue(pos, out var tile))
        {
            tile.SetVisible(true);
            DungeonRoomStateManager.VisitedRooms.Add(pos.ToString());
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
