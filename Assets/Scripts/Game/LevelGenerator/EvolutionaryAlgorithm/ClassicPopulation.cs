namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public class ClassicPopulation : Population
    {
        public ClassicPopulation(int explorationSize, int leniencySize, FitnessPlot fitnessPlot = null) : base(explorationSize, leniencySize, fitnessPlot)
        {
        }
    }
}