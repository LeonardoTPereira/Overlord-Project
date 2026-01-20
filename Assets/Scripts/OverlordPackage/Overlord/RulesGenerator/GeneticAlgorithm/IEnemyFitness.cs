namespace Overlord.RulesGenerator.EnemyGeneration
{
    /// This interface defines the fitness function for enemy generation.
    public interface IEnemyFitness
    {
        void SetSearchSpace(SearchSpaceConfig searchSpace);
        void Calculate(ref Individual _individual, float goal);
        bool IsBest(Individual _i1, Individual _i2);
    }
}