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
        [Header("Plataform Game Related")]
        public CompositeCollider2D colRoomConfiner;

        private bool[,] _backtrackingPath;
        private bool _isSpawnPointsGenerated = false;
        //Change enemy spawners later...
        //Do not forget EnemyKilledHandler event inside SpawnEnemies() to remove from enemy dictionary...

        protected override void RemoveFromDictionaryWhenEnemyDied(GameObject enemy)
        {
            enemy.GetComponent<EnemyHealth>().EnemyKilledHandler += RemoveFromDictionary;
        }
        
        protected override void SetSpritesTheme()
        {
            doorEast.SetTheme(0);
            doorWest.SetTheme(0);
            doorNorth.SetTheme(0);
            doorSouth.SetTheme(0);
            _blockTile = blockTiles[0];
            _floorTile = floorTiles[0];
        }

        protected override void InstantiateTileMap()
        {
            base.InstantiateTileMap();

            for (var ix = -1; ix < roomData.Dimensions.Width + 1; ix++)
            {
                blockTilemap.SetTile(new Vector3Int(ix, -1), _blockTile);
                blockTilemap.SetTile(new Vector3Int(ix, roomData.Dimensions.Height), _blockTile);
            }
            for (var iy = -1; iy < roomData.Dimensions.Height + 1; iy++)
            {
                blockTilemap.SetTile(new Vector3Int(-1, iy), _blockTile);
                blockTilemap.SetTile(new Vector3Int(roomData.Dimensions.Width, iy), _blockTile);
            }
        }

        protected override void GetAvailablePosition()
        {
            while (!_isSpawnPointsGenerated) { }

            int randSpawnpointIndex = Random.Range(0, spawnPoints.Count);
            _availablePosition = spawnPoints[randSpawnpointIndex] + new Vector3(1.0f, 1.0f, 0);
        }

        protected override GameObject PlaceObjectInRoom(GameObject prefab)
        {
            var instance = Instantiate(prefab, transform, true);
            instance.transform.position = _availablePosition;
            return instance;
        }

        protected override void InstantiateCornerProps()
        {
            //Do not instantiate anything
        }
        
        protected override void SetSpritesToWalls()
        {
            //Do nothing
        }
                
        public override void SpawnEnemies()
        {
            while (!_isSpawnPointsGenerated) { }

            base.SpawnEnemies();
        }

        private int startX, startY;

        protected override void SetEnemySpawners()
        {
            var roomPosition = transform.position;
            var xOffset = roomPosition.x;
            var yOffset = roomPosition.y;

            _isSpawnPointsGenerated = false;

            // Need to have the roomModel matrix to work            
            char [,] roomModel = CreateRoomModel();
            _backtrackingPath = new bool[roomModel.GetLength(0), roomModel.GetLength(1)];

            for (int i = 0; i < roomModel.GetLength(0); i++)
                for (int j = 0; j < roomModel.GetLength(1); j++)
                    _backtrackingPath[i, j] = false;

            SetBacktrackingStartPosition(roomModel);

            SpawnBacktracking(startY, startX, roomModel.GetLength(0), roomModel.GetLength(1), xOffset, yOffset, roomModel);
            _isSpawnPointsGenerated = true;
        }

        private void SetBacktrackingStartPosition(char[,] roomModel)
        {
            if (northDoor != null)
            {
                startX = (roomModel.GetLength(1) - 1) / 2;
                startY = (roomModel.GetLength(0) - 1);
            }
            else if (eastDoor != null)
            {
                startX = (roomModel.GetLength(1) - 1);
                startY = (roomModel.GetLength(0) - 1) / 2;
            }
            else if (southDoor != null)
            {
                startX = (roomModel.GetLength(1) - 1) / 2;
                startY = 0;
            }
            else
            {
                startX = 0;
                startY = (roomModel.GetLength(0) - 1) / 2;
            }
        }
                
        private char[,] CreateRoomModel()
        {
            char[,] model = new char[24, 28];
            for(int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    switch (roomData.Tiles[j,i].TileType)
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

        private void SpawnBacktracking(int i, int j, int lineNum, int colNum, float xOffset, float yOffset, char[,] roomModel)
        {
            if (roomModel[i, j] == '#')
            {
                return;
            }
            // Test if the block bellow is a walking block ( so it is a possible spawn point! )
            if (i - 1 > 0 && roomModel[i - 1, j] == '#')
            {
                _backtrackingPath[i + 1, j] = true;
                spawnPoints.Add(new Vector3(xOffset + j * transform.lossyScale.x + 0.5f, yOffset + i * transform.lossyScale.y + 0.5f, 0));
            }
            // Test the default ground block around the map
            if (i - 1 == 0 && roomModel[i - 1, j] != '#')
            {
                spawnPoints.Add(new Vector3(xOffset + j * transform.lossyScale.x + 0.5f, yOffset + (i - 1)* transform.lossyScale.y + 0.5f, 0));
            }

            // go UP
            if (i - 1 >= 0 && !_backtrackingPath[i - 1, j])
            {
                _backtrackingPath[i - 1, j] = true;
                SpawnBacktracking(i - 1, j, lineNum, colNum, xOffset, yOffset, roomModel);
            }
            // go RIGHT
            if (j + 1 < colNum && !_backtrackingPath[i, j + 1])
            {
                _backtrackingPath[i, j + 1] = true;
                SpawnBacktracking(i, j + 1, lineNum, colNum, xOffset, yOffset, roomModel);
            }
            // go DOWN
            if (i + 1 < lineNum && !_backtrackingPath[i + 1, j])
            {
                _backtrackingPath[i + 1, j] = true;
                SpawnBacktracking(i + 1, j, lineNum, colNum, xOffset, yOffset, roomModel);
            }
            // go LEFT
            if (j - 1 >= 0 && !_backtrackingPath[i, j - 1])
            {
                _backtrackingPath[i, j - 1] = true;
                SpawnBacktracking(i, j - 1, lineNum, colNum, xOffset, yOffset, roomModel);
            }
        }

        protected override void CallStartRoomEvent()
        {
            _availablePosition -= transform.position - new Vector3(1.0f, 1.0f, 0);
            base.CallStartRoomEvent();
        }

        protected override void SetCollidersOnRoom()
        {
            base.SetCollidersOnRoom();
            colRoomConfiner.gameObject.transform.localPosition = new Vector2(roomData.Dimensions.Width / 2f , roomData.Dimensions.Height / 2f );
            colRoomConfiner.gameObject.GetComponent<BoxCollider2D>().size = new Vector2((roomData.Dimensions.Width + 2)*3, (roomData.Dimensions.Height + 2)*3);
        }
    }
}