using LevelGenerator;
using System;
using System.Collections.Generic;
using Game.GameManager;
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

        /**
         * Constructor of the Map object that uses an input file for the dungeon
         */
        public Map(DungeonFileSo dungeonFileSo, string roomsFilePath = null, int mode = 0)
        {
            GameManagerSingleton.instance.maxTreasure = 0;
            GameManagerSingleton.instance.maxRooms = 0;
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

        //Constructs a Map based on the Dungeon created in "real-time" from the EA
        //For now, we aren't changing this to the new method that adds treasures and enemies, but is the same principle.
        public Map(Dungeon dun)
        {
            Coordinates currentRoomCoordinates;
            string dungeonPartCode;
            int treasure, difficulty, enemyType, items, npcs;

            List<int> lockedRooms = new List<int>();
            List<int> keys = new List<int>();

            List<int> keyIDs, lockIDs;

            int corridorx, corridory;
            foreach (Room room in dun.Rooms)
            {
                if (room.Type == LevelGenerator.RoomType.Key)
                {
                    keys.Add(room.Key);
                }
                else if (room.Type == LevelGenerator.RoomType.Locked)
                {
                    lockedRooms.Add(room.Key);
                }
            }
            dun.SetBoundariesFromRoomList();

            //The size is normalized to be always positive (easier to handle a matrix)
            dun.SetDimensionsFromBoundaries();

            DungeonPartByCoordinates = new Dictionary<Coordinates, DungeonPart>();

            for (int i = dun.boundaries.MinBoundaries.X; i < dun.boundaries.MaxBoundaries.X + 1; ++i)
            {
                for (int j = dun.boundaries.MinBoundaries.Y; j < dun.boundaries.MaxBoundaries.Y + 1; ++j)
                {
                    int iPositive = i - dun.boundaries.MinBoundaries.X;
                    int jPositive = j - dun.boundaries.MinBoundaries.Y;
                    Room actualRoom = dun.grid[i, j];
                    treasure = 0;
                    difficulty = 0;
                    enemyType = -1;
                    items = 0;
                    npcs = 0;
                    keyIDs = null;
                    lockIDs = null;
                    dungeonPartCode = null;
                    if (actualRoom != null)
                    {
                        currentRoomCoordinates = new Coordinates(2 * iPositive, 2 * jPositive);

                        if (i == 0 && j == 0)
                        {
                            StartRoomCoordinates = new Coordinates(iPositive * 2, jPositive * 2);
                            dungeonPartCode = DungeonPart.PartType.START_ROOM;
                        }
                        else if (actualRoom.Type == LevelGenerator.RoomType.Normal)
                        {
                            if (actualRoom.IsLeafNode())
                            {
                                treasure = UnityEngine.Random.Range(0, (int)GameManagerSingleton.instance.treasureSet.Items.Count);
                                dungeonPartCode = DungeonPart.PartType.TREASURE_ROOM;
                            }
                        }
                        else if (actualRoom.Type == LevelGenerator.RoomType.Key)
                        {
                            keyIDs = new List<int>
                            {
                                keys.IndexOf(actualRoom.Key) + 1
                            };
                        }
                        else if (actualRoom.Type == LevelGenerator.RoomType.Locked)
                        {
                            if (lockedRooms.IndexOf(actualRoom.Key) == lockedRooms.Count - 1)
                            {
                                FinalRoomCoordinates = new Coordinates(iPositive * 2, jPositive * 2);
                                dungeonPartCode = DungeonPart.PartType.FINAL_ROOM;
                            }
                        }
                        else
                        {
                            Debug.Log("Something went wrong printing the tree!\n");
                            Debug.Log("This Room type does not exist!\n\n");
                        }
                        DungeonPartByCoordinates.Add(currentRoomCoordinates, DungeonPartFactory.CreateDungeonRoomFromEARoom(currentRoomCoordinates, dungeonPartCode, keyIDs, difficulty, treasure, enemyType, items, npcs));

                        Room parent = actualRoom.Parent;
                        if (parent != null)
                        {
                            corridorx = parent.x - actualRoom.x + 2 * iPositive;
                            corridory = parent.y - actualRoom.y + 2 * jPositive;
                            currentRoomCoordinates = new Coordinates(corridorx, corridory);
                            dungeonPartCode = DungeonPart.PartType.CORRIDOR;
                            if (actualRoom.Type == LevelGenerator.RoomType.Locked)
                            {
                                lockIDs = new List<int>
                                {
                                    keys.IndexOf(actualRoom.Key) + 1
                                };
                                dungeonPartCode = DungeonPart.PartType.LOCKED;
                            }
                            DungeonPartByCoordinates.Add(currentRoomCoordinates, DungeonPartFactory.CreateDungeonCorridorFromEACorridor(currentRoomCoordinates, dungeonPartCode, lockIDs));
                        }
                    }
                }
            }

            BuildDefaultRooms();
        }


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
            //
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
                if (room.Enemies != -1)
                {
                    nEnemies += room.Enemies;
                }
                if (room.Npcs != -1)
                {
                    nNPCs += room.Npcs;
                }
            }
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
                    room.Dimensions = roomDimensions;
                    room.InitializeTiles(); // aloca memória para os tiles
                    for (int x = 0; x < room.Dimensions.Width; x++)
                        for (int y = 0; y < room.Dimensions.Height; y++)
                            room.Tiles[x, y] = defaultTileID;
                }
            }
        }
    }
}