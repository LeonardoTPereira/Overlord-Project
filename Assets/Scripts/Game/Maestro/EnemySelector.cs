using System;
using System.Collections.Generic;
using System.Linq;
using Game.LevelManager;
using ScriptableObjects;
using Util;

namespace Game.Maestro
{
    public static class EnemySelector
    {
        private static string HEALER_WEAPON_PREFAB_NAME = "EnemyHealArea";
        private static int CENT = 100;
        private static int CHANCE = 50;
        private static int SINGLE_CHALLENGE = 1;
        private static int DOUBLE_CHALLENGE = 2;
        private static int MULTIPLE_CHALLENGE = 3;


        /// Select semi-randomly enemies to populate a room. The enemies types
        /// are controlled by the weapon they hold.
        public static Dictionary<WeaponTypeSO, int> Select(
            DungeonRoom room,
            Dictionary<WeaponTypeSO, int> enemies
        ) {
            Dictionary<WeaponTypeSO, int> selected =
                new Dictionary<WeaponTypeSO, int>();
            // Get enemies by attack method
            List<WeaponTypeSO> melees = FilterMelees(enemies);
            List<WeaponTypeSO> rangers = FilterRangers(enemies);
            List<WeaponTypeSO> healers = FilterHealers(enemies);
            List<WeaponTypeSO> soldiers = MergeEnemyLists(melees, rangers);
            // Select enemies
            System.Random rand = RandomSingleton.GetInstance().Random;
            var amount = room.TotalEnemies;
            bool hasHealer = false;
            while (amount > 0)
            {
                if (amount == SINGLE_CHALLENGE)
                {
                    AddEnemy(GetRandomEnemy(soldiers), ref selected);
                    amount -= SINGLE_CHALLENGE;
                }
                else if (amount == DOUBLE_CHALLENGE)
                {
                    if (!hasHealer && rand.Next(CENT) < CHANCE)
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
                    amount -= DOUBLE_CHALLENGE;
                }
                else if (amount >= MULTIPLE_CHALLENGE)
                {
                    if (!hasHealer && rand.Next(CENT) < CHANCE)
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
                    amount -= MULTIPLE_CHALLENGE;
                }
            }
            return selected;
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

        /// Return true if an enemy is a healer.
        private static bool IsHealer(WeaponTypeSO enemy)
        {
            if (enemy.weaponPrefab == null)
            {
                return false;
            }
            string weapon = enemy.weaponPrefab.name;
            return weapon.Equals(HEALER_WEAPON_PREFAB_NAME);
        }

        /// Return true if an enemy is a ranger.
        private static bool IsRanger(WeaponTypeSO enemy)
        {
            return enemy.hasProjectile;
        }

        /// Return true if an enemy is a ranger.
        private static bool IsMelee(WeaponTypeSO enemy)
        {
            return !IsRanger(enemy) && !IsHealer(enemy);
        }

        /// Filter a dictionary of enemies and return the healer enemies.
        private static List<WeaponTypeSO> FilterHealers(
            Dictionary<WeaponTypeSO, int> enemies
        ) {
            List<WeaponTypeSO> healers = new List<WeaponTypeSO>();
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in enemies)
            {
                if (IsHealer(enemy.Key))
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
                if (IsRanger(enemy.Key))
                {
                    rangers.Add(enemy.Key);
                }
            }
            return rangers;
        }

        /// Filter a dictionary of enemies and return the melee enemies.
        private static List<WeaponTypeSO> FilterMelees(
            Dictionary<WeaponTypeSO, int> enemies
        ) {
            List<WeaponTypeSO> melees = new List<WeaponTypeSO>();
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in enemies)
            {
                if (IsMelee(enemy.Key))
                {
                    melees.Add(enemy.Key);
                }
            }
            return melees;
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

        /// Filter a list of enemies by removing bad enemies.
        public static List<EnemySO> FilterEnemies(List<EnemySO> enemies)
        {
            List<EnemySO> selected = new List<EnemySO>();
            foreach (EnemySO enemy in enemies)
            {
                Enums.MovementEnum movement = enemy.movement.enemyMovementIndex;
                bool noMovement = movement == Enums.MovementEnum.None;
                if (IsMelee(enemy.weapon) && !noMovement)
                {
                    selected.Add(enemy);
                }
                else if (!IsMelee(enemy.weapon))
                {
                    selected.Add(enemy);
                }
            }
            return enemies;
        }
    }
}