namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public class MapElites
    {
        private Individual[,,] Elites { get; }
        private int ExplorationEliteSize { get; }
        private int LeniencyEliteSize { get; }
        private int LinearityEliteSize { get; }

        public MapElites(int explorationEliteSize, int leniencyEliteSize, int linearityEliteSize)
        {
            Elites = new Individual[explorationEliteSize, leniencyEliteSize, linearityEliteSize];
            ExplorationEliteSize = explorationEliteSize;
            LeniencyEliteSize = leniencyEliteSize;
            LinearityEliteSize = linearityEliteSize;
        }

        public Individual GetElite(int explorationIndex, int leniencyIndex, int linearityIndex)
        {
            return Elites[explorationIndex, leniencyIndex, linearityIndex];
        }

        public void SetElite(int explorationIndex, int leniencyIndex, int linearityIndex, Individual individual)
        {
            Elites[explorationIndex, leniencyIndex, linearityIndex] = individual;
        }

        public bool IsCellInMapRange(int explorationIndex, int leniencyIndex, int linearityIndex)
        {
            if (explorationIndex < 0 || explorationIndex >= ExplorationEliteSize || leniencyIndex < 0 || leniencyIndex >= LeniencyEliteSize || linearityIndex < 0 || linearityIndex >= LinearityEliteSize) {
                return false;
            }
            return true;
        }
    }
}