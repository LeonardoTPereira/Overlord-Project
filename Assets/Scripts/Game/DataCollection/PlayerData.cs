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
    public class PlayerData : ScriptableObject
    { 
	    public DungeonDataByAttempt DungeonByAttempt { get; private set; }
	    public DungeonData CurrentDungeon { get; private set; }
        public string AssetPath { get; private set; }
        public string JsonPath { get; private set; }
        private int _currentCombo;
        [field: SerializeField] public PlayerSerializedData SerializedData { get; set; }

        public void Init()
        {
	        SerializedData ??= new PlayerSerializedData
	        {
		        PreFormAnswers = new List<int>(),
		        PlayerId = RandomSingleton.GetInstance().Next(0, int.MaxValue) + (int) Time.realtimeSinceStartup,
		        PlayerProfile = new PlayerProfile(),
		        GivenPlayerProfile = new PlayerProfile()
	        };
	        DungeonByAttempt = new DungeonDataByAttempt();

#if UNITY_EDITOR
            /*var jsonDirectory = Application.dataPath + Constants.SeparatorCharacter + "PlayerData";
            if (!Directory.Exists(jsonDirectory))
            {
                Directory.CreateDirectory(jsonDirectory);
            }
            JsonPath = Application.dataPath + Constants.SeparatorCharacter + "PlayerData" +
                       Constants.SeparatorCharacter + PlayerId;
            if (!Directory.Exists(jsonDirectory))
            {
                Directory.CreateDirectory(jsonDirectory);
            }*/
#endif
        }

        public void StartDungeon(string mapName, Map map)
        {
            CurrentDungeon = CreateInstance<DungeonData>();
            CurrentDungeon.Init(map, mapName, JsonPath, SerializedData.PlayerId);
            var dungeonAttempted = DungeonByAttempt.TryGetValue(mapName, out var currentDungeonList);
            if(dungeonAttempted)
            {
                currentDungeonList.Add(CurrentDungeon);
                CurrentDungeon.TotalAttempts = currentDungeonList.Count;
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
	        SerializedData.TotalKeys += map.NKeys;
	        SerializedData.TotalLocks += map.NLocks;
            SerializedData.TotalRooms += map.NRooms;
            SerializedData.TotalEnemies += map.NEnemies;
            SerializedData.TotalNpcs += map.NNPCs;
            SerializedData.TotalTreasure += map.TotalTreasure;
            SerializedData.TotalAttempts++;
        }

        public void ResetCombo()
        {
            if (_currentCombo > SerializedData.MaxCombo)
	            SerializedData.MaxCombo = _currentCombo;
            _currentCombo = 0;
        }

        public void IncrementCombo()
        {
            _currentCombo++;
        }

        public void IncrementKills()
        {
	        SerializedData.EnemiesKilled++;
        }

        public void IncrementInteractionsWithNpcs()
        {
	        SerializedData.NpcsInteracted++;
        }
        
        public void IncrementDeaths()
        {
            SerializedData.TotalDeaths++;
            SerializedData.RoomsEntered += CurrentDungeon.RoomsEntered;
            SerializedData.UniqueRoomsEntered += CurrentDungeon.UniqueRoomsEntered;
            CurrentDungeon.IncrementDeaths();
        }
        
        public void IncrementWins()
        {
	        SerializedData.TotalWins++;
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
            var playerFile = Application.dataPath + Constants.SeparatorCharacter + "PlayerData" + Constants.SeparatorCharacter +
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
	        SerializedData.TreasuresCollected += amount;
        }

        public void AddLostHealth(int amount)
        {
	        SerializedData.TotalLostHealth += amount;
        }
        
        public void IncrementCollectedKeys()
        {
	        SerializedData.KeysCollected++;
        }
        
        public void IncrementOpenedLocks()
        {
	        SerializedData.LocksOpened++;
        }

        public void AddAttemptToDungeon()
        {
            CurrentDungeon.TotalAttempts++;
        }

        public object SaveState()
        {
	        return SerializedData;
        }

        public void LoadState(object state)
        {
	        Debug.Log("Loading Player Data");
	        var saveData = (PlayerSerializedData) state;
	        SerializedData.PlayerId = saveData.PlayerId;
	        SerializedData.PreFormAnswers = saveData.PreFormAnswers;
	        SerializedData.TotalAttempts = saveData.TotalAttempts;
	        SerializedData.TotalDeaths = saveData.TotalDeaths;
	        SerializedData.TotalWins = saveData.TotalWins;
	        SerializedData.NpcsInteracted = saveData.NpcsInteracted;
	        SerializedData.TotalNpcs = saveData.TotalNpcs;
	        SerializedData.TotalEnemies = saveData.TotalEnemies;
	        SerializedData.EnemiesKilled = saveData.EnemiesKilled;
	        SerializedData.TotalTreasure = saveData.TotalTreasure;
	        SerializedData.TreasuresCollected = saveData.TreasuresCollected;
	        SerializedData.TotalLostHealth = saveData.TotalLostHealth;
	        SerializedData.MaxCombo = saveData.MaxCombo;
	        SerializedData.KeysCollected = saveData.KeysCollected;
	        SerializedData.TotalKeys = saveData.TotalKeys;
	        SerializedData.LocksOpened = saveData.LocksOpened;
	        SerializedData.TotalLocks = saveData.TotalLocks;
	        SerializedData.TotalRooms = saveData.TotalRooms;
	        SerializedData.UniqueRoomsEntered = saveData.UniqueRoomsEntered;
	        SerializedData.RoomsEntered = saveData.RoomsEntered;
	        SerializedData.PlayerProfile = saveData.PlayerProfile;
	        SerializedData.GivenPlayerProfile = saveData.GivenPlayerProfile;
	        SerializedData.TotalQuests = saveData.TotalQuests;
	        SerializedData.CompletedQuests = saveData.CompletedQuests;
	        SerializedData.TotalAchievementQuests = saveData.TotalAchievementQuests;
	        SerializedData.CompletedAchievementQuests = saveData.CompletedAchievementQuests;
	        SerializedData.TotalCreativityQuests = saveData.TotalCreativityQuests;
	        SerializedData.CompletedCreativityQuests = saveData.CompletedCreativityQuests;
	        SerializedData.TotalImmersionQuests = saveData.TotalImmersionQuests;
	        SerializedData.CompletedImmersionQuests = saveData.CompletedImmersionQuests;
	        SerializedData.TotalMasteryQuests = saveData.TotalMasteryQuests;
	        SerializedData.CompletedMasteryQuests = saveData.CompletedMasteryQuests;
	        SerializedData.TotalExchangeQuests = saveData.TotalExchangeQuests;
	        SerializedData.CompletedExchangeQuests = saveData.CompletedExchangeQuests;
	        SerializedData.TotalGatherQuests = saveData.TotalGatherQuests;
	        SerializedData.CompletedGatherQuests = saveData.CompletedGatherQuests;
	        SerializedData.TotalExploreQuests = saveData.TotalExploreQuests;
	        SerializedData.CompletedExploreQuests = saveData.CompletedExploreQuests;
	        SerializedData.TotalGoToQuests = saveData.TotalGoToQuests;
	        SerializedData.CompletedGoToQuests = saveData.CompletedGoToQuests;
	        SerializedData.TotalGiveQuests = saveData.TotalGiveQuests;
	        SerializedData.CompletedGiveQuests = saveData.CompletedGiveQuests;
	        SerializedData.TotalListenQuests = saveData.TotalListenQuests;
	        SerializedData.CompletedListenQuests = saveData.CompletedListenQuests;
	        SerializedData.TotalReadQuests = saveData.TotalReadQuests;
	        SerializedData.CompletedReadQuests = saveData.CompletedReadQuests;
	        SerializedData.TotalReportQuests = saveData.TotalReportQuests;
	        SerializedData.CompletedReportQuests = saveData.CompletedReportQuests;
	        SerializedData.TotalDamageQuests = saveData.TotalDamageQuests;
	        SerializedData.CompletedDamageQuests = saveData.CompletedDamageQuests;
	        SerializedData.TotalKillQuests = saveData.TotalKillQuests;
	        SerializedData.CompletedKillQuests = saveData.CompletedKillQuests;
        }
        
#if !UNITY_WEBGL || UNITY_EDITOR
        [FirestoreData]
#endif
        [Serializable]
        public class PlayerSerializedData
        {
			#if !UNITY_WEBGL || UNITY_EDITOR
				    [FirestoreProperty]
			#endif
				    [field: SerializeField] public int PlayerId { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
				    [FirestoreProperty]
			#endif 
				    [field: SerializeField] public List<int> PreFormAnswers { get; set; }
	        #if !UNITY_WEBGL || UNITY_EDITOR
					[FirestoreProperty]
			#endif 
			        [field: SerializeField] public int TotalAttempts { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif 
			        [field: SerializeField] public int TotalDeaths { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int TotalWins { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int NpcsInteracted { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int TotalNpcs { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int TotalEnemies { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int EnemiesKilled { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int TotalTreasure { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int TreasuresCollected { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int TotalLostHealth { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int MaxCombo { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int KeysCollected { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int TotalKeys { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int LocksOpened { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int TotalLocks { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int TotalRooms { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int UniqueRoomsEntered { get; set; }
			#if !UNITY_WEBGL || UNITY_EDITOR
			        [FirestoreProperty]
			#endif
			        [field: SerializeField] public int RoomsEntered { get; set; }
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
        }
    }
}