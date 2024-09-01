using System;
using System.Collections.Generic;
using Game.ExperimentControllers;
using Game.LevelGenerator.LevelSOs;
using UnityEngine;
using Util;

namespace Game.LevelManager.DungeonLoader
{
    public class Map
    {
        public int NRooms { get; set; }
        public int NKeys { get; set; }
        public int NLocks { get; set; }
        public int NEnemies { get; set; }
        public int NNPCs { get; set; }
        public int TotalTreasure { get; set; }

        public Dictionary<Coordinates, DungeonPart> DungeonPartByCoordinates { get; set; }
        public Coordinates StartRoomCoordinates { get; set; }
        public Coordinates FinalRoomCoordinates { get; set; }
        public Dimensions Dimensions { get; set; }
        public int NTreasureRooms { get; set; }
        private bool _createRooms;
        private Enums.GameType _gameType;

        public Map(DungeonFileSo dungeonFileSo, bool createRooms, Vector2 roomSize, Enums.GameType gameType)
        {
            NTreasureRooms = 0;
            _createRooms = createRooms;
            _gameType = gameType;
            DungeonPartByCoordinates = new Dictionary<Coordinates, DungeonPart>();
            ReadMapFile(dungeonFileSo);
            BuildRooms(roomSize);
        }

        private void ReadMapFile(DungeonFileSo dungeonFileSo)
        {
            Dimensions = dungeonFileSo.DungeonSizes;
            dungeonFileSo.ResetIndex();
            while (dungeonFileSo.GetNextPart(_gameType) is { } currentDungeonPart)
            {
                ProcessDungeonPart(currentDungeonPart);
            }
            foreach (var room in dungeonFileSo.Parts)
            {
                if ((room.Keys?.Count??0) > 0)
                {
                    NKeys += room.Keys.Count;
                }
                if ((room.Locks?.Count??0) > 0)
                {
                    NLocks += room.Locks.Count;
                }
            }
            NEnemies = dungeonFileSo.TotalEnemies;
            TotalTreasure = dungeonFileSo.TotalTreasures;
            NNPCs = dungeonFileSo.TotalNpcs;
        }

        private void ProcessDungeonPart(DungeonPart currentDungeonPart)
        {
            if (currentDungeonPart.IsRoom())
            {
                AddRoomData(currentDungeonPart);
            }

            if (currentDungeonPart.IsStartRoom())
            {
                StartRoomCoordinates = currentDungeonPart.GetCoordinates();
            }
            else if (currentDungeonPart.IsFinalRoom())
            {
                FinalRoomCoordinates = currentDungeonPart.GetCoordinates();
            }

            DungeonPartByCoordinates.Add(currentDungeonPart.Coordinates, currentDungeonPart);
        }

        private void AddRoomData(DungeonPart currentDungeonPart)
        {
            NRooms++;
            if (currentDungeonPart.IsLeafNode() || currentDungeonPart.IsLockedNode())
            {
                NTreasureRooms++;
            }
        }

        private void BuildRooms(Vector2 roomSize)
        {
            var roomDimensions = new Dimensions((int)roomSize.x, (int)roomSize.y);
            foreach (var currentPart in DungeonPartByCoordinates.Values)
            {
                if (currentPart is not DungeonRoom room) continue;
                RoomGeneratorInput roomGeneratorInput = null;
                if (_createRooms)
                {
                    var doorList = CreateDoorList(room.Coordinates);
                    roomGeneratorInput = ScriptableObject.CreateInstance<RoomGeneratorInput>();
                    roomGeneratorInput.Init(roomDimensions, doorList[0], doorList[1], doorList[2], doorList[3]);
                }
                room.CreateRoom(roomDimensions, roomGeneratorInput);
            }
        }
        
        protected List<int> CreateDoorList(Coordinates currentRoomCoordinates)
        {
            var doorList = new List<int>
            {
                IsCorridor(new Coordinates(currentRoomCoordinates.X, currentRoomCoordinates.Y + 1)),
                IsCorridor(new Coordinates(currentRoomCoordinates.X, currentRoomCoordinates.Y - 1)),
                IsCorridor(new Coordinates(currentRoomCoordinates.X+1, currentRoomCoordinates.Y)),
                IsCorridor(new Coordinates(currentRoomCoordinates.X-1, currentRoomCoordinates.Y))
            };
            return doorList;
        }

        private int IsCorridor(Coordinates coordinates)
        {
            return DungeonPartByCoordinates.ContainsKey(coordinates) ? 1 : 0;
        }
    }
}