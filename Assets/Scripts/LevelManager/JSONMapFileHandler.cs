using Newtonsoft.Json;
using UnityEngine;

public class JSONMapFileHandler
{
    private DungeonFile dungeonFile;
    private int currentIndex;

    public JSONMapFileHandler(string mapFile)
    {
        Debug.Log("File: " + mapFile);
        string jsonContent = Resources.Load<TextAsset>("Levels/" + mapFile).text;
        Debug.Log("Content: " + jsonContent);
        dungeonFile = JsonConvert.DeserializeObject<DungeonFile>(jsonContent);
        currentIndex = 0;
        Debug.Log(dungeonFile.rooms.Count);
    }

    public Dimensions GetDimensions()
    {
        return dungeonFile.dimensions;
    }

    public DungeonPart GetNextPart()
    {
        if(currentIndex < dungeonFile.rooms.Count)
            return DungeonPartFactory.CreateDungeonPartFromDungeonFileJSON(dungeonFile.rooms[currentIndex++]);
        else
            return null;
    }
}
