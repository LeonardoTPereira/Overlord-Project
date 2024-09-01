using System.Collections.Generic;
using System.Threading.Tasks;
using Game.ExperimentControllers;
using UnityEngine;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public class ClassicEvolutionaryAlgorithm : LevelGenerator
    {
        private const int PopSize = 100;
        private const int MaxGenWithoutImprovement = 30;
        private int _nGenerationsWithoutImprovement;
        private float _bestFitnessYet;

        protected override Population InitializePopulation()
        {
            waitGeneration = false;
            var dungeons = new ClassicPopulation(0, 0, Plotter);
            for (var i = 0; i < PopSize; ++i)
            {
                var individual = Individual.CreateRandom(FitnessInput);
                individual.dungeon.PlaceEnemies(FitnessInput.DesiredEnemies);
                if (individual.dungeon.GetNumberOfEnemies() != FitnessInput.DesiredEnemies)
                {
                    Debug.LogError($"Requested {FitnessInput.DesiredEnemies} Enemies, found {individual.dungeon.GetNumberOfEnemies()}");
                }
                individual.Fix(FitnessInput.DesiredEnemies);
                if (individual.dungeon.GetNumberOfEnemies() != FitnessInput.DesiredEnemies)
                {
                    Debug.LogError($"Requested {FitnessInput.DesiredEnemies} Enemies, found {individual.dungeon.GetNumberOfEnemies()}");
                }
                dungeons.EliteList.Add(individual);
                individual.generation = 0;
            }
            PopulationFitness.CalculateFitness(dungeons.EliteList);
            dungeons.EliteList[0] = PopulationFitness.BestDungeon;
            return dungeons;
        }

        protected override async Task EvolvePopulation(Population pop)
        {
            _nGenerationsWithoutImprovement = 0;
            var minFitness = float.MaxValue;
            int generation;
            _bestFitnessYet = float.MaxValue;

 
            //Evolve all the generations from the GA
            for (generation = 0; !HasMetStopCriteria(generation, minFitness); ++generation)
            {
                /*CurrentGenerationEventHandler?.Invoke(this, new CurrentGenerationEventArgs(pop));
                waitGeneration = true;
                while (waitGeneration)
                {
                    await Task.Yield();
                }*/


                var intermediate = new List<Individual>();
                for (var i = 0; i < (PopSize / 2); ++i)
                {
                    var parents = Selection.SelectParents(CROSSOVER_PARENTS, Parameters.Competitors, pop);
                    var offspring = CreateOffspring(parents);

                    // Place the offspring in the MAP-Elites population
                    foreach (var individual in offspring)
                    {
                        individual.Fix(FitnessInput.DesiredEnemies);
                        individual.generation = generation;
                        intermediate.Add(individual);
                        individual.BiomeName = individual.Fitness.DesiredInput.PlayerProfile.ToString();
                    }
                }
                PopulationFitness.CalculateFitness(intermediate);
                if (pop.EliteList[0].IsBetterThan(PopulationFitness.BestDungeon))
                {
                    intermediate[0] = pop.EliteList[0];
                    intermediate[1] = PopulationFitness.BestDungeon;
                }
                else
                {
                    intermediate[1] = pop.EliteList[0];
                    intermediate[0] = PopulationFitness.BestDungeon;
                }
                minFitness = intermediate[0].Fitness.Result;
                var progress = generation / (float)Parameters.MinimumElite;
                pop.EliteList = intermediate;
                if (pop is ClassicPopulation classicPopulation)
                {
                    classicPopulation.UpdateFitnessPlot(generation);
                }
                InvokeGenerationEvent(progress);
                await Task.Yield();
            }
            InvokeCompletedEvent();
        }

        private bool HasMetStopCriteria(int gen, float min)
        {
            if (gen >= Parameters.MinimumElite)
            {
                return true;
            }

            if (min <= 0.01f)
            {
                return true;
            }

            if ((_bestFitnessYet - min) > 0.001f)
            {
                _bestFitnessYet = min;
                _nGenerationsWithoutImprovement = 0;
            }
            else
            {
                _nGenerationsWithoutImprovement++;
            }

            return _nGenerationsWithoutImprovement >= MaxGenWithoutImprovement;
        }

        public ClassicEvolutionaryAlgorithm(GeneratorSettings.Parameters parameters, int timesToExecuteEA, 
            bool isVisualizingDungeon, FitnessInput fitnessInput, FitnessPlot fitnessPlot = null) 
            : base(parameters, timesToExecuteEA, isVisualizingDungeon,fitnessInput, fitnessPlot)
        {
            _nGenerationsWithoutImprovement = 0;
        }
    }
}