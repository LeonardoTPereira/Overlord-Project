using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.EnemyGenerator;
using Game.GameManager;
using Game.LevelManager;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Game.ExperimentControllers
{
    public class ArenaController : MonoBehaviour
    {
        [field: SerializeField] public DifficultyLevels Difficulty { get; set; }
        [field: SerializeField] public int TotalEnemies { get; set; }
        [field: SerializeField] public RoomBhv RoomPrefab { get; set; }
        [field: SerializeField] public Dimensions RoomSize { get; set; }
        [field: Scene] public string GameUI { get; set; } = "GameUI";

        private void Start()
        {
            var enemyGenerator = GetComponent<EnemyGeneratorManager>();
            var enemies = enemyGenerator.EvolveEnemies(Difficulty);
            EnemyLoader.LoadEnemies(enemies);
            var dungeonRoom = new DungeonRoom(new Coordinates(0, 0), Constants.RoomTypeString.Normal, new List<int>(), 0, TotalEnemies, 0);
            dungeonRoom.EnemiesByType = new EnemiesByType();
            dungeonRoom.EnemiesByType.EnemiesByTypeDictionary = CreateDictionaryOfRandomEnemies(enemies);
            dungeonRoom.CreateRoom(RoomSize);
            var room = RoomLoader.InstantiateRoom(dungeonRoom, RoomPrefab);
            SceneManager.LoadSceneAsync(GameUI, LoadSceneMode.Additive);
            StartCoroutine(SpawnEnemies(room));
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