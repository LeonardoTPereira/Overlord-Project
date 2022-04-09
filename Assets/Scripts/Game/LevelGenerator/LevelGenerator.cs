using System;
using System.Collections.Generic;
using Game.Events;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using UnityEngine;
using Util;

namespace Game.LevelGenerator
{
    /// This class holds the evolutionary level generation algorithm.
    public class LevelGenerator
    {
        /// The number of parents to be selected for crossover.
        private static readonly int CROSSOVER_PARENTS = 2;
        /// The size of the intermediate population.
        private static readonly int INTERMEDIATE_POPULATION = 100;

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


        /// The event to handle the progress bar update.
        public static event NewEAGenerationEvent NewEaGenerationEventHandler;


        /// Level Generator constructor.
        public LevelGenerator(Parameters _prs) {
            prs = _prs;
            data = new Data();
            data.parameters = prs;
        }

        /// Generate and return a set of levels.
        public Population Evolve()
        {
            DateTime start = DateTime.Now;
            Evolution();
            DateTime end = DateTime.Now;
            data.duration = (end - start).TotalSeconds;
            return solution;
        }

        /// Perform the level evolution process.
        private void Evolution()
        {
            Debug.Log("Begins Evolution");
            // Initialize the MAP-Elites population
            Population pop = new Population(
                SearchSpace.CoefficientOfExplorationRanges().Length,
                SearchSpace.LeniencyRanges().Length
            );


            var maxTries = INTERMEDIATE_POPULATION;
            var currentTry = 0;
            // Generate the initial population
            while (pop.Count() < prs.Population && currentTry < maxTries)
            {
                Individual individual = Individual.CreateRandom(prs.FitnessParameters);
                individual.Fix();
                individual.CalculateLinearCoefficient();
                individual.Fitness.Calculate(ref individual);
                float ce = Metric.CoefficientOfExploration(individual);
                float le = Metric.Leniency(individual);
                individual.exploration = ce;
                individual.leniency = le;
                pop.PlaceIndividual(individual);
                currentTry++;
            }
            
            // Evolve the population
            int g = 0;
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            while (!HasReachedStopCriteria(end, start, pop.Count(), pop.IndividualsBetterThan(prs.AcceptableFitness)))
            {
                List<Individual> intermediate = new List<Individual>();
                while (intermediate.Count < INTERMEDIATE_POPULATION)
                {
                    // Apply the crossover operation
                    var parents = Selection.Select(CROSSOVER_PARENTS, prs.Competitors, pop);
                    var offspring = Crossover.Apply(parents[0], parents[1]);
                    // Apply the mutation operation with a random chance or
                    // always that the crossover was not successful
                    if (offspring.Length == 0 ||
                        prs.Mutation > RandomSingleton.GetInstance().RandomPercent()
                    ) {
                        if (offspring.Length == CROSSOVER_PARENTS)
                        {
                            parents[0] = offspring[0];
                            parents[1] = offspring[1];
                        }
                        else
                        {
                            offspring = new Individual[2];
                        }
                        offspring[0] = Mutation.Apply(parents[0]);
                        offspring[1] = Mutation.Apply(parents[1]);
                    }
                    // Place the offspring in the MAP-Elites population
                    for (int i = 0; i < offspring.Length; i++)
                    {
                        offspring[i].Fix();
                        offspring[i].CalculateLinearCoefficient();
                        offspring[i].Fitness.Calculate(ref offspring[i]);
                        float c = Metric.CoefficientOfExploration(offspring[i]);
                        float l = Metric.Leniency(offspring[i]);
                        offspring[i].exploration = c;
                        offspring[i].leniency = l;
                        offspring[i].generation = g;
                        intermediate.Add(offspring[i]);
                    }
                }

                // Place the offspring in the MAP-Elites population
                foreach (Individual individual in intermediate)
                {
                    pop.PlaceIndividual(individual);
                }

                g++;
                end = DateTime.Now;

                // Update the progress bar
                double seconds = (end - start).TotalSeconds;
                var progress = (float)seconds / prs.Time;
                NewEaGenerationEventHandler?.Invoke(this, new NewEAGenerationEventArgs(progress));
            }
            NewEaGenerationEventHandler?.Invoke(this, new NewEAGenerationEventArgs(1.0f));
            // Get the final population (solution)
            solution = pop;
        }

        private bool HasReachedStopCriteria(DateTime end, DateTime start, int totalElites, float elitesWithAcceptableFitness)
        {
            Debug.Log("Dungeon Elites: "+totalElites+", "+"Acceptable Fitness: "+ elitesWithAcceptableFitness);
            if (totalElites < prs.MinimumElite) return false;
            if (elitesWithAcceptableFitness >= prs.MinimumElite) return true;
            var elapsedTime = (end - start).TotalSeconds;
            return elapsedTime > prs.Time;
        }
    }
}