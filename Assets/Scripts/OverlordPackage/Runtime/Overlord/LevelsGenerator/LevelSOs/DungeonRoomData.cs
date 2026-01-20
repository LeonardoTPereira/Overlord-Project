using Overlord.NarrativeGenerator.NPCs;
using System;
using System.Collections.Generic;
using Util;

namespace Overlord.LevelGenerator.LevelSOs
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
        public int NumOfNpcs { get; set ; }
        public int TotalEnemies { get; set; }

        public List<NpcSo> Npcs { get; set; }  // Added property to hold NPCs in the room. Obs: done after dungeon generation.

        public DungeonRoomData(int x, int y)
        {
            Coordinates = new Coordinates(x, y);
            Type = null;
            Keys = null;
            Locks = null;
            Treasures = 0;
            NumOfNpcs = 0;
            TotalEnemies = 0;
            IsLeaf = false;
        }

    }
}
