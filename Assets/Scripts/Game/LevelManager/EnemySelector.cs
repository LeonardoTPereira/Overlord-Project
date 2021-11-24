using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using Util;

namespace Game.LevelManager
{
    public static class EnemySelector
    {
        private static string HEALER_WEAPON_PREFAB_NAME = "EnemyHealArea";

        /// Select semi-randomly enemies to populate a room. The enemies types
        /// are controlled by the weapon they hold.
        public static Dictionary<WeaponTypeSO, int> Select(
            DungeonRoom room,
            Dictionary<WeaponTypeSO, int> enemies
        ) {
            Dictionary<WeaponTypeSO, int> selected =
                new Dictionary<WeaponTypeSO, int>();
            // Get enemies by attack method
            List<WeaponTypeSO> mellees = FilterMellees(enemies);
            List<WeaponTypeSO> rangers = FilterRangers(enemies);
            List<WeaponTypeSO> healers = FilterHealers(enemies);
            List<WeaponTypeSO> soldiers = MergeEnemyLists(mellees, rangers);
            // Select enemies
            System.Random rand = RandomSingleton.GetInstance().Random;
            var amount = room.TotalEnemies;
            bool hasHealer = false;
            while (amount > 0)
            {
                if (amount == 1)
                {
                    AddEnemy(GetRandomEnemy(soldiers), ref selected);
                    amount -= 1;
                }
                else if (amount == 2)
                {
                    if (!hasHealer && rand.Next(100) < 50)
                    {
                        AddEnemy(healers[0], ref selected);
                        AddEnemy(GetRandomEnemy(soldiers), ref selected);
                        hasHealer = true;
                    }
                    else
                    {
                        AddEnemy(GetRandomEnemy(soldiers), ref selected);
                        AddEnemy(GetRandomEnemy(soldiers), ref selected);
                    }
                    amount -= 2;
                }
                else if (amount >= 3)
                {
                    if (!hasHealer && rand.Next(100) < 50)
                    {
                        AddEnemy(healers[0], ref selected);
                        AddEnemy(GetRandomEnemy(soldiers), ref selected);
                        AddEnemy(GetRandomEnemy(soldiers), ref selected);
                        hasHealer = true;
                    }
                    else
                    {
                        AddEnemy(GetRandomEnemy(soldiers), ref selected);
                        AddEnemy(GetRandomEnemy(soldiers), ref selected);
                        AddEnemy(GetRandomEnemy(soldiers), ref selected);
                    }
                    amount -= 3;
                }
            }
            return selected; 
        }

        /// Filter a dictionary of enemies and return the healer enemies.
        private static List<WeaponTypeSO> FilterHealers(
            Dictionary<WeaponTypeSO, int> enemies
        ) {
            List<WeaponTypeSO> healers = new List<WeaponTypeSO>();
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in enemies)
            {
                if (enemy.Key.weaponPrefab == null)
                {
                    continue;
                }
                string weapon = enemy.Key.weaponPrefab.name;
                if (weapon.Equals(HEALER_WEAPON_PREFAB_NAME))
                {
                    healers.Add(enemy.Key);
                }
            }
            return healers;
        }

        /// Filter a dictionary of enemies and return the ranger enemies.
        private static List<WeaponTypeSO> FilterRangers(
            Dictionary<WeaponTypeSO, int> enemies
        ) {
            List<WeaponTypeSO> rangers = new List<WeaponTypeSO>();
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in enemies)
            {
                if (enemy.Key.hasProjectile)
                {
                    rangers.Add(enemy.Key);
                }
            }
            return rangers;
        }

        /// Filter a dictionary of enemies and return the mellee enemies.
        private static List<WeaponTypeSO> FilterMellees(
            Dictionary<WeaponTypeSO, int> enemies
        ) {
            List<WeaponTypeSO> mellees = new List<WeaponTypeSO>();
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in enemies)
            {
                bool isHealer = false;
                if (enemy.Key.weaponPrefab != null)
                {
                    string weapon = enemy.Key.weaponPrefab.name;
                    isHealer = weapon.Equals(HEALER_WEAPON_PREFAB_NAME);
                }
                bool hasProjectile = enemy.Key.hasProjectile;
                if (!hasProjectile && !isHealer)
                {
                    mellees.Add(enemy.Key);
                }
            }
            return mellees;
        }

        /// Merge two lists of enemies.
        private static List<WeaponTypeSO> MergeEnemyLists(
            List<WeaponTypeSO> enemies1,
            List<WeaponTypeSO> enemies2
        ) {
            List<WeaponTypeSO> enemies = new List<WeaponTypeSO>();
            foreach (WeaponTypeSO enemy in enemies1)
            {
                enemies.Add(enemy);
            }
            foreach (WeaponTypeSO enemy in enemies2)
            {
                enemies.Add(enemy);
            }
            return enemies;
        }

        /// Return a random enemy (a pair of weapon type SO).
        private static WeaponTypeSO GetRandomEnemy(
            List<WeaponTypeSO> enemies
        ) {
            System.Random rand = RandomSingleton.GetInstance().Random;
            int index = rand.Next(enemies.Count);
            return enemies[index];
        }

        /// Add the entered enemy in the entered dictionary of enemies.
        private static void AddEnemy(
            WeaponTypeSO enemy,
            ref Dictionary<WeaponTypeSO, int> enemies
        ) {
            if (enemies.ContainsKey(enemy))
            {
                enemies[enemy]++;
            }
            else
            {
                enemies[enemy] = 1;
            }
        }
    }
}