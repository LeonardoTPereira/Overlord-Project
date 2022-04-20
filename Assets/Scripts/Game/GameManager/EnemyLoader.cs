using System;
using System.Collections.Generic;
using System.Linq;
using Game.EnemyManager;
using Game.LevelManager;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using Game.Maestro;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using MyBox;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using Util;
using static Util.Enums;
using Random = UnityEngine.Random;

namespace Game.GameManager
{
    [Serializable]
    public class EnemyLoader : MonoBehaviour
    {
        private static List<EnemySO> enemyListForCurrentDungeon;
        
        public static EnemySO[] arena;
        public GameObject enemyPrefab;
        public GameObject barehandEnemyPrefab;
        public GameObject shooterEnemyPrefab;
        public GameObject bomberEnemyPrefab;
        public GameObject healerEnemyPrefab;
        [MustBeAssigned] [field: SerializeField] private WeaponTypeRuntimeSetSO _weaponTypes;

        
        public static void DistributeEnemiesInDungeon(Map map, QuestLine questLine)
        {
            var enemiesInQuestByType = new EnemiesByType(questLine.EnemyParametersForQuestLine.TotalByType);
            
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (dungeonPart.Value is DungeonRoom dungeonRoom && !dungeonRoom.IsStartRoom())
                {
                    dungeonRoom.EnemiesByType = EnemySelector.Select(dungeonRoom, ref enemiesInQuestByType);
                }
            }
        }

        private static EnemiesByType SelectWeaponTypesForRoom(DungeonRoom dungeonRoom, EnemiesByType enemiesInQuestByType)
        {
            var enemiesByType = new EnemiesByType();
            var enemiesInRoom = dungeonRoom.TotalEnemies;
            var selectedEnemies = 0;
            while (selectedEnemies < enemiesInRoom)
            {
                var selectedType = enemiesInQuestByType.GetRandom();
                var maxPossibleNewEnemies = math.min(selectedType.Value, enemiesInRoom - selectedEnemies);
                var newEnemies = RandomSingleton.GetInstance().Random.Next(1, maxPossibleNewEnemies);
                
                AddEnemiesInRoom(enemiesByType, selectedType.Key, newEnemies);
                UpdateRemainingEnemiesInQuest(enemiesInQuestByType, selectedType.Key, newEnemies);

                selectedEnemies += newEnemies;
            } 

            return enemiesByType;
        }

        private static void AddEnemiesInRoom(EnemiesByType enemiesByType, WeaponTypeSO selectedType, int newEnemies)
        {
            if (enemiesByType.EnemiesByTypeDictionary.TryGetValue(selectedType, out var enemiesForItem))
            {
                enemiesByType.EnemiesByTypeDictionary[selectedType] = enemiesForItem + newEnemies;
            }
            else
            {
                enemiesByType.EnemiesByTypeDictionary.Add(selectedType, newEnemies);
            }
        }

        private static void UpdateRemainingEnemiesInQuest(EnemiesByType enemiesInQuestByType, 
            WeaponTypeSO selectedType, int newEnemies)
        {
            if (enemiesInQuestByType.EnemiesByTypeDictionary.Count == 0)
                throw new ArgumentException("Enemies in Quest cannot be an empty collection.", nameof(enemiesInQuestByType));
            enemiesInQuestByType.EnemiesByTypeDictionary[selectedType] -= newEnemies;
            if (enemiesInQuestByType.EnemiesByTypeDictionary[selectedType] <= 0)
            {
                enemiesInQuestByType.EnemiesByTypeDictionary.Remove(selectedType);
            }
        }

        public static Dictionary<EnemySO, int> GetEnemiesForRoom(RoomBhv roomBhv)
        {
            var enemiesBySo = new Dictionary<EnemySO, int>();
            foreach (var enemiesByType in roomBhv.roomData.EnemiesByType.EnemiesByTypeDictionary)
            {
                var selectedEnemy = GetRandomEnemyOfType(enemiesByType.Key);
                enemiesBySo.Add(selectedEnemy, enemiesByType.Value);
            }
            return enemiesBySo;
        }
        public static void LoadEnemies(List<EnemySO> enemyList)
        {
            enemyListForCurrentDungeon = EnemySelector.FilterEnemies(enemyList);
            ApplyDelegates();
        }

        public static EnemySO GetRandomEnemyOfType(WeaponTypeSO enemyType)
        {
            List<EnemySO> currentEnemies = GetEnemiesFromType(enemyType);
            return currentEnemies[Random.Range(0, currentEnemies.Count)];
        }

        public GameObject InstantiateEnemyWithType(Vector3 position, Quaternion rotation, WeaponTypeSO enemyType)
        {
            EnemySO currentEnemy = GetRandomEnemyOfType(enemyType);
            GameObject enemy;
            //TODO change to use weaponType in comparison
            if (currentEnemy.weapon.name == "None")
            {
                enemy = Instantiate(barehandEnemyPrefab, position, rotation);
            }
            else if (currentEnemy.weapon.name == "Bow")
            {
                enemy = Instantiate(shooterEnemyPrefab, position, rotation);
            }
            else if (currentEnemy.weapon.name == "BombThrower")
            {
                enemy = Instantiate(bomberEnemyPrefab, position, rotation);
            }
            else if (currentEnemy.weapon.name == "Cure")
            {
                enemy = Instantiate(healerEnemyPrefab, position, rotation);
            }
            else
            {
                enemy = Instantiate(enemyPrefab, position, rotation);
            }
            enemy.GetComponent<EnemyController>().LoadEnemyData(currentEnemy);
            return enemy;
        }
    
        public GameObject InstantiateEnemyFromScriptableObject(Vector3 position, Quaternion rotation, EnemySO enemySo)
        {
            GameObject enemy;
            //TODO change to use weaponType in comparison
            if (enemySo.weapon.name == "None")
            {
                enemy = Instantiate(barehandEnemyPrefab, position, rotation);
            }
            else if (enemySo.weapon.name == "Bow")
            {
                enemy = Instantiate(shooterEnemyPrefab, position, rotation);
            }
            else if (enemySo.weapon.name == "BombThrower")
            {
                enemy = Instantiate(bomberEnemyPrefab, position, rotation);
            }
            else if (enemySo.weapon.name == "Cure")
            {
                enemy = Instantiate(healerEnemyPrefab, position, rotation);
            }
            else
            {
                enemy = Instantiate(enemyPrefab, position, rotation);
            }
            enemy.GetComponent<EnemyController>().LoadEnemyData(enemySo);
            return enemy;
        }

        private static List<EnemySO> GetEnemiesFromType(WeaponTypeSO weaponType)
        {
            //TODO create these lists only once per type on dungeon load
            return enemyListForCurrentDungeon.Where(enemy => enemy.weapon == weaponType).ToList();
        }

        private static void ApplyDelegates()
        {
            foreach (var enemy in enemyListForCurrentDungeon)
            {
                enemy.movement.movementType = GetMovementType(enemy.movement.enemyMovementIndex);
            }
        }
        public static MovementType GetMovementType(MovementEnum moveTypeEnum)
        {
            switch (moveTypeEnum)
            {
                case MovementEnum.None:
                    return EnemyMovement.NoMovement;
                case MovementEnum.Random:
                    return EnemyMovement.MoveRandomly;
                case MovementEnum.Flee:
                    return EnemyMovement.FleeFromPlayer;
                case MovementEnum.Follow:
                    return EnemyMovement.FollowPlayer;
                case MovementEnum.Follow1D:
                    return EnemyMovement.FollowPlayer1D;
                case MovementEnum.Random1D:
                    return EnemyMovement.MoveRandomly1D;
                case MovementEnum.Flee1D:
                    return EnemyMovement.FleeFromPlayer1D;
                default:
                    Debug.Log("No Movement Attached to Enemy");
                    return null;
            }
        }
    }
}
