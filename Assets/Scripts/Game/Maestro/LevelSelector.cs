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
    public static class LevelSelector
    {
        public static List<DungeonFileSo> FilterLevels(
            List<DungeonFileSo> levels
        ) {
            List<DungeonFileSo> selected = new List<DungeonFileSo>();
            foreach (DungeonFileSo dungeon in levels)
            {
                if (dungeon.fitness <= 2)
                {
                    selected.Add(dungeon);
                }
            }
            return selected;
        }
    }
}