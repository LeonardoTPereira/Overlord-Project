﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameManager;
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
            var enemiesInQuestByType = new Dictionary<WeaponTypeSO, int>(questLine.EnemyParametersForQuestLine.TotalByType);
            
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (dungeonPart.Value is DungeonRoom dungeonRoom && !dungeonRoom.IsStartRoom())
                {
                    dungeonRoom.EnemiesByType = SelectWeaponTypesForRoom(dungeonRoom, enemiesInQuestByType);
                }
            }
        }

        private static Dictionary<WeaponTypeSO, int> SelectWeaponTypesForRoom(DungeonRoom dungeonRoom, Dictionary<WeaponTypeSO, int> enemiesInQuestByType)
        {
            var enemiesByType = new Dictionary<WeaponTypeSO, int>();
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

        private static void AddEnemiesInRoom(Dictionary<WeaponTypeSO, int> enemiesByType, WeaponTypeSO selectedType, int newEnemies)
        {
            if (enemiesByType.TryGetValue(selectedType, out var enemiesForItem))
            {
                enemiesByType[selectedType] = enemiesForItem + newEnemies;
            }
            else
            {
                enemiesByType.Add(selectedType, newEnemies);
            }
        }

        private static void UpdateRemainingEnemiesInQuest(Dictionary<WeaponTypeSO, int> enemiesInQuestByType, 
            WeaponTypeSO selectedType, int newEnemies)
        {
            if (enemiesInQuestByType.Count == 0)
                throw new ArgumentException("Enemies in Quest cannot be an empty collection.", nameof(enemiesInQuestByType));
            enemiesInQuestByType[selectedType] -= newEnemies;
            if (enemiesInQuestByType[selectedType] <= 0)
            {
                enemiesInQuestByType.Remove(selectedType);
            }
        }

        public static Dictionary<EnemySO, int> GetEnemiesForRoom(RoomBhv roomBhv)
        {
            var enemiesBySo = new Dictionary<EnemySO, int>();
            foreach (var enemiesByType in roomBhv.roomData.EnemiesByType)
            {
                var selectedEnemy = GameManagerSingleton.instance.enemyLoader.GetRandomEnemyOfType(enemiesByType.Key);
                enemiesBySo.Add(selectedEnemy, enemiesByType.Value);
            }
            return enemiesBySo;
        }
    }
}