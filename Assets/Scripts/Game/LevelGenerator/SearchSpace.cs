namespace Game.LevelGenerator
{
    /// This class defines the discretization of the search space of dungeon
    /// levels for mapping the MAP-Elites population.
    public static class SearchSpace
    {
        public static readonly float[] ExplorationRanges = {0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f};
        public static readonly float[] LeniencyRanges = {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f};
        
        /// Return the index of the entered coefficient of exploration in the
        /// list of ranges of coefficient of exploration.
        public static int GetCoefficientOfExplorationIndex(in float exploration) {
            return GetIndex(exploration, ExplorationRanges);
        }

        /// Return the index of the entered leniency in the list of leniency
        /// ranges.
        public static int GetLeniencyIndex(in float leniency) {
            return GetIndex(leniency, LeniencyRanges);
        }

        /// Return the index of the value in the entered list of ranges.
        private static int GetIndex(in float value, in float[] ranges)
        {
            if (value < ranges[0]) return -1;
            for (var i = 1; i < ranges.Length; i++)
            {
                if (value < ranges[i])
                {
                    return i-1;
                }
            }
            return -1;
        }
    }
}