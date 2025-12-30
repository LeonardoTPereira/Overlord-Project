using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    public static class EnemiesForCurrentDungeon
    {
        private static List<TopdownEnemySO> _enemyListForCurrentDungeon;
        public static List<TopdownEnemySO> CurrentEnemies => _enemyListForCurrentDungeon;

        public static void UpdateEnemiesForCurrentDungeon(List<TopdownEnemySO> enemyList)
        {
            _enemyListForCurrentDungeon = enemyList;
        }

        public static TopdownEnemySO GetRandomEnemyOfType(WeaponTypeSo enemyType)
        {
            List<TopdownEnemySO> currentEnemies = GetEnemiesFromType(enemyType);
            Debug.Log("ENEMY COUNT: " + currentEnemies.Count);
            return currentEnemies[RandomSingleton.GetInstance().Next(0, currentEnemies.Count)];
        }

        private static List<TopdownEnemySO> GetEnemiesFromType(WeaponTypeSo weaponType)
        {
            Debug.Log(weaponType.ToString());
            //TODO create these lists only once per type on dungeon load
            return CurrentEnemies.Where(enemy => enemy.weapon == weaponType).ToList();
        }
    }
}