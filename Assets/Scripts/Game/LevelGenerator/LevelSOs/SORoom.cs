using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.LevelManager;

namespace Game.LevelGenerator.LevelSOs
{
    [Serializable]
    public class SORoom
    {
        public Coordinates coordinates;
        public string type;
        public List<int> keys;
        public List<int> locks;

        public int treasures = -1;
        public int npcs = -1;
        public int totalEnemies;

        public int TotalEnemies
        {
            get => totalEnemies;
            set => totalEnemies = value;
        }

        public SORoom(int x, int y)
        {
            coordinates = new Coordinates(x, y);
            type = null;
            keys = null;
            locks = null;
            treasures = -1;
            npcs = -1;
            totalEnemies = 0;
        }

        [DefaultValue(-1)]
        public int Treasures { get => treasures; set => treasures = value; }
        [DefaultValue(-1)]
        public int Npcs { get => npcs; set => npcs = value; }
    }
}
