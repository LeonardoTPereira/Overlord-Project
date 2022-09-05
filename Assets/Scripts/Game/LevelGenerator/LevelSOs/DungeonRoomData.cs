using System;
using System.Collections.Generic;
using Util;

namespace Game.LevelGenerator.LevelSOs
{
    [Serializable]
    public class DungeonRoomData
    {
        public Coordinates Coordinates { get; set; }
        public string Type {get; set; }
        public List<int> Keys { get; set; }
        public List<int> Locks { get; set; }
        public bool IsLeaf { get; set; }

        
        public int Treasures { get; set; }
        public int Npcs { get; set ; }
        public int TotalEnemies { get; set; }

        public DungeonRoomData(int x, int y)
        {
            Coordinates = new Coordinates(x, y);
            Type = null;
            Keys = null;
            Locks = null;
            Treasures = 0;
            Npcs = 0;
            TotalEnemies = 0;
            IsLeaf = false;
        }

    }
}
