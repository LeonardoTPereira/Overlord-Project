namespace Game.EnemyGenerator
{
    /// This struct holds the parameters of the evolutionary enemy generator.
    public struct Parameters
    {
        /// The seed that initializes the random generator.
        public int seed { get; }
        /// The maximum number of generations.
        public int generations { get; }
        /// The initial population size.
        public int population { get; }
        /// The intermediate population size.
        public int intermediate { get; }
        /// The mutation chance.
        public int mutation { get; }
        /// The mutation chance of a single gene.
        public int geneMutation { get; }
        /// The number of competitors of the tournament selection.
        public int competitors { get; }
        /// The aimed difficulty of the enemies.
        public float difficulty { get; }

        /// Parameters constructor.
        public Parameters(
            int _seed,
            int _generations,
            int _population,
            int _intermediate,
            int _mutation,
            int _geneMutation,
            int _competitors,
            float _difficulty
        ) {
            seed = _seed;
            generations = _generations;
            population = _population;
            intermediate = _intermediate;
            mutation = _mutation;
            geneMutation = _geneMutation;
            competitors = _competitors;
            difficulty = _difficulty;
        }
    }
}
