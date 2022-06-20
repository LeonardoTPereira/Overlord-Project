using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.LevelManager.DungeonLoader;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Util;

namespace Game.DataCollection
{
    [Serializable]
    public class DungeonData : ScriptableObject
    {
        [field: SerializeField] public int TotalAttempts { get; set; }
        [field: SerializeField] public string LevelName { get; private set; }
        [field: SerializeField] public string WeaponName { get; private set; }
        [field: SerializeField] public int TotalDeaths { get; private set; }
        [field: SerializeField] public int TotalWins { get; private set; }
        [field: SerializeField] public int KeysCollected { get; private set; }
        [field: SerializeField] public int TotalKeys { get; private set; }
        [field: SerializeField] public int LocksOpened { get; private set; }
        [field: SerializeField] public int TotalLocks { get; private set; }
        [field: SerializeField] public int TotalRooms { get; private set; }
        [field: SerializeField] public int UniqueRoomsEntered { get; private set; }
        [field: SerializeField] public int RoomsEntered { get; private set; }
        [field: SerializeField] public int TotalLostHealth { get; private set; }
        [field: SerializeField] public int TotalEnemies { get; private set; }
        [field: SerializeField] public int EnemiesKilled { get; private set; }
        [field: SerializeField] public int TotalNpcs { get; private set; }
        [field: SerializeField] public int NpcsInteracted { get; private set; }
        [field: SerializeField] public int TotalTreasure { get; private set; }
        [field: SerializeField] public int TreasuresCollected { get; private set; }
        [field: SerializeField] public int MaxCombo { get; private set; }
        [field: SerializeField] public List<int> PostFormAnswers { get; set; }
        [field: SerializeField] public int[,] HeatMap { get; set; }
        public RoomDataByVisit VisitedRooms { get; set; }
        [field: SerializeField] public float TimeToFinish { get; private set; }
        private float _startTime;
        private int _currentCombo;
        private RoomData _currentRoom;
        private string _assetPath;
        private string _jsonPath;
        public void Init(Map map, string mapName, string assetPath, string jsonPath)
        {
            PostFormAnswers = new List<int>();
            VisitedRooms = new RoomDataByVisit();
            LevelName = mapName;
            TotalKeys = map.NKeys;
            TotalLocks = map.NLocks;
            TotalRooms = map.NRooms;
            TotalEnemies = map.NEnemies;
            TotalNpcs = map.NNPCs;
            TotalTreasure = map.TotalTreasure;
            HeatMap = CreateHeatMap(map);
            TotalAttempts++;
            _startTime = Time.realtimeSinceStartup;
            _assetPath = assetPath;
            _jsonPath = jsonPath;
        }

        public void OnPlayerDeath()
        {
            TotalDeaths++;
            EndTheLevel();
        }
        
        public void OnPlayerVictory()
        {
            TotalWins++;
            EndTheLevel();
        }

        public void EndTheLevel()
        {
            _currentRoom.ExitRoom();
            TimeToFinish = Time.realtimeSinceStartup - _startTime;
#if UNITY_EDITOR
            CreateAsset();
            CreateJson();
#endif
        }

        public void ResetCombo()
        {
            if (_currentCombo > MaxCombo)
                MaxCombo = _currentCombo;
            _currentCombo = 0;
        }

        public void IncrementCombo()
        {
            _currentCombo++;
        }

        public void IncrementKills()
        {
            EnemiesKilled++;
        }

        public void IncrementInteractionsWithNpcs()
        {
            NpcsInteracted++;
        }
        
        public void IncrementDeaths()
        {
            TotalDeaths++;
        }
        
        public void IncrementWins()
        {
            TotalWins++;
        }

        public void AddCollectedTreasure(int amount)
        {
            TreasuresCollected += amount;
        }

        public void AddTotalEnemies(int amount)
        {
            TotalEnemies += amount;
        }
        
        public void AddTotalNpcs(int amount)
        {
            TotalNpcs += amount;
        }
        
        public void AddTotalTreasures(int amount)
        {
            TotalTreasure += amount;
        }

        public void AddLostHealth(int amount)
        {
            TotalLostHealth += amount;
        }
        
        public void IncrementCollectedKeys()
        {
            KeysCollected++;
        }
        
        public void IncrementOpenedLocks()
        {
            LocksOpened++;
        }

        public void OnRoomEnter(RoomData roomData)
        {
            RoomsEntered++;
            _currentRoom = roomData;
            var roomId = roomData.GetRoomIdFromCoordinates(_currentRoom.RoomCoordinates);
            var roomAlreadyVisited = VisitedRooms.TryGetValue(roomId, out var currentRoomDataList);
            if(roomAlreadyVisited)
            {
                currentRoomDataList.Add(_currentRoom);
            }
            else
            {
                currentRoomDataList = new List<RoomData> {_currentRoom};
                VisitedRooms.Add(roomId, currentRoomDataList);
                UniqueRoomsEntered++;
            }
            HeatMap[roomData.RoomCoordinates.X / 2, roomData.RoomCoordinates.Y / 2]++;
        }

        public void OnRoomExit()
        {
            _currentRoom.ExitRoom();
        }
        
        private int[,] CreateHeatMap(Map currentMap)
        {
            var heatMap = new int[(currentMap.Dimensions.Width + 1) / 2, (currentMap.Dimensions.Height + 1) / 2];
            for (var i = 0; i < currentMap.Dimensions.Width / 2; ++i)
            {
                for (var j = 0; j < currentMap.Dimensions.Height / 2; ++j)
                {
                    if (currentMap.DungeonPartByCoordinates.ContainsKey(new Coordinates(i * 2, j * 2)))
                    {
                        heatMap[i, j] = 0;
                    }
                    else
                    {
                        heatMap[i, j] = -1;
                    }
                }
            }
            return heatMap;
        }
        
#if UNITY_EDITOR
        public void CreateAsset()
        {
            var target = _assetPath;
            var newFolder = LevelName;
            if (!AssetDatabase.IsValidFolder(target + Constants.SEPARATOR_CHARACTER + newFolder))
            {
                AssetDatabase.CreateFolder(target, newFolder);
            }
            else
            {
                var existingFolderCounter = 1;
                while (AssetDatabase.IsValidFolder(target + Constants.SEPARATOR_CHARACTER + newFolder+existingFolderCounter))
                {
                    existingFolderCounter++;
                }

                newFolder += existingFolderCounter;
                AssetDatabase.CreateFolder(target, newFolder);
            }
            target += Constants.SEPARATOR_CHARACTER + newFolder;
            AssetDatabase.CreateFolder(target, "Rooms");
            var roomsFolder = target + Constants.SEPARATOR_CHARACTER + "Rooms";
            foreach (var roomList in VisitedRooms.Select(kvp => kvp.Value))
            {
                for (var i = 0; i < roomList.Count; ++i)
                {
                    roomList[i].CreateAsset(roomsFolder, i);
                }
            }
            var fileName = target + Constants.SEPARATOR_CHARACTER + "DungeonData.asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
#endif
        public void CreateJson()
        {
            var dungeonFolder = _jsonPath + Constants.SEPARATOR_CHARACTER + LevelName;
            if (!Directory.Exists(dungeonFolder))
            {
                Directory.CreateDirectory(dungeonFolder);
            }
            var dungeonFile = dungeonFolder + Constants.SEPARATOR_CHARACTER +
                              "DungeonData.json";
            var roomFile = dungeonFolder + Constants.SEPARATOR_CHARACTER;
            var roomFileEnding = "RoomData.json";
            var roomFileCounter = 0;
            if (File.Exists(roomFile+roomFileEnding))
            {
                roomFileCounter++;
                while (File.Exists(roomFile + roomFileCounter + roomFileEnding))
                {
                    roomFileCounter++;
                }
                roomFile += roomFileCounter + roomFileEnding;
            }
            else
            {
                roomFile += roomFileEnding;
            }
            string lines;
            using (var fileStream = new FileStream(dungeonFile, FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fileStream))
                {
                    lines = sr.ReadToEnd();
                }
            }

            using (var fileStream = new FileStream(dungeonFile, FileMode.Open))
            {
                using (var sw = new StreamWriter(fileStream))
                {
                    if (lines != "")
                    {
                        lines = lines.Remove(lines.LastIndexOf(Environment.NewLine, StringComparison.Ordinal));
                        sw.Write(lines);
                    }
                    sw.Write(JsonUtility.ToJson(this));
                }
            }
            

            var stringBuilder = new StringBuilder();
            foreach (var room in VisitedRooms.SelectMany(roomList => roomList.Value))
            {
                stringBuilder.Append(JsonUtility.ToJson(room));
            }
            var roomJson = stringBuilder.ToString();
            File.WriteAllText(roomFile, roomJson);
        }
    }
}