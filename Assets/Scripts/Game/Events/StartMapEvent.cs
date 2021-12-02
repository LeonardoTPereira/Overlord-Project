using Game.LevelManager;
using System;

public delegate void StartMapEvent(object sender, StartMapEventArgs e);
public class StartMapEventArgs : EventArgs
{
    private string mapName;
    private int mapBatch;
    private Map map;
    private int playerProjectileIndex;
    private int _totalTreasure;


    public StartMapEventArgs(string mapName, int mapBatch, Map map, int playerProjectileIndex, int totalTreasure)
    {
        MapName = mapName;
        MapBatch = mapBatch;
        Map = map;
        PlayerProjectileIndex = playerProjectileIndex;
        TotalTreasure = totalTreasure;
    }

    public string MapName { get => mapName; set => mapName = value; }
    public int MapBatch { get => mapBatch; set => mapBatch = value; }
    public Map Map { get => map; set => map = value; }
    public int PlayerProjectileIndex { get => playerProjectileIndex; set => playerProjectileIndex = value; }
    public int TotalTreasure { get => _totalTreasure; set => _totalTreasure = value; }
}
