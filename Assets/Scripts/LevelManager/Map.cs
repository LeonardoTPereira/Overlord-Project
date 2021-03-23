using System.Collections.Generic;
using UnityEngine;
using LevelGenerator;
using System;

public class Map
{

    private static class MapFileType
    {
        public const int ONE_KEY_ONE_DOOR = 0;
        public const int N_KEYS_N_DOORS = 1;
    }

    public int nRooms;

    // Usar variáveis acima do escopo das funções de interpretação de arquivos
    // (parser) torna a compreensão de terceiros mais difícil
    public Dictionary<Coordinates, DungeonPart> dungeonPartByCoordinates;
    public Coordinates startRoomCoordinates, finalRoomCoordinates;
    public Dimensions dimensions;
    private string currentMapFile;

    private string[] currentMapParsedFile;
    private int currentMapFileLineIndex;

    // Valores para gerar salas sem o arquivo de definição interna

    public static int defaultTileID = 2;

    /**
     * Constructor of the Map object that uses an input file for the dungeon
     */
    public Map(string text, string roomsFilePath = null, int mode = 0)
    {
        GameManager.instance.maxTreasure = 0;
        GameManager.instance.maxRooms = 0;
        // Create a Room grid with the sizes read
        dungeonPartByCoordinates = new Dictionary<Coordinates, DungeonPart>();

        ReadMapFile(text, mode); // lê o mapa global
        if (roomsFilePath != null)
        {
            // Dá a opção de gerar o mapa com ou sem os tiles
            // Debug.Log("Has Room File");
            // Lê cada sala, com seus tiles
            ReadRoomsFile(roomsFilePath);
        }
        else
        {
            // Sala vazia padrão
            Debug.Log("Doesn't Have Room File");
            BuildDefaultRooms();
        }
    }
    

    private void ReadMapFile(string text, int mode)
    {
        MapFileHandler mapFileHandler = new MapFileHandler(text);
        dimensions = mapFileHandler.GetMapDimensions();
        string dungeonPartCode;
        DungeonPart currentDungeonPart;
        while (mapFileHandler.HasMoreLines())
        {
            currentDungeonPart = DungeonPartFactory.CreateDungeonPartFromFile(mapFileHandler);
            if (currentDungeonPart.IsStartRoom())
                startRoomCoordinates = currentDungeonPart.GetCoordinates();
            else if (currentDungeonPart.IsFinalRoom())
            {
                finalRoomCoordinates = currentDungeonPart.GetCoordinates();
            }
            dungeonPartByCoordinates.Add(currentDungeonPart.coordinates, currentDungeonPart);
        }
    }


    //Recebe os dados de tiles das salas
    private void ReadRoomsFile(string text)
    {
        var splitFile = new string[] { "\r\n", "\r", "\n" };

        //StreamReader streamReaderMap = new StreamReader(filepath);
        var NameLines = text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);

        //sizeX = int.Parse(streamReaderMap.ReadLine());
        //sizeY = int.Parse(streamReaderMap.ReadLine());
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
                currentRoom = (DungeonRoom) dungeonPartByCoordinates[currentRoomCoordinates];
                currentRoom.Dimensions = new Dimensions(roomWidth, roomHeight);
                currentRoom.InitializeTiles(); // aloca memória para os tiles
                for (int x = 0; x < currentRoom.Dimensions.Width; x++)
                {
                    for (int y = 0; y < currentRoom.Dimensions.Height; y++)
                    {
                        currentRoom.tiles[x, y] = int.Parse(NameLines[i++]); // FIXME Desinverter x e y: foi feito assim pois o arquivo de entrada foi passado em um formato invertido
                        txtLine++;
                    }
                }
            }
            catch(InvalidCastException)
            {
                Debug.LogError($"One of the rooms in the file has the wrong coordinates - x = {currentRoomCoordinates.X}, y = {currentRoomCoordinates.Y}");
            }
        }
        Debug.Log("Rooms read.");
    }

    //Cria salas vazias no tamanho padrão
    private void BuildDefaultRooms()
    {
        Dimensions dimensions = new Dimensions(Util.defaultRoomSizeX, Util.defaultRoomSizeY);
        foreach (DungeonPart currentPart in dungeonPartByCoordinates.Values )
        {
            if(currentPart is DungeonRoom room)
            {
                Debug.Log("building a default room");
                room.Dimensions = dimensions;
                room.InitializeTiles(); // aloca memória para os tiles
                for (int x = 0; x < room.Dimensions.Width; x++)
                    for (int y = 0; y < room.Dimensions.Height; y++)
                        room.tiles[x, y] = defaultTileID; 
            }
        }
    }



    // ============================================================================================================== //



    //Constructs a Map based on the Dungeon created in "real-time" from the EA
    //For now, we aren't changing this to the new method that adds treasures and enemies, but is the same principle.
    public Map(Dungeon dun)
    {
        Room actualRoom, parent;
        RoomGrid grid = dun.roomGrid;
        Coordinates currentRoomCoordinates;
        LevelGenerator.Type type;
        string dungeonPartCode;
        int iPositive, jPositive;
        int treasure, difficulty;

        List<int> lockedRooms = new List<int>();
        List<int> keys = new List<int>();

        List<int> keyIDs, lockIDs;

        int corridorx, corridory;
        foreach (Room room in dun.RoomList)
        {
            if (room.Type == LevelGenerator.Type.key)
                keys.Add(room.KeyToOpen);
            else if (room.Type == LevelGenerator.Type.locked)
                lockedRooms.Add(room.KeyToOpen);
        }
        dun.SetBoundariesFromRoomList();

        //The size is normalized to be always positive (easier to handle a matrix)
        dun.SetDimensionsFromBoundaries();

        dungeonPartByCoordinates = new Dictionary<Coordinates, DungeonPart>();

        for (int i = dun.boundaries.MinBoundaries.X; i < dun.boundaries.MaxBoundaries.X + 1; ++i)
        {
            for (int j = dun.boundaries.MinBoundaries.Y; j < dun.boundaries.MaxBoundaries.Y + 1; ++j)
            {
                iPositive = i - dun.boundaries.MinBoundaries.X;
                jPositive = j - dun.boundaries.MinBoundaries.Y;
                actualRoom = grid[i, j];
                treasure = 0;
                difficulty = 0;
                keyIDs = null;
                lockIDs = null;
                dungeonPartCode = null;
                if (actualRoom != null)
                {
                    currentRoomCoordinates = new Coordinates(2 * iPositive, 2 * jPositive);
                    type = actualRoom.Type;

                    if (i == 0 && j == 0)
                    {
                        Debug.Log("Found start");
                        startRoomCoordinates = new Coordinates(iPositive * 2, jPositive * 2);
                        dungeonPartCode = DungeonPart.Type.START_ROOM;
                    }
                    else if (type == LevelGenerator.Type.normal)
                    {
                        if (actualRoom.IsLeafNode())
                        {
                            treasure = UnityEngine.Random.Range(0, GameManager.instance.treasureSet.Items.Count);
                            dungeonPartCode = DungeonPart.Type.TREASURE_ROOM;
                            Debug.Log("This is a Leaf Node Room! " + (iPositive * 2) + " - " + (jPositive * 2));
                        }
                        else
                            difficulty = GameManager.instance.dungeonDifficulty;
                        //rooms[iPositive * 2, jPositive * 2].keyID = 0;
                    }
                    else if (type == LevelGenerator.Type.key)
                    {
                        keyIDs = new List<int>();
                        keyIDs.Add(keys.IndexOf(actualRoom.KeyToOpen) + 1);
                        difficulty = GameManager.instance.dungeonDifficulty;
                    }
                    else if (type == LevelGenerator.Type.locked)
                    {
                        if (lockedRooms.IndexOf(actualRoom.KeyToOpen) == lockedRooms.Count - 1)
                        {
                            finalRoomCoordinates = new Coordinates(iPositive * 2, jPositive * 2);
                            dungeonPartCode = DungeonPart.Type.FINAL_ROOM;
                        }
                        else
                        {
                            difficulty = GameManager.instance.dungeonDifficulty;
                        }
                            
                        /*else
                            rooms[iPositive * 2, jPositive * 2].keyID = 0;*/
                    }
                    else
                    {
                        Debug.Log("Something went wrong printing the tree!\n");
                        Debug.Log("This Room type does not exist!\n\n");
                    }
                    dungeonPartByCoordinates.Add(currentRoomCoordinates, DungeonPartFactory.CreateDungeonRoomFromEARoom(currentRoomCoordinates, dungeonPartCode, keyIDs, difficulty, treasure));

                    parent = actualRoom.Parent;
                    if (parent != null)
                    {
                        corridorx = parent.X - actualRoom.X + 2 * iPositive;
                        corridory = parent.Y - actualRoom.Y + 2 * jPositive;
                        currentRoomCoordinates = new Coordinates(corridorx, corridory);
                        dungeonPartCode = DungeonPart.Type.CORRIDOR;
                        if (type == LevelGenerator.Type.locked)
                        {
                            lockIDs = new List<int>();
                            lockIDs.Add(keys.IndexOf(actualRoom.KeyToOpen) + 1);
                            dungeonPartCode = DungeonPart.Type.LOCKED;
                        }
                        dungeonPartByCoordinates.Add(currentRoomCoordinates, DungeonPartFactory.CreateDungeonCorridorFromEACorridor(currentRoomCoordinates, dungeonPartCode, lockIDs));
                    }
                }
            }
        }
        Debug.Log("Starts:" + startRoomCoordinates.X + "-" + startRoomCoordinates.Y);
        Debug.Log("Dungeon read.");

        BuildDefaultRooms();
    }
}
