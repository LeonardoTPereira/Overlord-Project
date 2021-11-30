using System;
using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.LevelManager;
using ScriptableObjects;
using Util;
using UnityEngine;

namespace Game.Maestro
{
    public static class EnemySelector
    {
        private static string HEALER_WEAPON_PREFAB_NAME = "EnemyHealArea";
        private static int CENT = 100;
        private static int CHANCE = 50;
        private static int SINGLE_CHALLENGE = 1;
        private static int DOUBLE_CHALLENGE = 2;


        /// Select semi-randomly enemies to populate a room. The enemies types
        /// are controlled by the weapon they hold.
        public static EnemiesByType Select(
            DungeonRoom room,
            ref EnemiesByType enemies
        ) {
            var selected = new EnemiesByType();
            // Get enemies by attack method
            EnemiesByType melees = FilterMelees(enemies);
            EnemiesByType rangers = FilterRangers(enemies);
            EnemiesByType healers = FilterHealers(enemies);
            // Select enemies
            var amount = room.TotalEnemies;
            bool healer = false;
            while (amount > 0)
            {
                bool hasMelees = melees.Count() > 0;
                bool hasRangers = rangers.Count() > 0;
                bool hasHealers = healers.Count() > 0;
                // Distribute enemies
                if (amount == 1)
                {
                    // We only place a healer in a room with a single enemy if
                    // no more melees or rangers remain.
                    if (!hasMelees && !hasRangers && hasHealers)
                    {
                        AddRandomEnemy(ref healers, ref enemies, ref selected);
                    }
                    else
                    {
                        PlaceSoldier(ref melees, ref rangers, ref enemies, ref selected);
                    }
                    amount -= SINGLE_CHALLENGE;
                }
                else if (amount >= 2)
                {
                    // We place a healer if there are at least two enemies in
                    // the room. If the room already has a healer, we only add
                    // another healer if there is no other type of enemy.
                    if ((!healer && hasHealers) || (!hasMelees && !hasRangers))
                    {
                        AddRandomEnemy(ref healers, ref enemies, ref selected);
                        amount -= SINGLE_CHALLENGE;
                        healer = true;
                    }
                    else if (melees.Count() + rangers.Count() > 1)
                    {
                        PlaceSoldier(ref melees, ref rangers, ref enemies, ref selected);
                        PlaceSoldier(ref melees, ref rangers, ref enemies, ref selected);
                        amount -= DOUBLE_CHALLENGE;
                    }
                    // If the code reaches this `else`, it means that there are
                    // no melees or no rangers.
                    else
                    {
                        PlaceSoldier(ref melees, ref rangers, ref enemies, ref selected);
                        amount -= SINGLE_CHALLENGE;
                    }
                }
            }
            return selected;
        }

        private static void PlaceSoldier(
            ref EnemiesByType melees,
            ref EnemiesByType rangers,
            ref EnemiesByType enemies,
            ref EnemiesByType selected
        ) {
            bool hasMelees = melees.Count() > 0;
            bool hasRangers = rangers.Count() > 0;
            if (hasMelees && hasRangers)
            {
                var rand = RandomSingleton.GetInstance().Random;
                if (rand.Next(CENT) < CHANCE)
                {
                    AddRandomEnemy(ref melees, ref enemies, ref selected);
                }
                else
                {
                    AddRandomEnemy(ref rangers, ref enemies, ref selected);
                }
            }
            else if (hasMelees)
            {
                AddRandomEnemy(ref melees, ref enemies, ref selected);
            }
            else if (hasRangers)
            {
                AddRandomEnemy(ref rangers, ref enemies, ref selected);
            }
            else
            {
                Debug.LogError("There are no more soldiers to place.");
            }
        }

        /// Add the entered enemy in the entered dictionary of enemies.
        private static void AddRandomEnemy(
            ref EnemiesByType soldiers,
            ref EnemiesByType enemies,
            ref EnemiesByType selected
        ) {
            // Add a unity of the selected enemy
            var enemy = soldiers.GetRandom().Key;
            if (selected.EnemiesByTypeDictionary.TryGetValue(enemy, out var enemiesForType))
            {
                selected.EnemiesByTypeDictionary[enemy] = enemiesForType + 1;
            }
            else
            {
                selected.EnemiesByTypeDictionary.Add(enemy, 1);
            }
            // Remove a unit of the selected enemy from the dictionaries
            RemoveEnemy(ref soldiers, enemy);
            RemoveEnemy(ref enemies, enemy);
        }

        private static void RemoveEnemy(
            ref EnemiesByType enemies,
            WeaponTypeSO enemy
        ) {
            enemies.EnemiesByTypeDictionary[enemy] -= 1;
            if (enemies.EnemiesByTypeDictionary[enemy] <= 0)
            {
                enemies.EnemiesByTypeDictionary.Remove(enemy);
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
        private static EnemiesByType FilterHealers(EnemiesByType enemies)
        {
            EnemiesByType healers = new EnemiesByType();
            WeaponTypeAmountDictionary dict = enemies.EnemiesByTypeDictionary;
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in dict)
            {
                if (IsHealer(enemy.Key) && enemy.Value > 0)
                {
                    healers.EnemiesByTypeDictionary.Add(enemy.Key, enemy.Value);
                }
            }
            return healers;
        }

        /// Filter a dictionary of enemies and return the ranger enemies.
        private static EnemiesByType FilterRangers(EnemiesByType enemies)
        {
            EnemiesByType rangers = new EnemiesByType();
            WeaponTypeAmountDictionary dict = enemies.EnemiesByTypeDictionary;
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in dict)
            {
                if (IsRanger(enemy.Key) && enemy.Value > 0)
                {
                    rangers.EnemiesByTypeDictionary.Add(enemy.Key, enemy.Value);
                }
            }
            return rangers;
        }

        /// Filter a dictionary of enemies and return the melee enemies.
        private static EnemiesByType FilterMelees(EnemiesByType enemies)
        {
            EnemiesByType melees = new EnemiesByType();
            WeaponTypeAmountDictionary dict = enemies.EnemiesByTypeDictionary;
            foreach (KeyValuePair<WeaponTypeSO, int> enemy in dict)
            {
                if (IsMelee(enemy.Key) && enemy.Value > 0)
                {
                    melees.EnemiesByTypeDictionary.Add(enemy.Key, enemy.Value);
                }
            }
            return melees;
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
            return selected;
        }
    }
}