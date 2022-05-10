using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.LevelManager.DungeonLoader;
using Game.NarrativeGenerator;
using UnityEditor;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Game.DataCollection
{
    [Serializable]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public int PlayerId { get; set; }
        [field: SerializeField] public List<int> PreFormAnswers { get; set; }
        public DungeonDataByAttempt DungeonByAttempt { get; private set; }
        [field: SerializeField] public int TotalAttempts { get; private set; }
        [field: SerializeField] public int TotalDeaths { get; private set; }
        [field: SerializeField] public int TotalWins { get; private set; }
        [field: SerializeField] public int NpcsInteracted { get; private set; }
        [field: SerializeField] public int TotalNpcs { get; private set; }
        [field: SerializeField] public int TotalEnemies { get; private set; }
        [field: SerializeField] public int EnemiesKilled { get; private set; }
        [field: SerializeField] public int TotalTreasure { get; private set; }
        [field: SerializeField] public int TreasuresCollected { get; private set; }
        [field: SerializeField] public int TotalLostHealth { get; private set; }
        [field: SerializeField] public int MaxCombo { get; private set; }
        [field: SerializeField] public int KeysCollected { get; private set; }
        [field: SerializeField] public int TotalKeys { get; private set; }
        [field: SerializeField] public int LocksOpened { get; private set; }
        [field: SerializeField] public int TotalLocks { get; private set; }
        [field: SerializeField] public int TotalRooms { get; private set; }
        [field: SerializeField] public int UniqueRoomsEntered { get; private set; }
        [field: SerializeField] public int RoomsEntered { get; private set; }
        [field: SerializeField] public PlayerProfile PlayerProfile { get; set; }
        [field: SerializeField] public PlayerProfile GivenPlayerProfile { get; set; }
        public DungeonData CurrentDungeon { get; private set; }
        public string AssetPath { get; private set; }
        public string JsonPath { get; private set; }
        private int _currentCombo;

        public void Init()
        {
            PreFormAnswers = new List<int>();
            DungeonByAttempt = new DungeonDataByAttempt();
            PlayerId = Random.Range(0, int.MaxValue);
            PlayerId += (int)Time.realtimeSinceStartup;
            var target = "Assets";
            target += Constants.SEPARATOR_CHARACTER + "Resources";
            target += Constants.SEPARATOR_CHARACTER + "PlayerData";
#if UNITY_EDITOR
            var newFolder = PlayerId.ToString();
            if (!AssetDatabase.IsValidFolder(target + Constants.SEPARATOR_CHARACTER + newFolder))
            {
                AssetDatabase.CreateFolder(target, newFolder);
            }
            AssetPath = target + Constants.SEPARATOR_CHARACTER + newFolder;
            var fileName = AssetPath + Constants.SEPARATOR_CHARACTER + "PlayerData.asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();

            var jsonDirectory = Application.persistentDataPath + Constants.SEPARATOR_CHARACTER + "PlayerData";
            if (!Directory.Exists(jsonDirectory))
            {
                Directory.CreateDirectory(jsonDirectory);
            }
            JsonPath = Application.persistentDataPath + Constants.SEPARATOR_CHARACTER + "PlayerData" +
                       Constants.SEPARATOR_CHARACTER + PlayerId;
            if (!Directory.Exists(jsonDirectory))
            {
                Directory.CreateDirectory(jsonDirectory);
            }
#endif
        }

        public void StartDungeon(string mapName, Map map)
        {
            CurrentDungeon = CreateInstance<DungeonData>();
            CurrentDungeon.Init(map, mapName, AssetPath, JsonPath);
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
            CurrentDungeon.IncrementDeaths();
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
            var playerFile = Application.persistentDataPath + Constants.SEPARATOR_CHARACTER + "PlayerData" + Constants.SEPARATOR_CHARACTER +
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
                    if (lines != "")
                    {
                        lines = lines.Remove(lines.LastIndexOf(Environment.NewLine, StringComparison.Ordinal));
                        sw.Write(lines);
                    }
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