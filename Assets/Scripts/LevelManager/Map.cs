using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LevelGenerator;

public class Map
{

    private static class MapFileType
    {
        public const int ONE_KEY_ONE_DOOR = 0;
        public const int N_KEYS_N_DOORS = 1;
    }

    public const string CORRIDOR = "c";

    protected static class RoomType
    {
        public const string START_ROOM = "s";
        public const string FINAL_ROOM = "B";
        public const string TREASURE_ROOM = "T";
    }

    public static int sizeX;
    public static int sizeY;
    public static int index; // Não é utilizada

    // Usar variáveis acima do escopo das funções de interpretação de arquivos
    // (parser) torna a compreensão de terceiros mais difícil
    public Room[,] rooms;
    public int startX, startY;
    public int endX, endY;
    private int currentRoomXPos, currentRoomYPos;
    private string currentRoomCode;
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
        ReadMapFile(text, mode); // lê o mapa global
        if (roomsFilePath != null)
        {
            // Dá a opção de gerar o mapa com ou sem os tiles
            // Debug.Log("Has Room File");
            // Lê cada sala, com seus tiles
            ReadRoomsFile(roomsFilePath);
            // O arquivo de tiles das salas foi lido; função de tiles ativada
            Room.tiled = true;
        }
        else
        {
            // Sala vazia padrão
            // Debug.Log("Doesn't Have Room File");
            BuildDefaultRooms();
        }
    }

    private void ReadMapFile(string text, int mode)
    {
        currentMapFile = text;
        switch (mode)
        {
            case MapFileType.ONE_KEY_ONE_DOOR:
                ReadOneKeyOneDoorMapFile();
                break;
            case MapFileType.N_KEYS_N_DOORS:
                ReadNKeysNDoorsMapFile();
                break;
        }
    }

    private void ReadOneKeyOneDoorMapFile()
    {
        InitializeMapFileReader();
        // Read all the other lines in the file
        while (currentMapFileLineIndex < currentMapParsedFile.Length)
        {
            // For now, every room/corridor has its x and y coordinates and code (identifies as a normal room, normal
            // corridor, locked corridor, room with key, room with treasure, etc.
            // TODO: change so the code now has up to 3 lines (if it is a room only): the first for the possible key in
            // the room, the second for the possible treasure, and the third for the possible enemy difficulty
            currentRoomXPos = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
            currentRoomYPos = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
            currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];

            rooms[currentRoomXPos, currentRoomYPos] = new Room(currentRoomXPos, currentRoomYPos);
            // Sala ou corredor(link)?
            if (((currentRoomXPos % 2) + (currentRoomYPos % 2)) == 0)
            {
                // Ambos pares: sala
                ReadRoomDataFromMapFile();
            }
            else
            {
                // If not corridor, is a locked corridor
                ReadCorridorDataFromMapFile();
            }
            // currentMapFileLineIndex++; // There is a dashed line before the info
        }
    }

    private void ReadNKeysNDoorsMapFile()
    {
        InitializeMapFileReader();
        // Read all the other lines in the file
        while (currentMapFileLineIndex < (currentMapParsedFile.Length - 1))
        {
            // For now, every room/corridor has its x and y coordinates and code (identifies as a normal room, normal
            // corridor, locked corridor, room with key, room with treasure, etc.
            // TODO: change so the code now has up to 3 lines (if it is a room only): the first for the possible key in
            // the room, the second for the possible treasure, and the third for the possible enemy difficulty
            currentRoomXPos = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
            Debug.Log("Room X Pos: " + currentRoomXPos);
            currentRoomYPos = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
            Debug.Log("Room Y Pos: " + currentRoomYPos);
            currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];

            rooms[currentRoomXPos, currentRoomYPos] = new Room(currentRoomXPos, currentRoomYPos);
            // Sala ou corredor(link)?
            if (((currentRoomXPos % 2) + (currentRoomYPos % 2)) == 0)
            {
                // Ambos pares: sala
                ReadCompleteRoomDataFromMapFile();
            }
            else
            {
                // If not corridor, is a locked corridor
                ReadCorridorDataFromMapFile();
            }
            // currentMapFileLineIndex++;
        }
    }

    private void InitializeMapFileReader()
    {
        //
        GameManager.instance.maxTreasure = 0;
        GameManager.instance.maxRooms = 0;

        //
        currentMapFileLineIndex = 0;

        // Split the file in lines. A readLine method could also be used
        string[] splitFile = new string[] { "\r\n", "\r", "\n" };
        currentMapParsedFile = currentMapFile.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);

        // The first two lines are the matrix sizes
        sizeX = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
        sizeY = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);

        // currentMapFileLineIndex++;

        // Create a Room grid with the sizes read
        rooms = new Room[sizeX, sizeY];
    }

    private void ReadCompleteRoomDataFromMapFile()
    {
        //
        GameManager.instance.maxRooms += 1;

        //
        CheckIfStartOrFinishRoom();

        //
        rooms[currentRoomXPos, currentRoomYPos].difficulty =
            int.Parse(currentRoomCode) * (GameManager.instance.dungeonDifficulty / 3);
        Debug.Log("Enemies: " + rooms[currentRoomXPos, currentRoomYPos].difficulty);

        // Já lê o código da sala na função chamada anteriormente e já adiciona um indice e aqui de novo
        currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];
        rooms[currentRoomXPos, currentRoomYPos].Treasure = Mathf.Min(int.Parse(currentRoomCode) - 1, 4);
        Debug.Log("Treasures: " + rooms[currentRoomXPos, currentRoomYPos].Treasure);

        //
        if (currentMapFileLineIndex < (currentMapParsedFile.Length - 1))
        {
            currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];

            // Has the room a locked corridor?
            bool hasKey = currentRoomCode[0] == '+';

            //
            if (hasKey)
                rooms[currentRoomXPos, currentRoomYPos].keyID.Clear();

            //
            while (hasKey && (currentMapFileLineIndex < (currentMapParsedFile.Length - 1)))
            {
                // Print log
                Debug.Log("Key: " + int.Parse(currentRoomCode));

                // Add lock to the room
                rooms[currentRoomXPos, currentRoomYPos].keyID.Add(int.Parse(currentRoomCode));
                currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];

                // Has the room a locked corridor?
                hasKey = currentRoomCode[0] == '+';
            }
            // Step back
            currentMapFileLineIndex--;
        }
    }

    private void CheckIfStartOrFinishRoom()
    {
        switch (currentRoomCode)
        {
            case RoomType.START_ROOM: // This is the starting room
                startX = currentRoomXPos;
                startY = currentRoomYPos;
                currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];
                break;
            case RoomType.FINAL_ROOM: // This room has the final item
                endX = currentRoomXPos;
                endY = currentRoomYPos;
                currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];
                break;
            default: // This is an empty room
                break;
        }
    }

    private void ReadCorridorDataFromMapFile()
    {
        if (currentRoomCode != CORRIDOR)
        {
            // As in Breno's case, a lock can have many keys to open it, so we read until a positive number
            // (not lock id) is found
            // But this is not yet available in the "PrintNumericalGridWithConnections" method.
            // Only with the files he provided us
            rooms[currentRoomXPos, currentRoomYPos].lockID.Clear();

            // Auxiliary variable that checks if there are still locks to be read
            bool hasLock = true;

            // Get all the keys needed to unlock the room
            while (hasLock && (currentMapFileLineIndex < (currentMapParsedFile.Length - 1)))
            {
                // Print log
                Debug.Log("Lock: " + int.Parse(currentRoomCode));

                // Add lock to the corridor
                rooms[currentRoomXPos, currentRoomYPos].lockID.Add(-int.Parse(currentRoomCode));
                currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];

                // Has the room a locked corridor?
                hasLock = currentRoomCode[0] == '-';
            }
            // Step back
            currentMapFileLineIndex--;
        }
    }

    /**
     * Reads the Map File Generated from the EA by the "PrintNumericalGridWithConnections" method
     * Not to be confused with the Map constructor that "reads" the created dungeon in real-time by the EA
     */
    private void ReadRoomDataFromMapFile()
    {
        GameManager.instance.maxRooms += 1;
        switch (currentRoomCode)
        {
            case RoomType.START_ROOM: // This is the starting room
                startX = currentRoomXPos;
                startY = currentRoomYPos;
                break;
            case RoomType.FINAL_ROOM: // This room has the final item
                endX = currentRoomXPos;
                endY = currentRoomYPos;
                break;
            case RoomType.TREASURE_ROOM: // This room has a treasure
                                         // "gambiarra" to give a predefined enemy difficulty for the room
                                         // and always place the most valuable treasure
                rooms[currentRoomXPos, currentRoomYPos].difficulty = GameManager.instance.dungeonDifficulty;
                rooms[currentRoomXPos, currentRoomYPos].Treasure = GameManager.instance.treasureSet.Items.Count - 1;
                GameManager.instance.maxTreasure += 50;
                break;
            default: // This is an empty room
                rooms[currentRoomXPos, currentRoomYPos].keyID.Add(int.Parse(currentRoomCode));
                // "gambiarra" to give a predefined enemy difficulty for the room
                rooms[currentRoomXPos, currentRoomYPos].difficulty = GameManager.instance.dungeonDifficulty;
                break;
        }
    }



    // ============================================================================================================== //



    //Recebe os dados de tiles das salas
    private void ReadRoomsFile(string text)
    {
        var splitFile = new string[] { "\r\n", "\r", "\n" };

        //StreamReader streamReaderMap = new StreamReader(filepath);
        var NameLines = text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);

        //sizeX = int.Parse(streamReaderMap.ReadLine());
        //sizeY = int.Parse(streamReaderMap.ReadLine());
        Room.sizeX = int.Parse(NameLines[0]);
        Room.sizeY = int.Parse(NameLines[1]);

        int txtLine = 3;
        for (uint i = 2; i < NameLines.Length;)
        {
            int roomX, roomY;
            roomX = int.Parse(NameLines[i++]);
            roomY = int.Parse(NameLines[i++]);
            txtLine += 2;
            //Debug.Log ("roomX " + roomX + "   roomY " + roomY + "   Line: " + txtLine);
            rooms[roomX, roomY].InitializeTiles(); // aloca memória para os tiles
            for (int x = 0; x < Room.sizeX; x++)
            {
                for (int y = 0; y < Room.sizeY; y++)
                {
                    rooms[roomX, roomY].tiles[x, y] = int.Parse(NameLines[i++]); // FIXME Desinverter x e y: foi feito assim pois o arquivo de entrada foi passado em um formato invertido
                    txtLine++;
                }
            }
        }
        Debug.Log("Rooms read.");
    }

    //Cria salas vazias no tamanho padrão
    private void BuildDefaultRooms()
    {
        Room.sizeX = Util.defaultRoomSizeX;
        Room.sizeY = Util.defaultRoomSizeY;
        for (int roomX = 0; roomX < sizeX; roomX += 2)
        {
            for (int roomY = 0; roomY < sizeY; roomY += 2)
            {
                if (rooms[roomX, roomY] == null)
                    continue;
                rooms[roomX, roomY].InitializeTiles(); // aloca memória para os tiles
                for (int x = 0; x < Room.sizeX; x++)
                {
                    for (int y = 0; y < Room.sizeY; y++)
                    {
                        rooms[roomX, roomY].tiles[x, y] = defaultTileID; // FIXME Desinverter x e y: foi feito assim pois o arquivo de entrada foi passado em um formato invertido
                    }
                }
            }
        }
    }



    // ============================================================================================================== //



    //Constructs a Map based on the Dungeon created in "real-time" from the EA
    //For now, we aren't changing this to the new method that adds treasures and enemies, but is the same principle.
    public Map(Dungeon dun)
    {
        Debug.Log(dun.RoomList.Count);
        LevelGenerator.Room actualRoom, parent;
        RoomGrid grid = dun.roomGrid;
        Type type;
        int iPositive, jPositive;

        List<int> lockedRooms = new List<int>();
        List<int> keys = new List<int>();

        int minX, minY, maxX, maxY, corridorx, corridory;
        minX = Constants.MATRIXOFFSET;
        minY = Constants.MATRIXOFFSET;
        maxX = -Constants.MATRIXOFFSET;
        maxY = -Constants.MATRIXOFFSET;
        foreach (LevelGenerator.Room room in dun.RoomList)
        {
            if (room.Type == Type.key)
                keys.Add(room.KeyToOpen);
            else if (room.Type == Type.locked)
                lockedRooms.Add(room.KeyToOpen);
            if (room.X < minX)
                minX = room.X;
            if (room.Y < minY)
                minY = room.Y;
            if (room.X > maxX)
                maxX = room.X;
            if (room.Y > maxY)
                maxY = room.Y;
        }
        sizeX = 2 * (maxX - minX + 1);
        sizeY = 2 * (maxY - minY + 1);

        rooms = new Room[sizeX, sizeY];

        for (int i = minX; i < maxX + 1; ++i)
        {
            for (int j = minY; j < maxY + 1; ++j)
            {
                iPositive = i - minX;
                jPositive = j - minY;

                actualRoom = grid[i, j];
                if (actualRoom != null)
                {
                    rooms[iPositive * 2, jPositive * 2] = new Room(iPositive * 2, jPositive * 2);
                    type = actualRoom.Type;

                    if (i == 0 && j == 0)
                    {
                        Debug.Log("Found start");
                        startX = iPositive * 2;
                        startY = jPositive * 2;
                    }
                    else if (type == Type.normal)
                    {
                        if (actualRoom.IsLeafNode())
                        {
                            rooms[iPositive * 2, jPositive * 2].Treasure = Random.Range(0, GameManager.instance.treasureSet.Items.Count);
                            Debug.Log("This is a Leaf Node Room! " + (iPositive * 2) + " - " + (jPositive * 2));
                        }
                        //rooms[iPositive * 2, jPositive * 2].keyID = 0;
                    }
                    else if (type == Type.key)
                    {
                        rooms[iPositive * 2, jPositive * 2].keyID.Clear();
                        rooms[iPositive * 2, jPositive * 2].keyID.Add(keys.IndexOf(actualRoom.KeyToOpen) + 1);
                    }
                    else if (type == Type.locked)
                    {
                        if (lockedRooms.IndexOf(actualRoom.KeyToOpen) == lockedRooms.Count - 1)
                        {
                            endX = iPositive * 2;
                            endY = jPositive * 2;
                        }
                        /*else
                            rooms[iPositive * 2, jPositive * 2].keyID = 0;*/
                    }
                    else
                    {
                        Debug.Log("Something went wrong printing the tree!\n");
                        Debug.Log("This Room type does not exist!\n\n");
                    }
                    rooms[iPositive * 2, jPositive * 2].difficulty = GameManager.instance.dungeonDifficulty;
                    parent = actualRoom.Parent;
                    if (parent != null)
                    {
                        corridorx = parent.X - actualRoom.X + 2 * iPositive;
                        corridory = parent.Y - actualRoom.Y + 2 * jPositive;
                        rooms[corridorx, corridory] = new Room(corridorx, corridory);
                        if (type == Type.locked)
                            rooms[corridorx, corridory].lockID.Add(keys.IndexOf(actualRoom.KeyToOpen) + 1);
                    }
                }
            }
        }
        Debug.Log("Starts:" + startX + "-" + startY);
        Debug.Log("Dungeon read.");

        BuildDefaultRooms();
    }
}
