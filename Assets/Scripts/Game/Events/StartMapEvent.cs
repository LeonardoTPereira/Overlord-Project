using Game.LevelManager;
using System;

public delegate void StartMapEvent(object sender, StartMapEventArgs e);
public class StartMapEventArgs : EventArgs
{
    private string mapName;
    private int mapBatch;
    private Map map;
    private int playerProjectileIndex;

    public StartMapEventArgs(string mapName, int mapBatch, Map map, int playerProjectileIndex)
    {
        MapName = mapName;
        MapBatch = mapBatch;
        Map = map;
        PlayerProjectileIndex = playerProjectileIndex;
    }

    public string MapName { get => mapName; set => mapName = value; }
    public int MapBatch { get => mapBatch; set => mapBatch = value; }
    public Map Map { get => map; set => map = value; }
    public int PlayerProjectileIndex { get => playerProjectileIndex; set => playerProjectileIndex = value; }
}
