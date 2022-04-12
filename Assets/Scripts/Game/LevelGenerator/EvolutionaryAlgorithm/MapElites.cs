namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public class MapElites
    {
        private Individual[,] Elites { get; }

        public MapElites(int explorationEliteCount, int leniencyEliteCount)
        {
            Elites = new Individual[explorationEliteCount, leniencyEliteCount];
        }

        public Individual GetElite(int explorationIndex, int leniencyIndex)
        {
            return Elites[explorationIndex, leniencyIndex];
        }

        public void SetElite(int explorationIndex, int leniencyIndex, Individual individual)
        {
            Elites[explorationIndex, leniencyIndex] = individual;
        }
    }
}