namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public class ClassicPopulation : Population
    {
        public ClassicPopulation(int explorationSize, int leniencySize, int linearitySize, FitnessPlot fitnessPlot = null) : base(explorationSize, leniencySize, linearitySize, fitnessPlot)
        {
        }
    }
}