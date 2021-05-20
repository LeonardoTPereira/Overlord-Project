using System.Collections.Generic;
using UnityEngine;

public class MapFileHandler
{
    private readonly string [] parsedMapFile;
    private int currentIndex;

    class parametersDungeon{
        public
            int size = 0, linearity = 0, nKeys = 0, enemyType = -1;
    };

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
#if UNITY_EDITOR
        Debug.Log($"Map Dimensions: Width {width}, Height {height}");
#endif
        return new Dimensions(width, height);
    }
    public Coordinates GetNextDungeonPartCoordinates()
    {
        int xCoordinate, yCoordinate;
        xCoordinate = int.Parse(parsedMapFile[currentIndex++]);
        yCoordinate = int.Parse(parsedMapFile[currentIndex++]);
#if UNITY_EDITOR
        Debug.Log($"Coordinates: X {xCoordinate}, Y {yCoordinate}");
#endif
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
        if (!currentCodeIsALock)
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
        return (currentIndex < (parsedMapFile.Length));
    }

    public int GetNextDifficulty()
    {
        return int.Parse(parsedMapFile[currentIndex++]);
    }

    public int GetNextTreasure()
    {
        return int.Parse(parsedMapFile[currentIndex++])-1;
    }

    public int GetNextEnemyType(){
        return int.Parse(parsedMapFile[currentIndex++])-2;
    }

    public List<int> GetNextKeyIDs()
    {
        string code;
        bool currentCodeIsAKey;
        List<int> keyIDs = new List<int>();
        if (HasMoreLines())
        {
            do
            {
                code = parsedMapFile[currentIndex++];
                currentCodeIsAKey = IsKey(code);
                if (currentCodeIsAKey)
                    keyIDs.Add(int.Parse(code));
            } while (currentCodeIsAKey && HasMoreLines());
            if(!currentCodeIsAKey)
                currentIndex--;
        }
        return keyIDs;
    }

    public bool IsStart(string code)
    {
        if (code[0] == 's')
        {
            Debug.Log($"We have a starting room: {code}");
            return true;
        }
        return false;
    }

    public bool IsFinal(string code)
    {
        if (code[0] == 'B')
        {
            Debug.Log($"We have a final room: {code}");
            return true;
        }
        return false;
    }
}
