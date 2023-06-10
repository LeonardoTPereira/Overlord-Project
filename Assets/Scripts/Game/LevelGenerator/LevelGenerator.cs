using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Events;
using Game.ExperimentControllers;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using UnityEngine;
using Util;

namespace Game.LevelGenerator
{
    /// This class holds the evolutionary level generation algorithm.
    public class LevelGenerator
    {
        /// The number of parents to be selected for crossover.
        protected static readonly int CROSSOVER_PARENTS = 2;
        /// The size of the intermediate population.
        private static readonly int INTERMEDIATE_POPULATION = 100;

        /// The evolutionary parameters.
        protected GeneratorSettings.Parameters Parameters;

        protected FitnessInput FitnessInput;
        /// The found MAP-Elites population.
        private Population solution;
        /// Return the found MAP-Elites population.
        public Population Solution { get => solution; }
        public bool waitGeneration;

        /// The event to handle the progress bar update.
        public static event NewEAGenerationEvent NewEaGenerationEventHandler;
        public static event CurrentGenerationEvent CurrentGenerationEventHandler;


        protected FitnessPlot Plotter;
        private IndividualJson _individualJson;

        /// Level Generator constructor.
        public LevelGenerator(GeneratorSettings.Parameters parameters, FitnessInput fitnessInput, FitnessPlot plotter = null) {
            Parameters = parameters;
            FitnessInput = fitnessInput;
            Plotter = plotter;
        }

        protected void InvokeGenerationEvent(float progress)
        {
            NewEaGenerationEventHandler?.Invoke(this, new NewEAGenerationEventArgs(progress, false));
        }

        /// Generate and return a set of levels.
        public async Task Evolve()
        {
            DateTime start = DateTime.Now;
            await Evolution();
            DateTime end = DateTime.Now;
        }

        /// Perform the level evolution process.
        private async Task Evolution()
        {
            for (var i = 0; i < 3; i++)
            {
                Debug.Log("Initialize Population: " + i);
                // Initialize the MAP-Elites population
                var population = InitializePopulation();
                Debug.Log("Initialize Population Finished: " + i);
                await EvolvePopulation(population);
                // Get the final population (solution)
                solution = population;
                Debug.Log("Passou aqui: " + i);
                // _individualJson.AddFitness(solution.EliteList[0]);
            }
            // _individualJson.SaveJson();
        }

        protected virtual Population InitializePopulation()
        {
            Population population = new Population(
                SearchSpace.ExplorationRanges.Length,
                SearchSpace.LeniencyRanges.Length,
                SearchSpace.LinearityRanges.Length,
                Plotter
            );
            var maxTries = INTERMEDIATE_POPULATION;
            var currentTry = 0;
            // Generate the initial population
            while (population.EliteList.Count < Parameters.Population && currentTry < maxTries)
            {
                var individual = Individual.CreateRandom(FitnessInput);
                individual.Fix();
                individual.CalculateFitness();
                population.PlaceIndividual(individual);
                currentTry++;
            }

            population.Debug();

            return population;
        }

        protected virtual async Task EvolvePopulation(Population population)
        {
            // Evolve the population
            int g = 0;
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            while (!HasReachedStopCriteria(end, start, population.GetAmountOfBiomesWithElites(), population.GetAmountOfBiomesWithElitesBetterThan(Parameters.AcceptableFitness)))
            {
                _individualJson = new IndividualJson();
                Debug.Log("HasReachedStopCriteria: " + HasReachedStopCriteria(end, start, population.GetAmountOfBiomesWithElites(), population.GetAmountOfBiomesWithElitesBetterThan(Parameters.AcceptableFitness)));
                var intermediate = CreateIntermediatePopulation(population, g);
                // Place the offspring in the MAP-Elites population
                foreach (Individual individual in intermediate)
                {
                    population.PlaceIndividual(individual);
                }
                g++;
                end = DateTime.Now;

                // Update the progress bar
                double seconds = (end - start).TotalSeconds;
                Debug.Log("Seconds: " + seconds);
                var progress = (float) seconds / Parameters.Time;
                population.Debug();
                NewEaGenerationEventHandler?.Invoke(this, new NewEAGenerationEventArgs(progress, false));
                population.UpdateBiomes(g);
                await Task.Yield();
                CurrentGenerationEventHandler?.Invoke(this, new CurrentGenerationEventArgs(population));
                waitGeneration = true;
                while (waitGeneration)
                {
                    await Task.Yield();
                }
                _individualJson.AddFitness(population.EliteList[0]);
                _individualJson.SaveJson();
            }
            //population.SaveJson();
            NewEaGenerationEventHandler?.Invoke(this, new NewEAGenerationEventArgs(1.0f, true));
        }

        private List<Individual> CreateIntermediatePopulation(in Population population, int g)
        {
            var intermediate = new List<Individual>();
            while (intermediate.Count < INTERMEDIATE_POPULATION)
            {
                // Apply the crossover operation
                var parents = Selection.SelectParents(CROSSOVER_PARENTS, Parameters.Competitors, population);
                var offspring = CreateOffspring(parents);

                // Place the offspring in the MAP-Elites population
                foreach (var individual in offspring)
                {
                    individual.Fix();
                    individual.CalculateFitness();
                    individual.generation = g;
                    intermediate.Add(individual);
                }
            }
            return intermediate;
        }

        protected Individual[] CreateOffspring(Individual[] parents)
        {
            var offspring = Crossover.Apply(parents[0], parents[1]);
            // Apply the mutation operation with a random chance or
            // always that the crossover was not successful
            if (offspring.Length != 0 && Parameters.Mutation <= RandomSingleton.GetInstance().RandomPercent())
                return offspring;
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

            return offspring;
        }

        private bool HasReachedStopCriteria(DateTime end, DateTime start, int biomesWithElites, float elitesWithAcceptableFitness)
        {
            if (biomesWithElites < Parameters.MinimumElite) return false;
            if (elitesWithAcceptableFitness >= Parameters.MinimumElite) return true;
            var elapsedTime = (end - start).TotalSeconds;
            return elapsedTime > Parameters.Time;
        }
    }
}