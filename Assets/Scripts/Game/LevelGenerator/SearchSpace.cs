namespace LevelGenerator
{
    /// This class defines the discretization of the search space of dungeon
    /// levels for mapping the MAP-Elites population.
    public class SearchSpace
    {
        /// Return the list of all the ranges of coefficient of exploration.
        public static (float, float)[] CoefficientOfExplorationRanges()
        {
            return new (float, float)[] {
                (0.5f, 0.6f),
                (0.6f, 0.7f),
                (0.7f, 0.8f),
                (0.8f, 0.9f),
                (0.9f, 1.0f),
            };
        }

        /// Return the list of all the leniency ranges.
        public static (float, float)[] LeniencyRanges()
        {
            return new (float, float)[] {
                (0.5f, 0.6f),
                (0.4f, 0.5f),
                (0.3f, 0.4f),
                (0.2f, 0.3f),
                (0.1f, 0.2f),
            };
        }

        /// Return the index of the entered coefficient of exploration in the
        /// list of ranges of coefficient of exploration.
        public static int GetCoefficientOfExplorationIndex(
            float _exploration
        ) {
            return GetIndex(_exploration, CoefficientOfExplorationRanges());
        }

        /// Return the index of the entered leniency in the list of leniency
        /// ranges.
        public static int GetLeniencyIndex(
            float _leniency
        ) {
            return GetIndex(_leniency, LeniencyRanges());
        }

        /// Return the index of the value in the entered list of ranges.
        private static int GetIndex(
            float _value,
            (float, float)[] _list
        ) {
            int index = Common.UNKNOWN;
            for (int i = 0; i < _list.Length; i++)
            {
                (float min, float max) e = _list[i];
                if (_value >= e.min && _value < e.max)
                {
                    index = i;
                }
            }
            return index;
        }
    }
}