namespace Game.EnemyGenerator
{
    /// This struct holds the parameters of the evolutionary enemy generator.
    public struct Parameters
    {
        /// The maximum number of generations.
        public int Generations { get; }
        /// The initial population size.
        public int Population { get; }
        /// The intermediate population size.
        public int Intermediate { get; }
        /// The mutation chance.
        public int Mutation { get; }
        /// The mutation chance of a single gene.
        public int GeneMutation { get; }
        /// The number of competitors of the tournament selection.
        public int Competitors { get; }
        public int MinimumElite { get; }
        public float AcceptableFitness { get; }
        /// The aimed difficulty of the enemies.
        public float Difficulty { get; }

        /// Parameters constructor.
        public Parameters(
            int generations,
            int population,
            int intermediate,
            int mutation,
            int geneMutation,
            int competitors,
            int minimumElite,
            float acceptableFitness,
            float difficulty
        ) {
            Generations = generations;
            Population = population;
            Intermediate = intermediate;
            Mutation = mutation;
            GeneMutation = geneMutation;
            Competitors = competitors;            
            MinimumElite = minimumElite;
            AcceptableFitness = acceptableFitness;
            Difficulty = difficulty;
        }
    }
}
