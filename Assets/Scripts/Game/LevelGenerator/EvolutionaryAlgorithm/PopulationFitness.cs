using System.Collections.Generic;
using MyBox;
using Util;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public static class PopulationFitness
    {

        public static Individual BestDungeon { get; private set; }
        private static FitnessRange _currentFitnessRange = new()
        {
            distanceToInput = new RangedFloat(float.MaxValue, float.MinValue),
            usage = new RangedFloat(float.MaxValue, float.MinValue),
            standardDeviation = new RangedFloat(float.MaxValue, float.MinValue),
            sparsity = new RangedFloat(float.MaxValue, float.MinValue)
        };
        private static bool _hasUpdateInDistance;
        private static bool _hasUpdateInUsage;
        private static bool _hasUpdateInSparsity;
        private static bool _hasUpdateInStandardDeviation;
        private static float _bestFitness;


        public static void CalculateFitness(List<Individual> population)
        {
            InitializeValues();
            foreach (var individual in population)
            {
                individual.CalculateFitness(_currentFitnessRange);
                UpdateRanges(individual.Fitness);
                ChangeEliteIfBetter(individual);
            }
            ReNormalizeFitnessIfRangeWasUpdated(population);
        }

        private static void InitializeValues()
        {
            _hasUpdateInDistance = false;
            _hasUpdateInUsage = false;
            _hasUpdateInSparsity = false;
            _hasUpdateInStandardDeviation = false;
            _bestFitness = float.MaxValue;
        }

        private static void ReNormalizeFitnessIfRangeWasUpdated(List<Individual> population)
        {
            _bestFitness = float.MaxValue;
            if (HasNoUpdatesInRange()) return;
            foreach (var individual in population)
            {
                ReNormalizeUpdatedParameters(individual.Fitness);
                ChangeEliteIfBetter(individual);
            }
        }

        private static bool HasNoUpdatesInRange()
        {
            return !(_hasUpdateInDistance || _hasUpdateInUsage || _hasUpdateInSparsity || _hasUpdateInStandardDeviation);
        }

        private static void ReNormalizeUpdatedParameters(Fitness fitness)
        {
            if (_hasUpdateInDistance)
            {
                fitness.NormalizedDistance = ReNormalizeValue(fitness.Distance, _currentFitnessRange.distanceToInput);
            }

            if (_hasUpdateInUsage)
            {
                fitness.NormalizedUsage = ReNormalizeValue(fitness.Usage, _currentFitnessRange.usage);
            }

            if (_hasUpdateInSparsity)
            {
                fitness.NormalizedEnemySparsity = ReNormalizeValue(fitness.EnemySparsity,
                    _currentFitnessRange.sparsity);
            }

            if (_hasUpdateInStandardDeviation)
            {
                fitness.NormalizedEnemyStandardDeviation = ReNormalizeValue(fitness.EnemyStandardDeviation,
                    _currentFitnessRange.standardDeviation);
            }

            fitness.UpdateResult();
        }

        private static float ReNormalizeValue(float value, RangedFloat newRange)
        {
            return Normalizer.GetMinMaxNormalization(value, newRange.Min, newRange.Max);
        }

        private static void ChangeEliteIfBetter(Individual individual)
        {
            var currentFitness = individual.Fitness.Result;
            if (_bestFitness < currentFitness) return;
            _bestFitness = currentFitness;
            BestDungeon = individual;
        }

        private static void UpdateRanges(Fitness fitness)
        {
            _hasUpdateInDistance = UpdateRange(fitness.Distance, ref _currentFitnessRange.distanceToInput);
            _hasUpdateInUsage = UpdateRange(fitness.Usage, ref _currentFitnessRange.usage);
            _hasUpdateInSparsity = UpdateRange(fitness.EnemySparsity, ref _currentFitnessRange.sparsity);
            _hasUpdateInStandardDeviation = UpdateRange(fitness.EnemyStandardDeviation, ref _currentFitnessRange.standardDeviation);
        }

        private static bool UpdateRange(float newValue, ref RangedFloat range)
        {
            var hasChanged = false;
            if (newValue < range.Min)
            {
                range.Min = newValue;
                hasChanged = true;
            }

            if (newValue <= range.Max) return hasChanged;
            range.Max = newValue;
            return true;
        }
    }
}