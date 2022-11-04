using System;
using System.Collections.Generic;
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

        /**
         * Constructor of the Map object that uses an input file for the dungeon
         */
        public Map(DungeonFileSo dungeonFileSo, string roomsFilePath = null, int mode = 0)
        {
            NTreasureRooms = 0;
            DungeonPartByCoordinates = new Dictionary<Coordinates, DungeonPart>();
            ReadMapFile(dungeonFileSo);
            if (roomsFilePath != null)
            {
                ReadRoomsFile(roomsFilePath);
            }
            else
            {
                // Sala vazia padrão
                BuildDefaultRooms();
            }
        }

        private void ReadMapFile(DungeonFileSo dungeonFileSo)
        {
            Dimensions = dungeonFileSo.DungeonSizes;
            DungeonPart currentDungeonPart;
            dungeonFileSo.ResetIndex();
            while ((currentDungeonPart = dungeonFileSo.GetNextPart()) != null)
            {
                ProcessDungeonPart(currentDungeonPart);
            }
            foreach (var room in dungeonFileSo.Rooms)
            {
                if ((room.keys?.Count??0) > 0)
                {
                    NKeys += room.keys.Count;
                }
                if ((room.locks?.Count??0) > 0)
                {
                    NLocks += room.locks.Count;
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
            if (currentDungeonPart.IsTreasureRoom())
            {
                NTreasureRooms++;
            }
        }


        //Recebe os dados de tiles das salas
        private void ReadRoomsFile(string text)
        {
            var splitFile = new string[] { "\r\n", "\r", "\n" };

            var nameLines = text.Split(splitFile, StringSplitOptions.RemoveEmptyEntries);

            int roomWidth, roomHeight;
            DungeonRoom currentRoom;
            Coordinates currentRoomCoordinates;
            roomWidth = int.Parse(nameLines[0]);
            roomHeight = int.Parse(nameLines[1]);

            int txtLine = 3;
            for (uint i = 2; i < nameLines.Length;)
            {
                int roomX, roomY;
                roomX = int.Parse(nameLines[i++]);
                roomY = int.Parse(nameLines[i++]);
                currentRoomCoordinates = new Coordinates(roomX, roomY);
                txtLine += 2;
                try
                {
                    currentRoom = (DungeonRoom)DungeonPartByCoordinates[currentRoomCoordinates];
                    currentRoom.Dimensions = new Dimensions(roomWidth, roomHeight);
                    currentRoom.InitializeTiles(); // aloca memória para os tiles
                    for (int x = 0; x < currentRoom.Dimensions.Width; x++)
                    {
                        for (int y = 0; y < currentRoom.Dimensions.Height; y++)
                        {
                            int tileID = int.Parse(nameLines[i++]);
                            currentRoom.Tiles[x, y] = tileID; // TODO Desinverter x e y: foi feito assim pois o arquivo de entrada foi passado em um formato invertido
                            txtLine++;
                        }
                    }
                }
                catch (InvalidCastException)
                {
                    Debug.LogError($"One of the rooms in the file has the wrong coordinates - x = {currentRoomCoordinates.X}, y = {currentRoomCoordinates.Y}");
                }
            }
        }

        //Cria salas vazias no tamanho padrão
        protected virtual void BuildDefaultRooms()
        {
            Dimensions roomDimensions = new Dimensions(Constants.DefaultRoomSizeX, Constants.DefaultRoomSizeY);
            foreach (DungeonPart currentPart in DungeonPartByCoordinates.Values)
            {
                if (currentPart is DungeonRoom room)
                {
                    var roomInput = ScriptableObject.CreateInstance<RoomGeneratorInput>();
                    var doorList = CreateDoorList(room.Coordinates);
                    roomInput.Init(roomDimensions, doorList[0], doorList[1], doorList[2], doorList[3]);
                    room.CreateRoom(roomDimensions);
                }
            }
        }
        
        protected List<int> CreateDoorList(Coordinates currentRoomCoordinates)
        {
            var doorList = new List<int>();
            doorList.Add(IsCorridor(new Coordinates(currentRoomCoordinates.X, currentRoomCoordinates.Y+1)));
            doorList.Add(IsCorridor(new Coordinates(currentRoomCoordinates.X, currentRoomCoordinates.Y-1)));
            doorList.Add(IsCorridor(new Coordinates(currentRoomCoordinates.X+1, currentRoomCoordinates.Y)));
            doorList.Add(IsCorridor(new Coordinates(currentRoomCoordinates.X-1, currentRoomCoordinates.Y)));
            return doorList;
        }

        private int IsCorridor(Coordinates coordinates)
        {
            return DungeonPartByCoordinates.ContainsKey(coordinates) ? 1 : 0;
        }
    }
}