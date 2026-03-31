using System;
using System.Collections.Generic;
using System.Diagnostics;
using Overlord.GenerationController.Facade;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    /// Alias for the coordinate of MAP-Elites matrix.
    using Coordinate = System.ValueTuple<int, int>;

    /// This struct represents a MAP-Elites population.
    ///
    /// The MAP-Elites population is a N-dimensional array of individuals,
    /// where each matrix's ax corresponds to a different feature.
    ///
    /// This particular population is mapped into the enemy's movement type
    /// and its weapon. Thus, each Elite (or matrix cell) corresponds to a
    /// combination of different types of movement types and weapons.
    public struct Population
    {
        /// The MAP-Elites dimension. The dimension is defined by the number of
        /// movement types multiplied by the number of weapon types.
        public (int movement, int weapon) dimension { get; }
        /// The MAP-Elites map (a matrix of individuals).
        public Individual[,] map { get; }

        private IEnemyFitness _fitnessFunction;

        /// MAP-Elites Population constructor.
        public Population(int _movement, int _weapons, IEnemyFitness fitnessFunction)
        {
            dimension = (_movement, _weapons);
            map = new Individual[dimension.movement, dimension.weapon];
            _fitnessFunction = fitnessFunction;
        }

        /// Return the number of Elites of the population.
        public int Count()
        {
            int count = 0;
            for (int m = 0; m < dimension.movement; m++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    if (!(map[m, w] is null))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// Return a list corresponding to the Elites coordinates.
        public List<Coordinate> GetElitesCoordinates()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int m = 0; m < dimension.movement; m++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    if (!(map[m, w] is null))
                    {
                        coordinates.Add((m, w));
                    }
                }
            }
            return coordinates;
        }

        /// Add an individual in the MAP-Elites population.
        ///
        /// First, we identify which Elite the individual is classified in.
        /// Then, if the corresponding Elite is empty, the individual is placed
        /// there. Otherwise, we compare the both old and new individuals, and
        /// the best individual is placed in the corresponding Elite.
        public void PlaceIndividual(
            Individual _individual
        )
        {
            // Calculate the individual slot (Elite)
            int m = Convert.ToInt32(_individual.Enemy.Movement);
            m = _individual.MovementIndex;
            int w = Convert.ToInt32(_individual.Weapon.Weapon);
            var test = map;

            // If the new individual deserves to survive
            if (_fitnessFunction.IsBest(_individual, map[m, w]))
            {
                // Then, place the individual in the MAP-Elites population
                map[m, w] = _individual;
            }
        }

        /// Return a list with the population individuals.
        public List<Individual> ToList()
        {
            List<Individual> list = new List<Individual>();
            for (int m = 0; m < dimension.movement; m++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    if (map[m, w] != null)
                    {
                        list.Add(map[m, w]);
                    }
                }
            }
            return list;
        }

        /// Print all the individuals of the MAP-Elites population.
        /*
        public enum WeaponType
        {
            Barehand,    // Enemy attacks the player with barehands (Melee).
            Sword,       // Enemy uses a short sword to damage the player (Melee).
            Bow,         // Enemy shots projectiles towards the player (Range).
            BombThrower, // Enemy shots bombs towards the player (Range).
            Shield,      // Enemy uses a shield to defend itself (Defense).
            CureSpell,   // Enemy uses magic to cure other enemies (Defense).
        }
        public void Debug()
        {
            for (int m = 0; m < dimension.movement; m++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    string log = "Elite ";
                    log += RulesGeneratorFacade.Instance.GetEnemyMovementType().GetMovementName(m) + "-";
                    log += ((WeaponType)w);
                    UnityEngine.Debug.Log(log);
                    if (map[m, w] is null)
                    {
                        UnityEngine.Debug.Log("  Empty");
                    }
                    else
                    {
                        map[m, w].Debug();
                    }
                    Console.WriteLine();
                }
            }
        }
        */
        public int NIndividualsBetterThan(int amount, float acceptableFitness)
        {
            var betterThanNCounter = 0;
            for (var w = 0; w < dimension.weapon; w++)
            {
                var betterCounter = 0;
                for (var m = 0; m < dimension.movement; m++)
                {
                    if ((map[m, w]?.FitnessValue ?? float.MaxValue) < acceptableFitness)
                    {
                        betterCounter++;
                    }
                }
                if (betterCounter > amount)
                {
                    betterThanNCounter++;
                }
            }

            return betterThanNCounter;
        }

        public int MinimumElitesOfEachType()
        {
            var minimumOfEach = int.MaxValue;
            for (var w = 0; w < dimension.weapon; w++)
            {
                var count = 0;
                for (var m = 0; m < dimension.movement; m++)
                {
                    if (!(map[m, w] is null))
                    {
                        count++;
                    }
                }
                if (count < minimumOfEach)
                {
                    minimumOfEach = count;
                }
            }
            return minimumOfEach;
        }
    }
}