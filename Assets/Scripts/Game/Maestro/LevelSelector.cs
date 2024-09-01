using System.Collections.Generic;
using Game.LevelGenerator.LevelSOs;

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
                if (dungeon.FitnessFromEa.NormalizedResult <= 2)
                {
                    selected.Add(dungeon);
                }
            }
            return selected;
        }
    }
}