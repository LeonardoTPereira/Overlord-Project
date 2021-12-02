using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameManager;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.Maestro;
using Game.NarrativeGenerator.Quests;
using LevelGenerator;
using MyBox;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using Util;

namespace Game.LevelManager
{
    public static class EnemyDispenser
    {

        public static void DistributeEnemiesInDungeon(Map map, QuestLine questLine)
        {
            Debug.Log("QuestLine: " + questLine.name);
            Debug.Log("EnemyParameters: " + questLine.EnemyParametersForQuestLine);
            Debug.Log("Enemies: " + questLine.EnemyParametersForQuestLine.NEnemies + " - " + questLine.EnemyParametersForQuestLine.TotalByType);
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
                var selectedEnemy = GameManagerSingleton.Instance.enemyLoader.GetRandomEnemyOfType(enemiesByType.Key);
                enemiesBySo.Add(selectedEnemy, enemiesByType.Value);
            }
            return enemiesBySo;
        }
    }
}