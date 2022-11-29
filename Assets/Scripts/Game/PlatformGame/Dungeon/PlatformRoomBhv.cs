using System.Collections;
using System.Collections.Generic;
using Game.LevelManager.DungeonManager;
using PlatformGame.Enemy;
using UnityEngine;
using Util;

namespace PlatformGame.Dungeon
{
    public class PlatformRoomBhv : RoomBhv
    {
        private bool[,] _backtrackingPath;
        private bool _isSpawnPointsGenerated = false;
        //Change enemy spawners later...
        //Do not forget EnemyKilledHandler event inside SpawnEnemies() to remove from enemy dictionary...

        protected override void RemoveFromDictionaryWhenEnemyDied(GameObject enemy)
        {
            enemy.GetComponent<EnemyHealth>().EnemyKilledHandler += RemoveFromDictionary;
        }

/*
        public override void SpawnEnemies()
        {
            while (!_isSpawnPointsGenerated) { }

            base.SpawnEnemies();
        }

        protected override void SetEnemySpawners(float centerX, float centerY)
        {
            _isSpawnPointsGenerated = false;
            // Need to have the roomModel matrix to work
            
            char [,] roomModel = CreateRoomModel();
            _backtrackingPath = new bool[roomModel.GetLength(0), roomModel.GetLength(1)];

            for (int i = 0; i < roomModel.GetLength(0); i++)
                for (int j = 0; j < roomModel.GetLength(1); j++)
                    _backtrackingPath[i, j] = false;
            int startX = 5, startY = 0;

            SpawnBacktracking(startX, startY, roomModel.GetLength(0), roomModel.GetLength(1), centerX, centerY, roomModel);
            _isSpawnPointsGenerated = true;
        }
        
        private char[,] CreateRoomModel()
        {
            char[,] model = new char[24, 28];
            for(int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    switch (roomData.Tiles[i,j].TileType)
                    {
                        case Enums.TileTypes.Block: model[i, j] = '#';
                            break;
                        case Enums.TileTypes.Floor: model[i, j] = ' ';
                            break;
                    }
                }
            }

            return model;
        }
        

        private void SpawnBacktracking(int i, int j, int lineNum, int colNum, float centerX, float centerY, char[,] roomModel)
        {
            if (roomModel[i,j] == '#')
            {
                return;
            }
            // Test if the block bellow is a walking block ( so it is a possible spawn point! )
            if (i + 1 < lineNum && roomModel[i + 1, j] == '#')
            {
                _backtrackingPath[i + 1, j] = true;
                spawnPoints.Add(new Vector3((j - centerX)* transform.lossyScale.x, (centerY - i) * transform.lossyScale.y, 0));
            }
            // Test the default ground block around the map
            else if (i + 1 == lineNum)
            {
                spawnPoints.Add(new Vector3((j - centerX) * transform.lossyScale.x, (centerY - i) * transform.lossyScale.y, 0));
            }

            // go UP
            if (i - 1 > 0 && !_backtrackingPath[i - 1, j])
            {
                _backtrackingPath[i - 1, j] = true;
                SpawnBacktracking(i - 1, j, lineNum, colNum, centerX, centerY, roomModel);
            }
            // go RIGHT
            if (j + 1 < colNum && !_backtrackingPath[i, j + 1])
            {
                _backtrackingPath[i, j + 1] = true;
                SpawnBacktracking(i, j + 1, lineNum, colNum, centerX, centerY, roomModel);
            }
            // go DOWN
            if (i + 1 < lineNum && !_backtrackingPath[i + 1, j])
            {
                _backtrackingPath[i + 1, j] = true;
                SpawnBacktracking(i + 1, j, lineNum, colNum, centerX, centerY, roomModel);
            }
            // go LEFT
            if (j - 1 > 0 && !_backtrackingPath[i, j - 1])
            {
                _backtrackingPath[i, j - 1] = true;
                SpawnBacktracking(i, j - 1, lineNum, colNum, centerX, centerY, roomModel);
            }

        }
    */
    }
    
}