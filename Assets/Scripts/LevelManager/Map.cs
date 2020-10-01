using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LevelGenerator;

public class Map {

    private static class MapFileType
    {
        public const int ONE_KEY_ONE_DOOR = 0;
        public const int N_KEYS_N_DOORS = 1;
    }

    public static int sizeX;
	public static int sizeY;
    public static int index;

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
	public Map(string text, string roomsFilePath = null, int mode = 0){
		ReadMapFile (text, mode); // lê o mapa global
        if (roomsFilePath != null){ // dá a opção de gerar o mapa com ou sem os tiles
            //Debug.Log("Has Room File");
            ReadRoomsFile (roomsFilePath); // lê cada sala, com seus tiles
			Room.tiled = true; // o arquivo de tiles das salas foi lido; função de tiles ativada
		} else { // sala vazia padrão
            //Debug.Log("Doesn't Have Room File");
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
    /**
     * Reads the Map File Generated from the EA by the "PrintNumericalGridWithConnections" method
     * Not to be confused with the Map constructor that "reads" the created dungeon in real-time by the EA
     */

    private void ReadRoomDataFromMapFile()
    {
        GameManager.instance.maxRooms += 1;
        switch (currentRoomCode)
        {
            case "s": //This is the starting room
                startX = currentRoomXPos;
                startY = currentRoomYPos;
                break;
            case "B": //This room has the final item
                endX = currentRoomXPos;
                endY = currentRoomYPos;
                break;
            case "T": //This room has a treasure
                      //"gambiarra" to give a predefined enemy difficulty for the room and always place the most valuable treasure
                rooms[currentRoomXPos, currentRoomYPos].difficulty = GameManager.instance.dungeonDifficulty;
                rooms[currentRoomXPos, currentRoomYPos].Treasure = GameManager.instance.treasureSet.Items.Count - 1;
                GameManager.instance.maxTreasure += 50;
                break;
            default: //This is an empty room
                rooms[currentRoomXPos, currentRoomYPos].keyID.Add(int.Parse(currentRoomCode));
                //"gambiarra" to give a predefined enemy difficulty for the room
                rooms[currentRoomXPos, currentRoomYPos].difficulty = GameManager.instance.dungeonDifficulty;
                break;
        }
    }
    private void CheckIfStartOrFinishRoom()
    {
        switch (currentRoomCode)
        {
            case "s": //This is the starting room
                startX = currentRoomXPos;
                startY = currentRoomYPos;
                currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];
                break;
            case "B": //This room has the final item
                endX = currentRoomXPos;
                endY = currentRoomYPos;
                currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];
                break;
            default: //This is an empty room
                break;
        }
    }


    private void ReadCompleteRoomDataFromMapFile()
    {
        GameManager.instance.maxRooms += 1;
        bool roomHasKey;

        CheckIfStartOrFinishRoom();

        rooms[currentRoomXPos, currentRoomYPos].difficulty = int.Parse(currentRoomCode) *(GameManager.instance.dungeonDifficulty/3);
        currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];
        Debug.Log("Enemies: " + rooms[currentRoomXPos, currentRoomYPos].difficulty);
        rooms[currentRoomXPos, currentRoomYPos].Treasure = Mathf.Min(int.Parse(currentRoomCode) -1, 4);
        Debug.Log("Treasures: " + rooms[currentRoomXPos, currentRoomYPos].Treasure);
        if (currentMapFileLineIndex < (currentMapParsedFile.Length - 1))
        {
            currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];

            roomHasKey = (currentRoomCode[0] == '+') ? true : false;
            if (roomHasKey)
                rooms[currentRoomXPos, currentRoomYPos].keyID.Clear();
            while (roomHasKey && (currentMapFileLineIndex < (currentMapParsedFile.Length - 1)))
            {
                Debug.Log("Key: " + int.Parse(currentRoomCode));
                rooms[currentRoomXPos, currentRoomYPos].keyID.Add(int.Parse(currentRoomCode));
                currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];
                roomHasKey = (currentRoomCode[0] == '+') ? true : false;
            }
            currentMapFileLineIndex--;
        }
   }

    private void ReadCorridorDataFromMapFile()
    {
        int lockID;
        bool roomHasLock;
        if (currentRoomCode != "c")
        {
            //As in Breno's case, a lock can have many keys to open it, so we read until a positive number (not lock id) is found
            //But this is not yet available in the "PrintNumericalGridWithConnections" method. Only with the files he provided us
            rooms[currentRoomXPos, currentRoomYPos].lockID.Clear();

            roomHasLock = true;
            while (roomHasLock && (currentMapFileLineIndex < (currentMapParsedFile.Length - 1)))
            {
                Debug.Log("Lock: " + int.Parse(currentRoomCode));
                rooms[currentRoomXPos, currentRoomYPos].lockID.Add(-int.Parse(currentRoomCode));
                currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];
                roomHasLock = (currentRoomCode[0] == '-') ? true : false;
            }
            currentMapFileLineIndex--;
        }
    }

    private void InitializeMapFileReader()
    {
        currentMapFileLineIndex = 0;
        GameManager.instance.maxTreasure = 0;
        GameManager.instance.maxRooms = 0;
        string[] splitFile = new string[] { "\r\n", "\r", "\n" };

        //Split the file in lines. A readLine method could also be used
        currentMapParsedFile = currentMapFile.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);

        //The first two lines are the matrix sizes
        sizeX = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
        sizeY = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
        //currentMapFileLineIndex++;
        //Create a Room grid with the sizes read
        rooms = new Room[sizeX, sizeY];
    }

    private void ReadOneKeyOneDoorMapFile(){

        InitializeMapFileReader();
        //Read all the other lines in the file
        while(currentMapFileLineIndex < currentMapParsedFile.Length)
        {
            //For now, every room/corridor has its x and y coordinates and code (identifies as a normal room, normal corridor, locked corridor, room with key, room with treasure, etc.
            //TODO: change so the code now has up to 3 lines (if it is a room only): the first for the possible key in the room, the second for the possible treasure, and the third for the possible enemy difficulty
            currentRoomXPos = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
            currentRoomYPos = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
            currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];

            rooms[currentRoomXPos, currentRoomYPos] = new Room(currentRoomXPos, currentRoomYPos);
            //Sala ou corredor(link)?
            if (((currentRoomXPos % 2) + (currentRoomYPos % 2)) == 0)
            { // ambos pares: sala
                ReadRoomDataFromMapFile();
            }
            else
            { //if not corridor, is a locked corridor
                ReadCorridorDataFromMapFile();
            }
            //currentMapFileLineIndex++; //There is a dashed line before the info
        }
    }

    private void ReadNKeysNDoorsMapFile()
    {
        InitializeMapFileReader();
        //Read all the other lines in the file
        while (currentMapFileLineIndex < (currentMapParsedFile.Length-1))
        {
            //For now, every room/corridor has its x and y coordinates and code (identifies as a normal room, normal corridor, locked corridor, room with key, room with treasure, etc.
            //TODO: change so the code now has up to 3 lines (if it is a room only): the first for the possible key in the room, the second for the possible treasure, and the third for the possible enemy difficulty
            currentRoomXPos = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
            Debug.Log("Room X Pos: " + currentRoomXPos);
            currentRoomYPos = int.Parse(currentMapParsedFile[currentMapFileLineIndex++]);
            Debug.Log("Room Y Pos: " + currentRoomYPos);
            currentRoomCode = currentMapParsedFile[currentMapFileLineIndex++];

            rooms[currentRoomXPos, currentRoomYPos] = new Room(currentRoomXPos, currentRoomYPos);
            //Sala ou corredor(link)?
            if (((currentRoomXPos % 2) + (currentRoomYPos % 2)) == 0)
            { // ambos pares: sala
                
                ReadCompleteRoomDataFromMapFile();
            }
            else
            { //if not corridor, is a locked corridor
                ReadCorridorDataFromMapFile();
            }
            //currentMapFileLineIndex++;
        }
    }
	//Recebe os dados de tiles das salas
	private void ReadRoomsFile(string text){
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
			rooms [roomX, roomY].InitializeTiles (); // aloca memória para os tiles
			for (int x = 0; x < Room.sizeX; x++){
                for (int y = 0; y < Room.sizeY; y++)
                {
                    rooms[roomX, roomY].tiles [x, y] = int.Parse(NameLines[i++]); // FIXME Desinverter x e y: foi feito assim pois o arquivo de entrada foi passado em um formato invertido
					txtLine++;
				}
			}
		}
		Debug.Log ("Rooms read.");
	}

    //Cria salas vazias no tamanho padrão
    private void BuildDefaultRooms ()
    {
        Room.sizeX = Util.defaultRoomSizeX;
        Room.sizeY = Util.defaultRoomSizeY;
        for (int roomX = 0; roomX < sizeX; roomX+=2)
        {
            for (int roomY = 0; roomY < sizeY; roomY+=2)
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
        sizeX = 2*(maxX - minX + 1);
        sizeY = 2*(maxY - minY + 1);

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
                        if(actualRoom.IsLeafNode())
                        {
                            rooms[iPositive * 2, jPositive * 2].Treasure = Random.Range(0, GameManager.instance.treasureSet.Items.Count);
                            Debug.Log("This is a Leaf Node Room! "+ (iPositive * 2) + " - " + (jPositive * 2));
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
