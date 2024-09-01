using System;
using Game.LevelManager.DungeonLoader;

namespace Game.Events
{
    public delegate void StartMapEvent(object sender, StartMapEventArgs e);
    public class StartMapEventArgs : EventArgs
    {
        public string MapName { get; }
        public Map Map { get; }
        public int TotalTreasure { get; private set; }

        public StartMapEventArgs(string mapName, Map map, int totalTreasure)
        {
            MapName = mapName;
            Map = map;
            TotalTreasure = totalTreasure;
        }
    }
}