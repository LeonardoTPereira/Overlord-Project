using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
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
        public static EnemiesByType Select(
            DungeonRoom room,
            EnemiesByType enemies
        ) {
            var selected = new EnemiesByType();
            // Get enemies by attack method
            // TODO update lists to dictionaries, they must contain how much of every type is available to draw
            // If not, quest requirements can't be met
            List<WeaponTypeSO> melees = FilterMelees(enemies);
            List<WeaponTypeSO> rangers = FilterRangers(enemies);
            List<WeaponTypeSO> healers = FilterHealers(enemies);
            List<WeaponTypeSO> soldiers = MergeEnemyLists(melees, rangers);
            // Select enemies
            Random rand = RandomSingleton.GetInstance().Random;
            var amount = room.TotalEnemies;
            bool hasHealer = false;
            while (amount > 0)
            {
                if (amount == SINGLE_CHALLENGE)
                {
                    AddEnemy(soldiers.Count > 0 ? GetRandomEnemy(soldiers) : healers[0], selected, enemies);
                    amount -= SINGLE_CHALLENGE;
                }
                else if (amount == DOUBLE_CHALLENGE)
                {
                    if (!hasHealer && rand.Next(CENT) < CHANCE)
                    {
                        AddEnemy(healers[0], selected, enemies);
                        AddEnemy(GetRandomEnemy(soldiers), selected, enemies);
                        hasHealer = true;
                    }
                    else
                    {
                        AddEnemy(GetRandomEnemy(soldiers), selected, enemies);
                        AddEnemy(GetRandomEnemy(soldiers), selected, enemies);
                    }
                    amount -= DOUBLE_CHALLENGE;
                }
                else if (amount >= MULTIPLE_CHALLENGE)
                {
                    if (!hasHealer && rand.Next(CENT) < CHANCE)
                    {
                        AddEnemy(healers[0], selected, enemies);
                        AddEnemy(GetRandomEnemy(soldiers), selected, enemies);
                        AddEnemy(GetRandomEnemy(soldiers), selected, enemies);
                        hasHealer = true;
                    }
                    else
                    {
                        AddEnemy(GetRandomEnemy(soldiers), selected, enemies);
                        AddEnemy(GetRandomEnemy(soldiers), selected, enemies);
                        AddEnemy(GetRandomEnemy(soldiers), selected, enemies);
                    }
                    amount -= MULTIPLE_CHALLENGE;
                }
            }
            return selected;
        }

        /// Add the entered enemy in the entered dictionary of enemies.
        private static void AddEnemy(WeaponTypeSO enemy, EnemiesByType enemies, EnemiesByType totalEnemies) {
            if (enemies.EnemiesByTypeDictionary.TryGetValue(enemy, out var enemiesForType))
            {
                enemies.EnemiesByTypeDictionary[enemy] = enemiesForType+1;
            }
            else
            {
                enemies.EnemiesByTypeDictionary.Add(enemy, 1);
            }
            //Remove a unit of the selected enemy from the total list and 
            totalEnemies.EnemiesByTypeDictionary[enemy] -= 1;
            if (totalEnemies.EnemiesByTypeDictionary[enemy] <= 0)
            {
                totalEnemies.EnemiesByTypeDictionary.Remove(enemy);
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
        private static List<WeaponTypeSO> FilterHealers(EnemiesByType enemies) {
            List<WeaponTypeSO> healers = new List<WeaponTypeSO>();
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in enemies.EnemiesByTypeDictionary)
            {
                if (IsHealer(enemy.Key))
                {
                    healers.Add(enemy.Key);
                }
            }
            return healers;
        }

        /// Filter a dictionary of enemies and return the ranger enemies.
        private static List<WeaponTypeSO> FilterRangers(EnemiesByType enemies) {
            List<WeaponTypeSO> rangers = new List<WeaponTypeSO>();
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in enemies.EnemiesByTypeDictionary)
            {
                if (IsRanger(enemy.Key))
                {
                    rangers.Add(enemy.Key);
                }
            }
            return rangers;
        }

        /// Filter a dictionary of enemies and return the melee enemies.
        private static List<WeaponTypeSO> FilterMelees(EnemiesByType enemies) {
            List<WeaponTypeSO> melees = new List<WeaponTypeSO>();
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in enemies.EnemiesByTypeDictionary)
            {
                if (IsMelee(enemy.Key))
                {
                    melees.Add(enemy.Key);
                }
            }
            return melees;
        }

        /// Merge two lists of enemies.
        private static List<WeaponTypeSO> MergeEnemyLists(List<WeaponTypeSO> enemies1, List<WeaponTypeSO> enemies2) {
            {
            }
            return enemies;
        }

        /// Return a random enemy (a pair of weapon type SO).
        private static WeaponTypeSO GetRandomEnemy(List<WeaponTypeSO> enemies) {
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