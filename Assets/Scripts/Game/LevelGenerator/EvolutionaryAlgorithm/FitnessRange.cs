using MyBox;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public struct FitnessRange
    {
        public RangedFloat distanceToInput;
        public RangedFloat usage;
        public RangedFloat standardDeviation;
        public RangedFloat sparsity;
    }
}