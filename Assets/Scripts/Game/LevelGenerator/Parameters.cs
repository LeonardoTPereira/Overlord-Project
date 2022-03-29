namespace Game.LevelGenerator
{
    /// This struct holds the parameters of the evolutionary level generator.
    public struct Parameters
    {
        /// The maximum time.
        public int Time { get; }
        /// The initial population size.
        public int Population { get; }
        /// The mutation chance.
        public int Mutation { get; }
        /// The number of competitors of tournament selection.
        public int Competitors { get; }

        public int MinimumElite { get; }
        public float AcceptableFitness { get; }

        /// The aimed number of rooms.
        public int Rooms { get; }
        /// The aimed number of keys.
        public int Keys { get; }
        /// The aimed number of locks.
        public int Locks { get; }
        /// The aimed number of enemies.
        public int Enemies { get; }
        /// The aimed linear coefficient.
        public float LinearCoefficient { get; }
        /// The object that calculates fitness values of individuals.
        public Fitness Fitness { get; }

        /// Parameters constructor.
        public Parameters(int time,
            int population,
            int mutation,
            int competitors,
            int minimumElite,
            float acceptableFitness,
            int rooms,
            int keys,
            int locks,
            int enemies,
            float linearCoefficient,
            Fitness fitness
        ) {
            Time = time;
            Population = population;
            Mutation = mutation;
            Competitors = competitors;
            MinimumElite = minimumElite;
            AcceptableFitness = acceptableFitness;
            Rooms = rooms;
            Keys = keys;
            Locks = locks;
            Enemies = enemies;
            LinearCoefficient = linearCoefficient;
            Fitness = fitness;
        }
    }
}