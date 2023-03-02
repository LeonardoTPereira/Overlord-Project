using System;
using System.Collections;
using System.Collections.Generic;
using Game.Dialogues;
using Game.EnemyGenerator;
using Game.Events;
using Game.GameManager;
using Game.LevelManager;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Random = System.Random;

namespace Game.ExperimentControllers
{
    public class ArenaController : MonoBehaviour
    {
        [field: SerializeField] public DifficultyLevels Difficulty { get; set; }
        [field: SerializeField] public int TotalEnemies { get; set; }
        [field: SerializeField] public RoomBhv RoomPrefab { get; set; }
        [field: SerializeField] public Dimensions RoomSize { get; set; }
        [field: SerializeField] public List<int> Keys { get; set; }
        [field: SerializeField] public List<NpcSo> ArenaNpcs { get; set; }
        [field: Scene, SerializeField] public string GameUI { get; set; } = "ArenaUI";
        [field: SerializeField] public ItemAmountDictionary Items { get; set; }

        public static event ShowRoomOnMiniMapEvent ShowRoomOnMiniMapEventHandler;

        private void OnEnable()
        {
            TaggedDialogueHandler.MarkRoomOnMiniMapEventHandler += MarkRoom;
        }
        
        private void OnDisable()
        {
            TaggedDialogueHandler.MarkRoomOnMiniMapEventHandler -= MarkRoom;
        }

        private void MarkRoom(object sender, MarkRoomOnMinimapEventArgs e)
        {
            ShowRoomOnMiniMapEventHandler?.Invoke(this, new ShowRoomOnMiniMapEventArgs(new Vector3(100, 120, 0)));
        }

        private void Start()
        {
            var enemyGenerator = GetComponent<EnemyGeneratorManager>();
            var enemies = enemyGenerator.EvolveEnemies(Difficulty);
            EnemyLoader.LoadEnemies(enemies);

            var dungeonRoom = new DungeonRoom(new Coordinates(0, 0), Constants.RoomTypeString.Start, Keys, 0, TotalEnemies, 0)
                {
                    EnemiesByType = new EnemiesByType
                    {
                        EnemiesByTypeDictionary = CreateDictionaryOfRandomEnemies(enemies)
                    },
                    Items = new ItemsAmount
                    {
                        ItemAmountBySo = Items
                    },
                    Npcs = ArenaNpcs
                };

            dungeonRoom.CreateRoom(RoomSize);
            
            var room = RoomLoader.InstantiateRoom(dungeonRoom, RoomPrefab, Enums.GameType.TopDown);
            
            var theme = RandomSingleton.GetInstance().Next(0, (int)Enums.RoomThemeEnum.Count);
            room.SetTheme((Enums.RoomThemeEnum) theme);
            LoadGameUI();
            StartCoroutine(SpawnEnemies(room));
        }

        protected virtual DungeonRoom InstantiateDungeonRoom()
        {
            return new DungeonRoom(new Coordinates(0, 0), Constants.RoomTypeString.Normal, new List<int>(), 0, TotalEnemies, 0);
        }

        protected virtual void LoadGameUI()
        {
            SceneManager.LoadSceneAsync(GameUI, LoadSceneMode.Additive);
        }

        private WeaponTypeAmountDictionary CreateDictionaryOfRandomEnemies(List<EnemySO> enemies)
        {
            var weaponDictionary = new WeaponTypeAmountDictionary();
            for (int i = 0; i < TotalEnemies; i++)
            {                
                QuestIdList questIdList;
                var enemy = enemies.GetRandom();
                if (!weaponDictionary.ContainsKey(enemy.weapon))
                {
                    questIdList = new QuestIdList();
                    weaponDictionary.Add(enemy.weapon, questIdList);
                }
                weaponDictionary[enemy.weapon].Add(0);
            }
            return weaponDictionary;
        }

        private IEnumerator SpawnEnemies(RoomBhv room)
        {
            yield return null;
            room.SpawnEnemies();
        }
    }
    
    
}