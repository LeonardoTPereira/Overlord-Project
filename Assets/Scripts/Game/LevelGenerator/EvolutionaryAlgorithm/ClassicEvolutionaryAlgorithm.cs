using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Events;
using Game.ExperimentControllers;
using Game.LevelGenerator.LevelSOs;
using MyBox;
using UnityEngine;
using Util;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public class ClassicEvolutionaryAlgorithm : LevelGenerator
    {
        private const int PopSize = 100;
        private readonly int MAX_GEN_WITHOUT_IMPROVEMENT = 30;
        int nGenerationsWithoutImprovement;
        public bool waitGeneration;
        private float _bestFitnessYet;

        public static event CurrentGenerationEvent CurrentGenerationEventHandler;

        
        protected override Population InitializePopulation()
        {
            waitGeneration = false;
            var dungeons = new ClassicPopulation(0, 0);
            for (var i = 0; i < PopSize; ++i)
            {
                var individual = Individual.CreateRandom(FitnessInput);
                individual.Fix();
                dungeons.EliteList.Add(individual);
                individual.generation = 0;
            }
            PopulationFitness.CalculateFitness(dungeons.EliteList);
            return dungeons;
        }

        protected override async Task EvolvePopulation(Population pop)
        {
            var nGenerationsWithoutImprovement = 0;
            var hasFinished = false;
            var minFitness = float.MaxValue;
            int generation;
            _bestFitnessYet = float.MaxValue;

 
            //Evolve all the generations from the GA
            for (generation = 0; !HasMetStopCriteria(generation, minFitness); ++generation)
            {
                //Get every dungeon's fitness
                foreach (var dun in pop.EliteList)
                {
                    dun.CalculateFitness();
                    dun.generation = generation;
                }
                
                var bestDungeon = pop.EliteList[0];
                foreach (var dun in pop.EliteList)
                {
                    var current = dun.Fitness.Result;
                    if (minFitness < current) continue;
                    minFitness = current;
                    bestDungeon = dun;
                }


                var intermediate = new List<Individual>();
                for (var i = 0; i < (PopSize / 2); ++i)
                {
                    var parents = Selection.SelectParents(CROSSOVER_PARENTS, Parameters.Competitors, pop);
                    var offspring = CreateOffspring(parents);

                    // Place the offspring in the MAP-Elites population
                    foreach (var individual in offspring)
                    {
                        individual.Fix();
                        individual.generation = generation;
                        intermediate.Add(individual);
                        individual.BiomeName = "Classic";
                    }
                }
                PopulationFitness.CalculateFitness(intermediate);
                intermediate[0] = PopulationFitness.BestDungeon;
                _bestFitnessYet = PopulationFitness.BestDungeon.Fitness.Result;
                var progress = generation / (float)Parameters.MinimumElite;
                pop.EliteList = intermediate;
                InvokeGenerationEvent(progress);
                await Task.Yield();
            }
            Debug.LogWarning("Fitness:" + pop.EliteList[0].Fitness);
            InvokeGenerationEvent(1.0f);
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

            if ((_bestFitnessYet - min) > 0.01)
            {
                _bestFitnessYet = min;
                nGenerationsWithoutImprovement = 0;
            }
            else
            {
                nGenerationsWithoutImprovement++;
            }

            return nGenerationsWithoutImprovement >= MAX_GEN_WITHOUT_IMPROVEMENT;
        }

        public ClassicEvolutionaryAlgorithm(GeneratorSettings.Parameters parameters, FitnessInput fitnessInput, 
            FitnessPlot fitnessPlot = null) : base(parameters, fitnessInput, fitnessPlot)
        {
            nGenerationsWithoutImprovement = 0;
        }
    }
}