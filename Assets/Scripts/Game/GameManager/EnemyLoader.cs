using System;
using System.Collections.Generic;
using System.Linq;
using Game.EnemyManager;
using Game.LevelManager.DungeonLoader;
using Game.Maestro;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.Quests;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using Util;
using static Util.Enums;

namespace Game.GameManager
{
    [Serializable]
    public class EnemyLoader : MonoBehaviour
    {
        private static List<EnemySO> _enemyListForCurrentDungeon;
        
        public GameObject enemyPrefab;
        public GameObject barehandEnemyPrefab;
        public GameObject shooterEnemyPrefab;
        public GameObject bomberEnemyPrefab;
        public GameObject healerEnemyPrefab;

        
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
                var maxPossibleNewEnemies = math.min(selectedType.Value.Count, enemiesInRoom - selectedEnemies);
                var newEnemiesCount = RandomSingleton.GetInstance().Random.Next(1, maxPossibleNewEnemies);
                enemiesByType.AddNEnemiesFromType(selectedType, newEnemiesCount);
                enemiesInQuestByType.RemoveCurrentTypeIfEmpty(selectedType.Key);
                selectedEnemies += newEnemiesCount;
            } 

            return enemiesByType;
        }

        public static void LoadEnemies(List<EnemySO> enemyList)
        {
            _enemyListForCurrentDungeon = EnemySelector.FilterEnemies(enemyList);
            ApplyDelegates();
        }

        public static EnemySO GetRandomEnemyOfType(WeaponTypeSO enemyType)
        {
            List<EnemySO> currentEnemies = GetEnemiesFromType(enemyType);
            return currentEnemies[RandomSingleton.GetInstance().Next(0, currentEnemies.Count)];
        }

        public GameObject InstantiateEnemyWithType(Vector3 position, Quaternion rotation, WeaponTypeSO enemyType, int questId)
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
            enemy.GetComponent<EnemyController>().LoadEnemyData(currentEnemy, questId);
            return enemy;
        }
    
        public GameObject InstantiateEnemyFromScriptableObject(Vector3 position, Quaternion rotation, EnemySO enemySo, int questId)
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
            enemy.GetComponent<EnemyController>().LoadEnemyData(enemySo, questId);
            return enemy;
        }

        private static List<EnemySO> GetEnemiesFromType(WeaponTypeSO weaponType)
        {
            //TODO create these lists only once per type on dungeon load
            return _enemyListForCurrentDungeon.Where(enemy => enemy.weapon == weaponType).ToList();
        }

        private static void ApplyDelegates()
        {
            foreach (var enemy in _enemyListForCurrentDungeon)
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
