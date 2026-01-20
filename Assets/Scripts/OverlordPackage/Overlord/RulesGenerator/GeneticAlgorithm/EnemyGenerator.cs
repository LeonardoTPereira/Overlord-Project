using System;
using System.Collections.Generic;
using System.Diagnostics;
using Util;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    //EnemyGeneticAlgorithm
    public class EnemyGenerator
    {
        private static readonly int CROSSOVER_PARENTS = 2;

        private EnemyGeneratorGeneticAlgorithmSettings _parameters;
        private SearchSpaceConfig _searchSpace;
        private Population _solution;
        private GeneticAlgorithmData _data;
        private readonly IEnemyFitness _fitnessFunction;
        public Population Solution { get => _solution; }
        public GeneticAlgorithmData Data { get => _data; }

        public EnemyGenerator(EnemyGeneratorGeneticAlgorithmSettings parameters, SearchSpaceConfig searchSpace, IEnemyFitness fitnessFunction)
        {
            _parameters = parameters;
            _searchSpace = searchSpace;
            _fitnessFunction = fitnessFunction;
            _data = new GeneticAlgorithmData
            {
                geneticAlgorithmSettings = _parameters
            };
            _fitnessFunction = fitnessFunction;
        }

        public Population Evolve()
        {
            DateTime start = DateTime.Now;
            Evolution();
            DateTime end = DateTime.Now;
            _data.duration = (end - start).TotalSeconds;
            return _solution;
        }

        private void Evolution()
        {            
            Population pop = new Population(
                _parameters.numberOfMovements,
                _parameters.numberOfWeapons,
                _fitnessFunction
            );
            if (_parameters.initialPopulationSize > _parameters.numberOfMovements * _parameters.numberOfWeapons)
            {
                UnityEngine.Debug.Log("Initial population size is larger than the search space. Changing it for nxm size (n=numberOfMovements, m=numberOfWeapons)");
                _parameters.initialPopulationSize = _parameters.numberOfMovements * _parameters.numberOfWeapons;
            }
            while (pop.Count() < _parameters.initialPopulationSize)
            {
                Individual ind = Individual.GetRandom(_searchSpace);
                _fitnessFunction.SetSearchSpace(_searchSpace);
                _fitnessFunction.Calculate(ref ind, _parameters.difficulty);
                pop.PlaceIndividual(ind);
            }
            _data.initialPopulation = new List<Individual>(pop.ToList());

            var currentGeneration = 0;
            while (!HasReachedStopCriteria(currentGeneration, pop.MinimumElitesOfEachType(), pop.NIndividualsBetterThan(_parameters.numberOfDesiredElitesPerEnemy, _parameters.minimumAcceptableFitnessPerEnemy)))
            {
                List<Individual> intermediate = new List<Individual>();
                while (intermediate.Count < _parameters.intermediatePopulationSize)
                {
                    Individual[] parents = Selection.Select(CROSSOVER_PARENTS, _parameters.numberOfCompetitors, pop, _fitnessFunction);
                    Individual[] offspring = Crossover.Apply(parents[0], parents[1], _searchSpace);

                    if (_parameters.mutationRate > RandomSingleton.GetInstance().RandomPercent())
                    {
                        parents[0] = offspring[0];
                        offspring[0] = Mutation.Apply(parents[0], _parameters.geneMutationRate, _searchSpace);
                        parents[1] = offspring[1];
                        offspring[1] = Mutation.Apply(parents[1], _parameters.geneMutationRate, _searchSpace);
                    }

                    for (int i = 0; i < offspring.Length; i++)
                    {
                        //Difficulty.Calculate(ref offspring[i]);
                        _fitnessFunction.Calculate(ref offspring[i], _parameters.difficulty);
                        intermediate.Add(offspring[i]);
                    }
                }

                foreach (Individual individual in intermediate)
                {
                    individual.Generation = currentGeneration;
                    pop.PlaceIndividual(individual);
                }

                if (currentGeneration == _parameters.maxGenerations / 2)
                {
                    _data.intermediatePopulation = new List<Individual>(pop.ToList());
                }
                currentGeneration++;
            }

            _solution = pop;
            _data.finalPopulation = new List<Individual>(_solution.ToList());
        }

        private bool HasReachedStopCriteria(int generation, int totalElitesPerType, float elitesWithAcceptableFitnessPerType)
        {
            if (totalElitesPerType < _parameters.numberOfDesiredElitesPerEnemy) return false;
            if (elitesWithAcceptableFitnessPerType >= _parameters.numberOfDesiredElitesPerEnemy) return true;
            return generation > _parameters.maxGenerations;
        }
    }
}