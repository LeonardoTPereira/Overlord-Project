using System;
using System.Collections.Generic;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NPCs;
using UnityEngine;
using Util;
using Enums = Util.Enums;

namespace Game.LevelManager.DungeonLoader
{
    [Serializable]
    public class DungeonRoom : DungeonPart
    {
        [SerializeField]
        private List<int> keyIDs;
        [SerializeField]
        protected ItemsAmount items;
        [SerializeField]
        private List<NpcSo> npcs;
        [SerializeField]
        private EnemiesByType enemiesByType;
        [SerializeField]
        private int totalEnemies;

        private Vector3 _currentFreeTilePosition;
        private Vector3 _roomCenter;
        private Queue<Tile> _floodFillState;
        [field: SerializeField] public bool HasItemPreference { get; set; }

        [field: SerializeField] public bool HasNpcPreference { get; set; }
        public RoomData Tiles { get; set; }

        public DungeonRoom(Coordinates coordinates, string code, List<int> keyIDs, int treasure, int totalEnemies, int npc) : base(coordinates, code)
        {
            KeyIDs = keyIDs;
            HasItemPreference = treasure > 0;
            TotalEnemies = totalEnemies;
            HasNpcPreference = npc > 0;
            EnemiesByType = null;
            Items = null;
            Npcs = null;
            _floodFillState = new Queue<Tile>();
        }

        public Vector3 GetNextAvailablePosition()
        {
            Tile candidateTile;
            do
            {
                do
                {
                    candidateTile = _floodFillState.Dequeue();
                }
                while (candidateTile.HasBeenVisited && _floodFillState.Count > 0);
                candidateTile.HasBeenVisited = true;
                var x = candidateTile.Position.x;
                var y = candidateTile.Position.y;
                PushNewTiles(x, y);
            } while (_floodFillState.Count > 0 && TileIsOccupied(candidateTile));
            _currentFreeTilePosition = new Vector3(candidateTile.Position.x + 0.5f,  candidateTile.Position.y + 0.5f, 0);

            return _currentFreeTilePosition;
        }
        
        private void PushNewTiles(float x, float y)
        {
            if (!OutOfBounds(new Vector2(x + 1, y)))
            {
                _floodFillState.Enqueue(Tiles[(int) x + 1, (int) y]);
            }

            if (!OutOfBounds(new Vector2(x - 1, y)))
            {
                _floodFillState.Enqueue(Tiles[(int) x - 1, (int) y]);
            }

            if (!OutOfBounds(new Vector2(x, y + 1)))
            {
                _floodFillState.Enqueue(Tiles[(int) x, (int) y + 1]);
            }

            if (!OutOfBounds(new Vector2(x, y - 1)))
            {
                _floodFillState.Enqueue(Tiles[(int) x, (int) y - 1]);
            }
        }

        private bool TileIsOccupied(Tile candidateTile)
        {
            return Tiles[(int)candidateTile.Position.x, (int)candidateTile.Position.y].TileType !=
                   Enums.TileTypes.Floor;
        }

        private bool OutOfBounds(Vector2 position)
        {
            if (position.x > Dimensions.Width - 1) return true;
            if (position.x < 0) return true;
            if (position.y < 0) return true;
            return position.y > Dimensions.Height - 1;
        }
        
        public virtual void CreateRoom(Dimensions roomDimensions, RoomGeneratorInput roomGeneratorInput = null)
        {
            Dimensions = roomDimensions;
            var roomType = RandomSingleton.GetInstance().Random.Next((int)Enums.RoomPatterns.Count);
            if (roomGeneratorInput == null)
            {
                DefaultRoomCreator.CreateRoomOfType(this, roomType);
            }
            else
            {
                SoRoomLoader.CreateRoom(this, roomGeneratorInput);
            }

            SetCenterAndFloodFillState();
        }

        protected void SetCenterAndFloodFillState()
        {
            _roomCenter = new Vector3(Dimensions.Width / 2.0f, Dimensions.Height / 2.0f, 0);
            _floodFillState.Enqueue(Tiles[(int)_roomCenter.x, (int)_roomCenter.y]);
        }

        public int TotalEnemies 
        {
            get => totalEnemies;
            set => totalEnemies = value;
        }
        public EnemiesByType EnemiesByType 
        {
            get => enemiesByType;
            set => enemiesByType = value;
        }

        public ItemsAmount Items
        {
            get => items; 
            set => items = value;
        }
        public Dimensions Dimensions { get; set; }
        public List<int> KeyIDs
        {
            get => keyIDs; 
            set => keyIDs = value;
        }

        public List<NpcSo> Npcs
        {
            get => npcs; 
            set => npcs = value;
        }
    }
}