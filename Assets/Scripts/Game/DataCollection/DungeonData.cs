using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.LevelManager.DungeonLoader;
using Game.NarrativeGenerator;
using UnityEngine;
using Util;
#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
#endif

namespace Game.DataCollection
{
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreData]
#endif
    [Serializable]
    public class DungeonData : ScriptableObject
    {
#if !UNITY_WEBGL || UNITY_EDITOR
            [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalAttempts { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public string LevelName { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public string WeaponName { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalDeaths { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalWins { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int KeysCollected { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalKeys { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int LocksOpened { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalLocks { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalRooms { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int UniqueRoomsEntered { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int RoomsEntered { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalLostHealth { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalEnemies { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int EnemiesKilled { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalNpcs { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int NpcsInteracted { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalTreasure { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TreasuresCollected { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int MaxCombo { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public List<int> PostFormAnswers { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int[,] HeatMap { get; set; }
        public RoomDataByVisit VisitedRooms { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public float TimeToFinish { get; private set; }
        
        #region //Quest related data
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalAchievementQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedAchievementQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalCreativityQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedCreativityQuests { get; set; } 
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalImmersionQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedImmersionQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalMasteryQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedMasteryQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalExchangeQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedExchangeQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalGatherQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedGatherQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalExploreQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedExploreQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalGoToQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedGoToQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalGiveQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedGiveQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalListenQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedListenQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalReadQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedReadQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalReportQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedReportQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalDamageQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedDamageQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalKillQuests { get; set; }        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int CompletedKillQuests { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        #endregion
        [field: SerializeField] public int PlayerId { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public PlayerProfile InputProfile { get; set; }

        private float _startTime;
        private int _currentCombo;
        private RoomData _currentRoom;
        private string _jsonPath;
        public void Init(Map map, string mapName, string jsonPath, int playerId)
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
            _jsonPath = jsonPath;
            PlayerId = playerId;
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
            //CreateJson();
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
        
        public void CreateJson()
        {
            var dungeonFolder = _jsonPath + Constants.SeparatorCharacter + LevelName;
            if (!Directory.Exists(dungeonFolder))
            {
                Directory.CreateDirectory(dungeonFolder);
            }
            var dungeonFile = dungeonFolder + Constants.SeparatorCharacter +
                              "DungeonData.json";
            var roomFile = dungeonFolder + Constants.SeparatorCharacter;
            const string roomFileEnding = "RoomData.json";
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
            
            var dungeonData = JsonUtility.ToJson(lines);
            dungeonData+=JsonUtility.ToJson(this);
            var dungeonString = JsonUtility.ToJson(dungeonData);
            
            using (var fileStream = new FileStream(dungeonFile, FileMode.OpenOrCreate))
            {
                using (var sw = new StreamWriter(fileStream)) 
                {
                    sw.Write(dungeonString); 
                }
            }

            var roomData = VisitedRooms.SelectMany(roomList => roomList.Value).ToList();
            var roomString = JsonUtility.ToJson(roomData);
            using (var fileStream = new FileStream(roomFile, FileMode.OpenOrCreate))
            {
                    using (var sw = new StreamWriter(fileStream)) 
                    {
                            sw.Write(roomString); 
                    }
            }
        }
    }
}