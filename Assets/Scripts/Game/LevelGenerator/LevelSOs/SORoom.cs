using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.LevelManager;
using UnityEngine;
using Util;

namespace Game.LevelGenerator.LevelSOs
{
    [Serializable]
    public class SORoom
    {
        public Coordinates coordinates;
        public string type;
        public List<int> keys;
        public List<int> locks;

        private int treasures;
        private int npcs;
        private int totalEnemies;

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
            treasures = 0;
            npcs = 0;
            totalEnemies = 0;
        }

        public int Treasures { get => treasures; set => treasures = value; }
        public int Npcs { get => npcs; set => npcs = value; }
    }
}
