using System;
using System.Collections.Generic;

namespace EnemyGenerator
{
    /// This class holds the evolutionary enemy generation algorithm.
    public class EnemyGenerator
    {
        /// The number of parents to be selected for crossover.
        private static readonly int CROSSOVER_PARENTS = 2;

        /// The evolutionary parameters.
        private Parameters prs;
        /// The found MAP-Elites population.
        private Population solution;
        /// The evolutionary process' collected data.
        private Data data;

        /// Return the found MAP-Elites population.
        public Population Solution { get => solution; }

        /// Return the collected data from the evolutionary process.
        public Data Data { get => data; }

        /// Enemy Generator constructor.
        public EnemyGenerator(
            Parameters _prs
        ) {
            prs = _prs;
            data = new Data();
            data.parameters = prs;
        }

        /// Generate and return a set of enemies.
        public Population Evolve()
        {
            DateTime start = DateTime.Now;
            Evolution();
            DateTime end = DateTime.Now;
            data.duration = (end - start).TotalSeconds;
            return solution;
        }

        /// Perform the enemy evolution process.
        private void Evolution()
        {
            // Initialize the random generator
            Random rand = new Random(prs.seed);

            // Initialize the MAP-Elites population
            Population pop = new Population(
                SearchSpace.AllMovementTypes().Length,
                SearchSpace.AllWeaponTypes().Length
            );

            // Generate the initial population
            while (pop.Count() < prs.population)
            {
                Individual ind = Individual.GetRandom(ref rand);
                Difficulty.Calculate(ref ind);
                Fitness.Calculate(ref ind, prs.difficulty);
                pop.PlaceIndividual(ind);
            }

            // Save the initial population
            data.initial = new List<Individual>(pop.ToList());

            // Evolve the population
            for (int g = 0; g < prs.generations; g++)
            {
                List<Individual> intermediate = new List<Individual>();
                while (intermediate.Count < prs.intermediate)
                {
                    // Apply the crossover operation
                    Individual[] parents = Selection.Select(
                        CROSSOVER_PARENTS, prs.competitors, pop, ref rand
                    );
                    Individual[] offspring = Crossover.Apply(
                        parents[0], parents[1], ref rand
                    );
                    // Apply the mutation operation
                    if (prs.mutation > Common.RandomPercent(ref rand))
                    {
                        parents[0] = offspring[0];
                        offspring[0] = Mutation.Apply(
                            parents[0], prs.geneMutation, ref rand
                        );
                        parents[1] = offspring[1];
                        offspring[1] = Mutation.Apply(
                            parents[1], prs.geneMutation, ref rand
                        );
                    }
                    // Add the new individuals in the intermediate population
                    for (int i = 0; i < offspring.Length; i++)
                    {
                        Difficulty.Calculate(ref offspring[i]);
                        Fitness.Calculate(ref offspring[i], prs.difficulty);
                        intermediate.Add(offspring[i]);
                    }
                }

                // Place the intermediate population in the MAP-Elites
                foreach (Individual individual in intermediate)
                {
                    individual.generation = g;
                    pop.PlaceIndividual(individual);
                }

                // Save the intermediate population
                if (g == (int) prs.generations / 2)
                {
                    data.intermediate = new List<Individual>(pop.ToList());
                }
            }

            // Get the final population (solution)
            solution = pop;

            // Save the final population
            data.final = new List<Individual>(solution.ToList());
        }
    }
}