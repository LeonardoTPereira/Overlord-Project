using LevelGenerator;
using System;
using System.Collections.Generic;
using Game.GameManager;
using ScriptableObjects;
using UnityEngine;

namespace Game.LevelManager
{
    public class Map
    {

        private static class MapFileType
        {
            public const int ONE_KEY_ONE_DOOR = 0;
            public const int N_KEYS_N_DOORS = 1;
        }


        // Usar variáveis acima do escopo das funções de interpretação de arquivos
        // (parser) torna a compreensão de terceiros mais difícil
        private Dictionary<Coordinates, DungeonPart> dungeonPartByCoordinates;
        private Coordinates startRoomCoordinates;
        private Coordinates finalRoomCoordinates;
        private Dimensions dimensions;
        private int nRooms;
        private int nKeys;
        private int nLocks;
        private int nEnemies;
        private int nNPCs;
        private int nTreasure;
        private int nTreasureRooms;

        // Valores para gerar salas sem o arquivo de definição interna

        public const int defaultTileID = 2;

        public int NRooms { get => nRooms; set => nRooms = value; }
        public int NKeys { get => nKeys; set => nKeys = value; }
        public int NLocks { get => nLocks; set => nLocks = value; }
        public int NEnemies { get => nEnemies; set => nEnemies = value; }
        public int NNPCs { get => nNPCs; set => nNPCs = value; }
        public Dictionary<Coordinates, DungeonPart> DungeonPartByCoordinates { get => dungeonPartByCoordinates; set => dungeonPartByCoordinates = value; }
        public Coordinates StartRoomCoordinates { get => startRoomCoordinates; set => startRoomCoordinates = value; }
        public Coordinates FinalRoomCoordinates { get => finalRoomCoordinates; set => finalRoomCoordinates = value; }
        public Dimensions Dimensions { get => dimensions; set => dimensions = value; }
        public int NTreasureRooms
        {
            get => nTreasureRooms;
            set => nTreasureRooms = value;
        }

        /**
         * Constructor of the Map object that uses an input file for the dungeon
         */
        public Map(DungeonFileSo dungeonFileSo, string roomsFilePath = null, int mode = 0)
        {
            GameManagerSingleton.Instance.maxRooms = 0;
            GameManagerSingleton.Instance.maxEnemies = 0;
            NTreasureRooms = 0;
            // Create a Room grid with the sizes read
            DungeonPartByCoordinates = new Dictionary<Coordinates, DungeonPart>();

            ReadMapFile(dungeonFileSo, mode); // lê o mapa global
            if (roomsFilePath != null)
            {
                // Lê cada sala, com seus tiles
                ReadRoomsFile(roomsFilePath);
            }
            else
            {
                // Sala vazia padrão
                BuildDefaultRooms();
            }
        }

        //TODO passes level's SO when created in real time to take place of old method that used the "Dungeon" object
        private void ReadMapFile(DungeonFileSo dungeonFileSO, int mode)
        {
            Dimensions = dungeonFileSO.dimensions;
            DungeonPart currentDungeonPart;
            dungeonFileSO.ResetIndex();
            while ((currentDungeonPart = dungeonFileSO.GetNextPart()) != null)
            {
                if (currentDungeonPart.IsRoom())
                {
                    nRooms++;
                    if (currentDungeonPart.IsTreasureRoom())
                    {
                        NTreasureRooms++;
                    }
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
            Debug.Log("NTreasure Rooms in Map = "+NTreasureRooms);

            GameManagerSingleton.Instance.maxRooms = nRooms;
            foreach (SORoom room in dungeonFileSO.rooms)
            {
                if (room.keys.Count != -1)
                {
                    nKeys += room.keys.Count;
                }
                if (room.locks.Count != -1)
                {
                    nLocks += room.locks.Count;
                }

                if (room.Treasures != -1)
                {
                    nTreasure += room.Treasures;
                }
                if (room.TotalEnemies != -1)
                {
                    nEnemies += room.TotalEnemies;
                }
                if (room.Npcs != -1)
                {
                    nNPCs += room.Npcs;
                }
            }

            GameManagerSingleton.Instance.maxEnemies = nEnemies;
        }


        //Recebe os dados de tiles das salas
        private void ReadRoomsFile(string text)
        {
            var splitFile = new string[] { "\r\n", "\r", "\n" };

            var NameLines = text.Split(splitFile, StringSplitOptions.RemoveEmptyEntries);

            int roomWidth, roomHeight;
            DungeonRoom currentRoom;
            Coordinates currentRoomCoordinates;
            roomWidth = int.Parse(NameLines[0]);
            roomHeight = int.Parse(NameLines[1]);

            int txtLine = 3;
            for (uint i = 2; i < NameLines.Length;)
            {
                int roomX, roomY;
                roomX = int.Parse(NameLines[i++]);
                roomY = int.Parse(NameLines[i++]);
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
                            int tileID = int.Parse(NameLines[i++]);
                            currentRoom.Tiles[x, y] = tileID; // FIXME Desinverter x e y: foi feito assim pois o arquivo de entrada foi passado em um formato invertido
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
        private void BuildDefaultRooms()
        {
            Dimensions roomDimensions = new Dimensions(Util.Constants.defaultRoomSizeX, Util.Constants.defaultRoomSizeY);
            foreach (DungeonPart currentPart in DungeonPartByCoordinates.Values)
            {
                if (currentPart is DungeonRoom room)
                {
                    room.CreateRoom(roomDimensions);
                }
            }
        }
    }
}