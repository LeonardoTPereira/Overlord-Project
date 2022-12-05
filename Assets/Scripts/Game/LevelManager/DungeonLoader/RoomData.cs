using System;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonLoader
{
   
    [CreateAssetMenu(fileName = "RoomData", menuName = "RoomGenerator/RoomData")]
    [Serializable]
    public class RoomData : ScriptableObject
    {
        [HideInInspector]
        public string roomDataName = "RoomData";
        [field: SerializeField] public TileRow[] Room { get; set; }
        public Tile this[int x, int y]
        {
            get => Room[x][y];
            set => Room[x][y] = value;
        }

        public void Init(int rows, int cols)
        {
            Room = new TileRow[rows];
            for(var i = 0; i < rows; ++i)
            {
                Room[i] = new TileRow(cols);
            }
        }
    }

    [Serializable]
    public class TileRow
    {
        [HideInInspector]
        public string tileRowName = "TileRow";
        [field: SerializeField] public Tile[] Tiles { get; set; }
        public Tile this[int index]
        {
            get => Tiles[index];
            set => Tiles[index] = value;
        }

        public TileRow(int cols)
        {
            Tiles = new Tile[cols];
        }
    }
    
    [Serializable]
    public class Tile
    {
        [HideInInspector]
        public string tileName = "Tile";
        [field: SerializeField] public Enums.TileTypes TileType { get; set; }
        public bool HasBeenVisited { get; set; }
        public Vector2 Position { get; set; }

        public Tile(Enums.TileTypes tileType, Vector2 position)
        {
            TileType = tileType;
            HasBeenVisited = false;
            Position = position;
        }
    }

}