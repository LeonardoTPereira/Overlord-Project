using System.Collections;
using System.Collections.Generic;
using Game.LevelManager.DungeonManager;
using PlatformGame.Enemy;
using UnityEngine;
using Util;
using PlatformGame.Util;

namespace PlatformGame.Dungeon
{
    public class PlatformRoomBhv : RoomBhv
    {        
        [Header("Plataform Game Related")]
        public CompositeCollider2D colRoomConfiner;
        private bool _isSpawnPointsGenerated = false;

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

        protected override void InstantiateCornerProps() { }
        
        protected override void SetSpritesToWalls() { }
                
        public override void SpawnEnemies()
        {
            while (!_isSpawnPointsGenerated) { }

            base.SpawnEnemies();
        }

        protected override void SetEnemySpawners()
        {
            var roomPosition = transform.position;
            var xOffset = roomPosition.x;
            var yOffset = roomPosition.y;

            _isSpawnPointsGenerated = false;

            // Need to have the roomModel matrix to work            
            char [,] roomModel = CreateRoomModel();

            CalculateSpawnPoints calcSP = new CalculateSpawnPoints(roomModel.GetLength(0), roomModel.GetLength(1), roomPosition.x, roomPosition.y, transform.lossyScale);
            calcSP.SetStartPosition(roomModel, northDoor, eastDoor, southDoor);
            spawnPoints = calcSP.GetSpawnPoints(roomModel);

            _isSpawnPointsGenerated = true;
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