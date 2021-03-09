using System.Collections.Generic;

public class MapFileHandler
{
    private readonly string [] parsedMapFile;
    private int currentIndex;

    public MapFileHandler(string mapFile)
    {
        parsedMapFile = SplitMapFileInLines(mapFile);
        currentIndex = 2;
    }

    public string[] SplitMapFileInLines(string mapFile)
    {
        string[] splitFile = new string[] { "\r\n", "\r", "\n" };
        return mapFile.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
    }

    public Dimensions GetMapDimensions()
    {
        int width, height;
        width = int.Parse(parsedMapFile[0]);
        height = int.Parse(parsedMapFile[1]);
        return new Dimensions(width, height);
    }
    public Coordinates GetNextDungeonPartCoordinates()
    {
        int xCoordinate, yCoordinate;
        xCoordinate = int.Parse(parsedMapFile[currentIndex++]);
        yCoordinate = int.Parse(parsedMapFile[currentIndex++]);
        return new Coordinates(xCoordinate, yCoordinate);
    }

    public string GetNextDungeonPartCode()
    {
        return parsedMapFile[currentIndex++];
    }

    public List<int> GetNextLockIDs()
    {
        string code;
        bool currentCodeIsALock;
        currentIndex--;
        List<int> lockIDs = new List<int>();
        do
        {
            code = parsedMapFile[currentIndex++];
            currentCodeIsALock = IsLock(code);
            if(currentCodeIsALock)
                lockIDs.Add(-int.Parse(code));
        } while (currentCodeIsALock && HasMoreLines());
        currentIndex--;
        return lockIDs;
    }

    private bool IsLock(string code)
    {
        return (code[0] == '-');
    }

    private bool IsKey(string code)
    {
        return (code[0] == '+');
    }

    public bool HasMoreLines()
    {
        return (currentIndex < (parsedMapFile.Length - 1));
    }

    public int GetNextDifficulty()
    {
        return int.Parse(parsedMapFile[currentIndex++]);
    }

    public int GetNextTreasure()
    {
        return int.Parse(parsedMapFile[currentIndex++]);
    }

    public List<int> GetNextKeyIDs()
    {
        string code;
        bool currentCodeIsAKey;
        List<int> keyIDs = new List<int>();
        do
        {
            code = parsedMapFile[currentIndex++];
            currentCodeIsAKey = IsKey(code);
            if (currentCodeIsAKey)
                keyIDs.Add(-int.Parse(code));
        } while (currentCodeIsAKey && HasMoreLines());
        currentIndex--;
        return keyIDs;
    }
}
