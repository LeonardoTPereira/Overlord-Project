using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelManager.DungeonLoader
{
    public class MatrixMap
    {
        private int[,] matrixMap; //0 for floor, 1 for wall, 2 for items, 3 for outer wall, 4 for door
        private int columns;
        private int rows;

        public MatrixMap(int columns, int rows)
        {
            this.rows = rows;
            this.columns = columns;
            matrixMap = new int[rows + 2, columns + 2];
        }

        public MatrixMap(MatrixMap original)
        {
            rows = original.rows;
            columns = original.columns;
            matrixMap = new int[rows + 2, columns + 2];
            for (int i = -1; i < rows + 1; i++)
            {
                for (int j = -1; j < columns + 1; j++)
                {
                    this[i, j] = original[i, j];
                }
            }
        }

        public int this[int indexY, int indexX]
        {
            get
            {
                return matrixMap[indexY + 1, indexX + 1];
            }
            set
            {
                matrixMap[indexY + 1, indexX + 1] = value;
            }
        }        
    }
}