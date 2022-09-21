using System.Collections.Generic;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.LevelManager.DungeonLoader;
using ScriptableObjects;
using Util;
using UnityEngine;

namespace Game.Maestro
{
    public static class EnemySelector
    {
        public static EnemiesByType Select(DungeonRoom room, ref EnemiesByType enemies) 
        {
            var selected = new EnemiesByType();
            // Select enemies
            var amount = room.TotalEnemies;
            var hasMeleeOrRanged = false;
            var hasHealer = false;
            while (amount > 0)
            {
                if (IfCanCreateHealer(ref enemies, hasMeleeOrRanged, hasHealer, selected))
                {
                    hasHealer = true;
                    amount--;
                    continue;
                }

                if (selected.TryAddAttacker(ref enemies))
                {
                    hasMeleeOrRanged = true;
                    amount--;
                    continue;
                }

                if (selected.TryAddHealer(ref enemies))
                {
                    amount--;
                    hasHealer = true;
                }
                else
                {
                    Debug.LogError("No enemies found to add to room!");
                    break;
                }
            }

            return selected;
        }

        private static bool IfCanCreateHealer(ref EnemiesByType enemies, bool hasMeleeOrRanged, bool hasHealer, EnemiesByType selected)
        {
            return hasMeleeOrRanged && !hasHealer && selected.TryAddHealer(ref enemies);
        }

        private static bool IsBadEnemy(EnemySO enemy)
        {
            var movement = enemy.movement.enemyMovementIndex;
            // All melees that cannot move are bad enemies
            var noMovement = movement == Enums.MovementEnum.None;
            var a = enemy.weapon.IsMelee() && noMovement;
            // The melees with swords that flee from the player are bad enemies
            var fleeMovement = movement == Enums.MovementEnum.Flee;
            var flee1dMovement = movement == Enums.MovementEnum.Flee1D;
            var b = enemy.weapon.IsSword() && (fleeMovement || flee1dMovement);
            return a || b;
        }

        /// Filter a list of enemies by removing bad enemies.
        public static List<EnemySO> FilterEnemies(List<EnemySO> enemies)
        {
            var selected = new List<EnemySO>();
            foreach (var enemy in enemies)
            {
                if (!IsBadEnemy(enemy))
                {
                    selected.Add(enemy);
                }
            }
            return selected;
        }
    }
}