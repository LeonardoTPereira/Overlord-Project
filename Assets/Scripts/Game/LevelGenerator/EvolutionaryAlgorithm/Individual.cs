using System;
using System.IO;

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
        public string BiomeName { get; set; }

        public Individual(FitnessInput fitnessInput)
        {
            Fitness = new Fitness(fitnessInput);
            dungeon = new Dungeon();
            generation = Common.UNKNOWN;
            neededLocks = 0;
            neededRooms = 0.0f;
            linearCoefficient = 0.0f;
            leniency = 0.0f;
            exploration = 0.0f;
        }

        /// Return a clone of the individual.
        public Individual(Individual originalIndividual)
        {
            Fitness = new Fitness(originalIndividual.Fitness);
            generation = originalIndividual.generation;
            neededLocks = originalIndividual.neededLocks;
            neededRooms = originalIndividual.neededRooms;
            linearCoefficient = originalIndividual.linearCoefficient;
            dungeon = new Dungeon(originalIndividual.dungeon);
            leniency = originalIndividual.leniency;
            exploration = originalIndividual.exploration;
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
            linearCoefficient /= (total - leafs);
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
            Console.WriteLine(LevelDebug.INDENT + "MISSION MAP=");
            LevelDebug.PrintMissionMap(dungeon, LevelDebug.INDENT);
            Console.WriteLine(LevelDebug.INDENT + "ENEMY MAP=");
            LevelDebug.PrintEnemyMap(dungeon, LevelDebug.INDENT);
        }

        /// Generate and return a random individual.
        public static Individual CreateRandom(FitnessInput input)
        {
            var newIndividual = new Individual(input);
            var newDungeon = new Dungeon();
            newDungeon.GenerateRooms();
            newIndividual.dungeon = newDungeon;
            return newIndividual;
        }

        public void Fix(int enemies)
        {
            dungeon.Fix(enemies);
        }

        public void CalculateFitness(FitnessRange fitnessRange)
        {
            CalculateLinearCoefficient();
            CalculateUsage();
            Fitness.Calculate(this, fitnessRange);
        }
        
        public void CalculateFitness()
        {
            CalculateLinearCoefficient();
            CalculateUsage();
            Fitness.Calculate(this);
            exploration = Metric.CoefficientOfExploration(this);
            leniency = Metric.Leniency(this);
        }

        private void CalculateUsage()
        {
            if (dungeon.LockIds.Count > 0)
            {
                // Calculate the number of locks needed to finish the level
                neededLocks = AStar.FindRoute(dungeon);
                // Validate the calculated number of needed locks
                if (neededLocks > dungeon.LockIds.Count)
                {
                    throw new InvalidDataException("Inconsistency! The number of " +
                                                   "needed locks is higher than the number of total " +
                                                   "locks of the level." +
                                                   "\n  Total locks=" + dungeon.LockIds.Count +
                                                   "\n  Needed locks=" + neededLocks);
                }

                neededRooms = 0.0f;
                // Calculate the number of rooms needed to finish the level
                for (int i = 0; i < 3; i++)
                {
                    DFS dfs = new DFS(dungeon);
                    dfs.FindRoute();
                    neededRooms += dfs.NVisitedRooms;
                }

                neededRooms /= 3.0f;
                // Validate the calculated number of needed rooms
                if (neededRooms > dungeon.Rooms.Count)
                {
                    throw new InvalidDataException("Inconsistency! The number of " +
                                                   "needed rooms is higher than the number of total " +
                                                   "rooms of the level." +
                                                   "\n  Total rooms=" + dungeon.Rooms.Count +
                                                   "\n  Needed rooms=" + neededRooms);
                }
            }
        }

        public bool IsBetterThan(Individual other)
        {
            if(other == null) return true;
            return Fitness.IsBetter(other.Fitness);
        }
        
        public Fitness Fitness
        {
            get => fitness;
            set => fitness = value;
        }
    }
}