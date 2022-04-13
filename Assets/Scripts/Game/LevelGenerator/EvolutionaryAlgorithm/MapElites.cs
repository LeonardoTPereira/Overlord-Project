namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public class MapElites
    {
        private Individual[,] Elites { get; }
        private int ExplorationEliteSize { get; }
        private int LeniencyEliteSize { get; }

        public MapElites(int explorationEliteSize, int leniencyEliteSize)
        {
            Elites = new Individual[explorationEliteSize, leniencyEliteSize];
            ExplorationEliteSize = explorationEliteSize;
            LeniencyEliteSize = leniencyEliteSize;
        }

        public Individual GetElite(int explorationIndex, int leniencyIndex)
        {
            return Elites[explorationIndex, leniencyIndex];
        }

        public void SetElite(int explorationIndex, int leniencyIndex, Individual individual)
        {
            Elites[explorationIndex, leniencyIndex] = individual;
        }

        public bool IsCellInMapRange(int explorationIndex, int leniencyIndex)
        {
            if (explorationIndex < 0 || explorationIndex >= ExplorationEliteSize || leniencyIndex < 0 || leniencyIndex >= LeniencyEliteSize) {
                return false;
            }
            return true;
        }
    }
}