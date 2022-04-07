using System;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    /// This class represents an individual.
    ///
    /// Individuals are composed of a dungeon, their fitness value, and the
    /// generation when they were created. There are also other attributes to
    /// hold some measurements of the level.
    ///
    /// Why individuals are represented by a class instead of a struct? When
    /// using MAP-Elites some slots may be empty, then the `null` option makes
    /// easier to manage the MAP-Elites population.
    public class Individual
    {
        /// The dungeon level.
        public Dungeon dungeon;
        /// The generation in which the individual was created.
        public int generation;
        /// The linear coefficient of the dungeon level.
        public float linearCoefficient;
        /// The number of locked doors that must be unlocked to reach the goal.
        public int neededLocks;
        /// The number of rooms that must be visited to reach the goal.
        public float neededRooms;
        /// The coefficient of exploration.
        public float exploration;
        /// The leniency degree.
        public float leniency;

        private Fitness fitness;
        
        public Individual(FitnessParameters fitnessParameters)
        {
            Fitness = new Fitness(fitnessParameters);
            dungeon = new Dungeon();
            generation = Common.UNKNOWN;
            neededLocks = 0;
            neededRooms = 0;
            linearCoefficient = 0;
            leniency = 0;
            exploration = 0;
        }

        private Individual()
        {
        }

        /// Return a clone of the individual.
        public Individual Clone()
        {
            var individual = new Individual
            {
                Fitness = Fitness,
                generation = generation,
                neededLocks = neededLocks,
                neededRooms = neededRooms,
                linearCoefficient = linearCoefficient,
                dungeon = dungeon,
                leniency = leniency,
                exploration = exploration
            };
            return individual;
        }

        /// Calculate the linear coefficient of the dungeon level.
        public void CalculateLinearCoefficient()
        {
            linearCoefficient = 0f;
            var leafs = 0;
            foreach (var room in dungeon.Rooms)
            {
                var children = 0;
                if (room.Right?.Parent != null)
                {
                    children++;
                }
                if (room.Left?.Parent != null)
                {
                    children++;
                }
                if (room.Bottom?.Parent != null)
                {
                    children++;
                }
                if (children == 0)
                {
                    leafs++;
                }
                linearCoefficient += children;
            }
            int total = dungeon.Rooms.Count;
            linearCoefficient = linearCoefficient / (total - leafs);
        }

        /// Print the individual attributes and the dungeon map.
        public void Debug()
        {
            Console.WriteLine(LevelDebug.INDENT +
                "Generation=" + generation);
            Console.WriteLine(LevelDebug.INDENT +
                "Fitness=" + Fitness);
            Console.WriteLine(LevelDebug.INDENT +
                "Rooms=" + dungeon.Rooms.Count);
            Console.WriteLine(LevelDebug.INDENT +
                "Keys=" + dungeon.KeyIds.Count);
            Console.WriteLine(LevelDebug.INDENT +
                "Locks=" + dungeon.LockIds.Count);
            Console.WriteLine(LevelDebug.INDENT +
                "Enemies=" + dungeon.GetNumberOfEnemies());
            Console.WriteLine(LevelDebug.INDENT +
                "Linear coefficient=" + linearCoefficient);
            Console.WriteLine(LevelDebug.INDENT +
                "Coefficient of exploration=" + exploration);
            Console.WriteLine(LevelDebug.INDENT +
                "Leniency=" + leniency);
            Console.WriteLine(LevelDebug.INDENT +
                "Enemy Sparsity=" + fitness.fEnemySparsity);
            Console.WriteLine(LevelDebug.INDENT + "MISSION MAP=");
            LevelDebug.PrintMissionMap(dungeon, LevelDebug.INDENT);
            Console.WriteLine(LevelDebug.INDENT + "ENEMY MAP=");
            LevelDebug.PrintEnemyMap(dungeon, LevelDebug.INDENT);
        }

        /// Generate and return a random individual.
        public static Individual CreateRandom(FitnessParameters parameters)
        {
            var newIndividual = new Individual(parameters);
            var newDungeon = new Dungeon();
            newDungeon.GenerateRooms();
            newDungeon.PlaceEnemies(parameters.DesiredEnemies);
            newIndividual.dungeon = newDungeon;
            return newIndividual;
        }

        public void Fix()
        {
            dungeon.Fix(Fitness.DesiredParameters.DesiredEnemies);
        }
        
        public Fitness Fitness
        {
            get => fitness;
            set => fitness = value;
        }
    }
}