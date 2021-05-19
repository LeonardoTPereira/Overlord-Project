using System;
using System.Collections.Generic;
using System.ComponentModel;

[Serializable]
public class DungeonFile
{
    public Dimensions dimensions;
    public List<Room> rooms;

    public DungeonFile()
    {
        rooms = new List<Room>();
    }
    [Serializable]
    public class Room
    {
        public Coordinates coordinates;
        public string type;
        public List<int> keys;
        public List<int> locks;
        private int enemies = -1;
        private int treasures = -1;

        [DefaultValue(-1)]
        public int Enemies { get => enemies; set => enemies = value; }
        [DefaultValue(-1)]
        public int Treasures { get => treasures; set => treasures = value; }
    }
}
