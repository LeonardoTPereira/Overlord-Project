using System;
using System.Collections.Generic;
using Util;

namespace Game.EnemyGenerator
{
    /// This class holds the evolutionary enemy generation algorithm.
    public class EnemyGenerator
    {
        /// The number of parents to be selected for crossover.
        private static readonly int CROSSOVER_PARENTS = 2;

        /// The evolutionary parameters.
        private Parameters _parameters;
        /// The found MAP-Elites population.
        private Population _solution;
        /// The evolutionary process' collected data.
        private Data _data;

        /// Return the found MAP-Elites population.
        public Population Solution { get => _solution; }

        /// Return the collected data from the evolutionary process.
        public Data Data { get => _data; }

        /// Enemy Generator constructor.
        public EnemyGenerator(
            Parameters parameters
        ) {
            _parameters = parameters;
            _data = new Data
            {
                parameters = _parameters
            };
        }

        /// Generate and return a set of enemies.
        public Population Evolve()
        {
            DateTime start = DateTime.Now;
            Evolution();
            DateTime end = DateTime.Now;
            _data.duration = (end - start).TotalSeconds;
            return _solution;
        }

        /// Perform the enemy evolution process.
        private void Evolution()
        {

            // Initialize the MAP-Elites population
            Population pop = new Population(
                SearchSpace.AllMovementTypes().Length,
                SearchSpace.AllWeaponTypes().Length
            );

            // Generate the initial population
            while (pop.Count() < _parameters.population)
            {
                Individual ind = Individual.GetRandom();
                Difficulty.Calculate(ref ind);
                Fitness.Calculate(ref ind, _parameters.difficulty);
                pop.PlaceIndividual(ind);
            }

            // Save the initial population
            _data.initial = new List<Individual>(pop.ToList());

            // Evolve the population
            for (int g = 0; g < _parameters.generations; g++)
            {
                List<Individual> intermediate = new List<Individual>();
                while (intermediate.Count < _parameters.intermediate)
                {
                    // Apply the crossover operation
                    Individual[] parents = Selection.Select(CROSSOVER_PARENTS, _parameters.competitors, pop);
                    Individual[] offspring = Crossover.Apply(parents[0], parents[1]);
                    // Apply the mutation operation
                    if (_parameters.mutation > RandomSingleton.GetInstance().RandomPercent())
                    {
                        parents[0] = offspring[0];
                        offspring[0] = Mutation.Apply(parents[0], _parameters.geneMutation);
                        parents[1] = offspring[1];
                        offspring[1] = Mutation.Apply(parents[1], _parameters.geneMutation);
                    }
                    // Add the new individuals in the intermediate population
                    for (int i = 0; i < offspring.Length; i++)
                    {
                        Difficulty.Calculate(ref offspring[i]);
                        Fitness.Calculate(ref offspring[i], _parameters.difficulty);
                        intermediate.Add(offspring[i]);
                    }
                }

                // Place the intermediate population in the MAP-Elites
                foreach (Individual individual in intermediate)
                {
                    individual.Generation = g;
                    pop.PlaceIndividual(individual);
                }

                // Save the intermediate population
                if (g == _parameters.generations / 2)
                {
                    _data.intermediate = new List<Individual>(pop.ToList());
                }
            }

            // Get the final population (solution)
            _solution = pop;

            // Save the final population
            _data.final = new List<Individual>(_solution.ToList());
        }
    }
}