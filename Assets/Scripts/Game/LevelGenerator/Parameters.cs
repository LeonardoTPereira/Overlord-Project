namespace Game.LevelGenerator
{
    /// This struct holds the parameters of the evolutionary level generator.
    public struct Parameters
    {
        /// The seed that initializes the random generator.
        public int seed { get; }
        /// The maximum time.
        public int time { get; }
        /// The initial population size.
        public int population { get; }
        /// The mutation chance.
        public int mutation { get; }
        /// The number of competitors of tournament selection.
        public int competitors { get; }
        /// The aimed number of rooms.
        public int rooms { get; }
        /// The aimed number of keys.
        public int keys { get; }
        /// The aimed number of locks.
        public int locks { get; }
        /// The aimed number of enemies.
        public int enemies { get; }
        /// The aimed linear coefficient.
        public float linearCoefficient { get; }
        /// The object that calculates fitness values of individuals.
        public Fitness fitness { get; }

        /// Parameters constructor.
        public Parameters(
            int _seed,
            int _time,
            int _population,
            int _mutation,
            int _competitors,
            int _rooms,
            int _keys,
            int _locks,
            int _enemies,
            float _linearCoefficient,
            Fitness _fitness
        ) {
            seed = _seed;
            time = _time;
            population = _population;
            mutation = _mutation;
            competitors = _competitors;
            rooms = _rooms;
            keys = _keys;
            locks = _locks;
            enemies = _enemies;
            linearCoefficient = _linearCoefficient;
            fitness = _fitness;
        }
    }
}