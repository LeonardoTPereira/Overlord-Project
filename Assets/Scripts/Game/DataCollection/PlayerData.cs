using System;
using System.Collections.Generic;
using System.IO;
using Game.LevelManager.DungeonLoader;
using Game.NarrativeGenerator;
using UnityEditor;
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
    public class PlayerData : ScriptableObject
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int PlayerId { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif 
        [field: SerializeField] public List<int> PreFormAnswers { get; set; }
        public DungeonDataByAttempt DungeonByAttempt { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif 
        [field: SerializeField] public int TotalAttempts { get; private set; }
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
        [field: SerializeField] public int NpcsInteracted { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalNpcs { get; private set; }
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
        [field: SerializeField] public int TotalTreasure { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TreasuresCollected { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int TotalLostHealth { get; private set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public int MaxCombo { get; private set; }
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
        [field: SerializeField] public PlayerProfile PlayerProfile { get; set; }
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreProperty]
#endif
        [field: SerializeField] public PlayerProfile GivenPlayerProfile { get; set; }
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
        public DungeonData CurrentDungeon { get; private set; }
        public string AssetPath { get; private set; }
        public string JsonPath { get; private set; }
        private int _currentCombo;

        public void Init()
        {
            PreFormAnswers = new List<int>();
            DungeonByAttempt = new DungeonDataByAttempt();
            PlayerId = RandomSingleton.GetInstance().Next(0, int.MaxValue);
            PlayerId += (int)Time.realtimeSinceStartup;
#if UNITY_EDITOR
            var jsonDirectory = Application.persistentDataPath + Constants.SeparatorCharacter + "PlayerData";
            if (!Directory.Exists(jsonDirectory))
            {
                Directory.CreateDirectory(jsonDirectory);
            }
            JsonPath = Application.persistentDataPath + Constants.SeparatorCharacter + "PlayerData" +
                       Constants.SeparatorCharacter + PlayerId;
            if (!Directory.Exists(jsonDirectory))
            {
                Directory.CreateDirectory(jsonDirectory);
            }
#endif
        }

        public void StartDungeon(string mapName, Map map)
        {
            CurrentDungeon = CreateInstance<DungeonData>();
            CurrentDungeon.Init(map, mapName, JsonPath);
            var dungeonAttempted = DungeonByAttempt.TryGetValue(mapName, out var currentDungeonList);
            if(dungeonAttempted)
            {
                currentDungeonList.Add(CurrentDungeon);
            }
            else
            {
                currentDungeonList = new List<DungeonData> {CurrentDungeon};
                DungeonByAttempt.Add(mapName, currentDungeonList);
            }
            AddTotalsForNewDungeon(map);
        }

        private void AddTotalsForNewDungeon(Map map)
        {
            TotalKeys += map.NKeys;
            TotalLocks += map.NLocks;
            TotalRooms += map.NRooms;
            TotalEnemies += map.NEnemies;
            TotalNpcs += map.NNPCs;
            TotalTreasure += map.TotalTreasure;
            TotalAttempts++;
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
            RoomsEntered += CurrentDungeon.RoomsEntered;
            UniqueRoomsEntered += CurrentDungeon.UniqueRoomsEntered;
            CurrentDungeon.IncrementDeaths();
        }
        
        public void IncrementWins()
        {
            TotalWins++;
            CurrentDungeon.IncrementWins();
        }

        public void AddPostTestDataToDungeon(List<int> answers)
        {
            CurrentDungeon.PostFormAnswers = answers;
#if UNITY_EDITOR
            SaveAndRefreshAssets();
            RefreshJson();
#endif
        }
        
#if UNITY_EDITOR
        public void SaveAndRefreshAssets()
        {
            EditorUtility.SetDirty(CurrentDungeon);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.SaveAssetIfDirty(CurrentDungeon);
            AssetDatabase.Refresh();
        }
#endif

        public void RefreshJson()
        {
            var playerFile = Application.persistentDataPath + Constants.SeparatorCharacter + "PlayerData" + Constants.SeparatorCharacter +
                             "PlayerData.json";
            string lines;
            using (var fileStream = new FileStream(playerFile, FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fileStream))
                {
                    lines = sr.ReadToEnd();
                }
            }

            using (var fileStream = new FileStream(playerFile, FileMode.Open))
            {
                using (var sw = new StreamWriter(fileStream))
                {
                    var lastIndexOfNewLine = lines.LastIndexOf(Environment.NewLine, StringComparison.Ordinal);
                    if (lastIndexOfNewLine != -1)
                    {
                        lines = lines.Remove(lastIndexOfNewLine);
                    }
                    sw.Write(lines);
                    sw.Write(JsonUtility.ToJson(this));
                }
            }
        }
        


        public void AddCollectedTreasure(int amount)
        {
            TreasuresCollected += amount;
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
    }
}