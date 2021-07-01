using Game.LevelManager;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.LevelManager
{
    public class JsonMapFileHandler
    {
        private readonly DungeonFile dungeonFile;
        private int currentIndex;

        public JsonMapFileHandler(string mapFile)
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
            if (currentIndex < dungeonFile.rooms.Count)
                return DungeonPartFactory.CreateDungeonPartFromDungeonFileJSON(dungeonFile.rooms[currentIndex++]);
            else
                return null;
        }
    }
}