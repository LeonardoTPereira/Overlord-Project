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
        protected int timesToExecuteEA;
        protected bool isVisualizingDungeon;


        /// Level Generator constructor.
        public LevelGenerator(GeneratorSettings.Parameters parameters, int timesToExecuteEA, 
            bool isVisualizingDungeon, FitnessInput fitnessInput, FitnessPlot plotter = null) {
            Parameters = parameters;
            FitnessInput = fitnessInput;
            Plotter = plotter;
            this.timesToExecuteEA = timesToExecuteEA;
            this.isVisualizingDungeon = isVisualizingDungeon;
        }

        protected void InvokeGenerationEvent(float progress)
        {
            NewEaGenerationEventHandler?.Invoke(this, new NewEAGenerationEventArgs(progress, false));
        }
        
        protected void InvokeCompletedEvent()
        {
	        NewEaGenerationEventHandler?.Invoke(this, new NewEAGenerationEventArgs(1.0f, true));
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
            //_individualJson = new IndividualJson();
            for (var i = 0; i < timesToExecuteEA; i++)
            {
                // Initialize the MAP-Elites population
                var pop = InitializePopulation();
                await EvolvePopulation(pop);
                // Get the final population (solution)
                solution = pop;
                //_individualJson.AddFitness(solution.EliteList[0]);
            }
            //_individualJson.SaveJson();
        }

        protected virtual Population InitializePopulation()
        {
            Population pop = new Population(
                SearchSpace.ExplorationRanges.Length,
                SearchSpace.LeniencyRanges.Length, Plotter
            );
            var maxTries = INTERMEDIATE_POPULATION;
            var currentTry = 0;
            // Generate the initial population
            while (pop.EliteList.Count < Parameters.Population && currentTry < maxTries)
            {
                var individual = Individual.CreateRandom(FitnessInput);
                individual.Fix(FitnessInput.DesiredEnemies);
                individual.CalculateFitness();
                pop.PlaceIndividual(individual);
                currentTry++;
            }

            return pop;
        }

        protected virtual async Task EvolvePopulation(Population pop)
        {
            // Evolve the population
            int g = 0;
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            while (!HasReachedStopCriteria(end, start, pop.GetAmountOfBiomesWithElites(), pop.GetAmountOfBiomesWithElitesBetterThan(Parameters.AcceptableFitness)))
            {
                if (isVisualizingDungeon)
                {
                    CurrentGenerationEventHandler?.Invoke(this, new CurrentGenerationEventArgs(pop));
                    waitGeneration = true;
                    while (waitGeneration)
                    {
                        await Task.Yield();
                    }
                }

                var intermediate = CreateIntermediatePopulation(pop, g);
                // Place the offspring in the MAP-Elites population
                foreach (Individual individual in intermediate)
                {
                    pop.PlaceIndividual(individual);
                }
                g++;
                end = DateTime.Now;

                // Update the progress bar
                double seconds = (end - start).TotalSeconds;
                var progress = (float) seconds / Parameters.Time;
                InvokeGenerationEvent(progress);
                pop.UpdateBiomes(g);
                await Task.Yield();
            }
            //pop.SaveJson();
            InvokeCompletedEvent();
        }

        private List<Individual> CreateIntermediatePopulation(in Population pop, int g)
        {
            var intermediate = new List<Individual>();
            while (intermediate.Count < INTERMEDIATE_POPULATION)
            {
                // Apply the crossover operation
                var parents = Selection.SelectParents(CROSSOVER_PARENTS, Parameters.Competitors, pop);
                var offspring = CreateOffspring(parents);

                // Place the offspring in the MAP-Elites population
                foreach (var individual in offspring)
                {
                    individual.Fix(FitnessInput.DesiredEnemies);
                    individual.CalculateFitness();
                    individual.generation = g;
                    intermediate.Add(individual);
                }
            }
            return intermediate;
        }

        protected Individual[] CreateOffspring(Individual[] parents)
        {
            var originalParentEnemy1 = parents[0].dungeon.GetNumberOfEnemies();
            var originalParentEnemy2 = parents[1].dungeon.GetNumberOfEnemies();
            var crossoverOffspringEnemy1 = 0;
            var crossoverOffspringEnemy2 = 0;
            var offspring = Crossover.Apply(parents[0], parents[1]);
            if (offspring.Length == CROSSOVER_PARENTS)
            {
                parents[0] = offspring[0];
                parents[1] = offspring[1];
                crossoverOffspringEnemy1 = parents[0].dungeon.GetNumberOfEnemies();
                if (crossoverOffspringEnemy1 != FitnessInput.DesiredEnemies)
                {
                    Debug.LogError($"Requested {FitnessInput.DesiredEnemies} Enemies, found {crossoverOffspringEnemy1}");
                }
                crossoverOffspringEnemy2 = parents[1].dungeon.GetNumberOfEnemies();
                if (crossoverOffspringEnemy2 != FitnessInput.DesiredEnemies)
                {
                    Debug.LogError($"Requested {FitnessInput.DesiredEnemies} Enemies, found {crossoverOffspringEnemy2}");
                }
            }
            else
            {
                offspring = new Individual[2];
                if (Parameters.Mutation > RandomSingleton.GetInstance().RandomPercent())
                {
                    parents[0] = new Individual(parents[0]);
                    parents[1] = new Individual(parents[1]);
                    var enemiesP12 = parents[0].dungeon.GetNumberOfEnemies();
                    if (enemiesP12 != FitnessInput.DesiredEnemies)
                    {
                        Debug.LogError($"Requested {FitnessInput.DesiredEnemies} Enemies, found {enemiesP12}");
                    }

                    var enemiesP22 = parents[1].dungeon.GetNumberOfEnemies();
                    if (enemiesP22 != FitnessInput.DesiredEnemies)
                    {
                        Debug.LogError($"Requested {FitnessInput.DesiredEnemies} Enemies, found {enemiesP22}");
                    }
                    return parents;
                }
            }

            offspring[0] = Mutation.Apply(parents[0]);
            offspring[1] = Mutation.Apply(parents[1]);
            var enemiesO1 = offspring[0].dungeon.GetNumberOfEnemies();
            if (enemiesO1 != FitnessInput.DesiredEnemies)
            {
                Debug.LogError($"Requested {FitnessInput.DesiredEnemies} Enemies, found {enemiesO1}");
            }

            var enemiesO2 = offspring[1].dungeon.GetNumberOfEnemies();
            if (enemiesO2 != FitnessInput.DesiredEnemies)
            {
                Debug.LogError($"Requested {FitnessInput.DesiredEnemies} Enemies, found {enemiesO2}");
            }
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