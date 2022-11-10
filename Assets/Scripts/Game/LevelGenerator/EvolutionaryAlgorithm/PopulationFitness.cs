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
            distanceToInput = new RangedFloat(0, float.MinValue),
            usage = new RangedFloat(0, float.MinValue),
            standardDeviation = new RangedFloat(0, float.MinValue),
            sparsity = new RangedFloat(0, float.MinValue)
        };
        private static FitnessRange _currentPopulationFitnessRange = new()
        {
            distanceToInput = new RangedFloat(0, float.MinValue),
            usage = new RangedFloat(0, float.MinValue),
            standardDeviation = new RangedFloat(0, float.MinValue),
            sparsity = new RangedFloat(0, float.MinValue)
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
                ChangeEliteIfBetter(individual);
                UpdateCurrentPopulationFitnessRange(individual.Fitness);
            }
            UpdateRanges();
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

        private static void UpdateCurrentPopulationFitnessRange(Fitness fitness)
        {
            UpdateRange(fitness.Distance, ref _currentPopulationFitnessRange.distanceToInput);
            UpdateRange(fitness.Usage, ref _currentPopulationFitnessRange.usage);
            UpdateRange(fitness.EnemySparsity, ref _currentPopulationFitnessRange.sparsity);
            UpdateRange(fitness.EnemyStandardDeviation, ref _currentPopulationFitnessRange.standardDeviation);
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

        private static void UpdateRanges()
        {
            _hasUpdateInDistance = UpdateRange(_currentPopulationFitnessRange.distanceToInput.Min, ref _currentFitnessRange.distanceToInput);
            _hasUpdateInDistance = _hasUpdateInDistance || UpdateRange(_currentPopulationFitnessRange.distanceToInput.Max, ref _currentFitnessRange.distanceToInput);
            _hasUpdateInUsage = UpdateRange(_currentPopulationFitnessRange.usage.Min, ref _currentFitnessRange.usage);
            _hasUpdateInUsage = _hasUpdateInUsage ||UpdateRange(_currentPopulationFitnessRange.usage.Max, ref _currentFitnessRange.usage);
            _hasUpdateInSparsity = UpdateRange(_currentPopulationFitnessRange.sparsity.Min, ref _currentFitnessRange.sparsity);
            _hasUpdateInSparsity = _hasUpdateInSparsity ||UpdateRange(_currentPopulationFitnessRange.sparsity.Max, ref _currentFitnessRange.sparsity);
            _hasUpdateInStandardDeviation = UpdateRange(_currentPopulationFitnessRange.standardDeviation.Min, ref _currentFitnessRange.standardDeviation);
            _hasUpdateInStandardDeviation = _hasUpdateInStandardDeviation ||UpdateRange(_currentPopulationFitnessRange.standardDeviation.Max, ref _currentFitnessRange.standardDeviation);
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