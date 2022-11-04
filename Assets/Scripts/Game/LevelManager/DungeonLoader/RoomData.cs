using System;
using UnityEngine;

namespace Game.LevelManager.DungeonLoader
{
    public enum TileTypes
    {
        Floor,
        Block
    }
    
    [CreateAssetMenu(fileName = "RoomData", menuName = "RoomGenerator/RoomData")]
    [Serializable]
    public class RoomData : ScriptableObject
    {
        [HideInInspector]
        public string roomDataName = "RoomData";
        [field: SerializeField] public TileRow[] Room { get; set; }
        public Tile this[int x, int y]
        {
            get { return Room[x][y];}
            set { Room[x][y] = value; }
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
            get { return Tiles[index];}
            set { Tiles[index] = value; }
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
        [field: SerializeField] public TileTypes TileType { get; set; }

        public Tile(TileTypes tileType)
        {
            TileType = tileType;
        }
    }

}