using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelGenerator
{
    public class RoomGrid
    {
        //Also will be used to show the resulting dungeon
        private Room[,] roomGrid;
        //Indexer for the matrix in this class
        public Room this[int X, int Y]
        {
            get => roomGrid[X + Constants.MATRIXOFFSET, Y + Constants.MATRIXOFFSET];
            set => roomGrid[X + Constants.MATRIXOFFSET, Y + Constants.MATRIXOFFSET] = value;
        }

        public RoomGrid()
        {
            roomGrid = new Room[Constants.MATRIXOFFSET*2, Constants.MATRIXOFFSET*2];
        }
    }
}
