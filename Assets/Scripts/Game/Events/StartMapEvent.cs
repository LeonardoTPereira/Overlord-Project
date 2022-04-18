using System;
using Game.LevelManager;

namespace Game.Events
{
    public delegate void StartMapEvent(object sender, StartMapEventArgs e);
    public class StartMapEventArgs : EventArgs
    {
        private string mapName;
        private Map map;


        public StartMapEventArgs(string mapName, Map map)
        {
            MapName = mapName;
            Map = map;
        }

        public string MapName { get => mapName; set => mapName = value; }
        public Map Map { get => map; set => map = value; }
    }
}