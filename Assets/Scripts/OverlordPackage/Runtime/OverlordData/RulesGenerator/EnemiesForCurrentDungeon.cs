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
        private static List<EnemySO> _enemyListForCurrentDungeon;
        public static List<EnemySO> CurrentEnemies => _enemyListForCurrentDungeon;

        public static void UpdateEnemiesForCurrentDungeon(List<EnemySO> enemyList)
        {
            _enemyListForCurrentDungeon = enemyList;
        }

        public static EnemySO GetRandomEnemyOfType(WeaponTypeSo enemyType)
        {
            List<EnemySO> currentEnemies = GetEnemiesFromType(enemyType);
            Debug.Log("ENEMY COUNT: " + currentEnemies.Count);
            return currentEnemies[RandomSingleton.GetInstance().Next(0, currentEnemies.Count)];
        }

        private static List<EnemySO> GetEnemiesFromType(WeaponTypeSo weaponType)
        {
            Debug.Log(weaponType.ToString());
            //TODO create these lists only once per type on dungeon load
            return CurrentEnemies.Where(enemy => enemy.weapon == weaponType).ToList();
        }
    }
}