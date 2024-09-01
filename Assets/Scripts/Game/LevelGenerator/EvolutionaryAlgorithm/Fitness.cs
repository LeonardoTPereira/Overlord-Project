using System;
using System.Linq;
using MyBox;
using Util;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    [Serializable]
    public class Fitness
    {
        public float NormalizedResult { get; set; }
        public float Distance { get; set; }
        public float NormalizedDistance { get; set; }
        public float Usage { get; set; }
        public float NormalizedUsage { get; set; }
        public float EnemySparsity { get; set; }
        public float NormalizedEnemySparsity { get; set; }
        public float EnemyStandardDeviation { get; set; }
        public float NormalizedEnemyStandardDeviation { get; set; }
        public float Result { get; set; }

        public FitnessInput DesiredInput { get; set; }
        private const float FitnessPenalty = 100f;

        public Fitness( FitnessInput input )
        {
            DesiredInput = input;
            NormalizedResult = Common.UNKNOWN;
            Result = Common.UNKNOWN;
        }
        
        public Fitness( Fitness originalFitness )
        {
            DesiredInput = originalFitness.DesiredInput;
            NormalizedResult = originalFitness.NormalizedResult;
            Distance = originalFitness.Distance;
            NormalizedDistance = originalFitness.NormalizedDistance;
            Usage = originalFitness.Usage;
            NormalizedUsage = originalFitness.NormalizedUsage;
            EnemySparsity = originalFitness.EnemySparsity;
            NormalizedEnemySparsity = originalFitness.NormalizedEnemySparsity;
            EnemyStandardDeviation = originalFitness.EnemyStandardDeviation;
            NormalizedEnemyStandardDeviation = originalFitness.NormalizedEnemyStandardDeviation;
            Result = originalFitness.Result;
        }
        
        public void Calculate(Individual individual, FitnessRange fitnessRange) {
            var dungeon = individual.dungeon;
            CalculateDistanceFromInput(individual, fitnessRange.distanceToInput, dungeon);
            CalculateUsage(individual, fitnessRange.usage, dungeon);
            CalculateSparsity(fitnessRange.sparsity, dungeon);
            CalculateStandardDeviation(fitnessRange.standardDeviation, dungeon);
            UpdateResult();
        }
        
        public void Calculate(Individual individual) {
            var dungeon = individual.dungeon;
            DistanceFromInput(individual, dungeon);
            GetUsageOfRoomsAndLocks(individual, dungeon);
            EvolutionaryAlgorithm.EnemySparsity.GetEnemySparsity(dungeon);
            StdDevEnemyByRoom(dungeon, DesiredInput.DesiredEnemies);
            UpdateResult(false);
        }

        private void CalculateDistanceFromInput(Individual individual, RangedFloat fitnessRange, Dungeon dungeon)
        {
            Distance = DistanceFromInput(individual, dungeon);
            NormalizedDistance = Normalizer.GetMinMaxNormalization(Distance, 0, fitnessRange.Max);
        }        
        
        private void CalculateUsage(Individual individual, RangedFloat fitnessRange, Dungeon dungeon)
        {
            Usage = GetUsageOfRoomsAndLocks(individual, dungeon);
            NormalizedUsage = Normalizer.GetMinMaxNormalization(Usage, 0, fitnessRange.Max);
        }        
        
        private void CalculateSparsity(RangedFloat fitnessRange, Dungeon dungeon)
        {
            EnemySparsity = EvolutionaryAlgorithm.EnemySparsity.GetEnemySparsity(dungeon);
            NormalizedEnemySparsity = Normalizer.GetMinMaxNormalization(EnemySparsity, 0, fitnessRange.Max);
        }        
        private void CalculateStandardDeviation(RangedFloat fitnessRange, Dungeon dungeon)
        {
            EnemyStandardDeviation = StdDevEnemyByRoom(dungeon, DesiredInput.DesiredEnemies);
            NormalizedEnemyStandardDeviation = Normalizer.GetMinMaxNormalization(EnemyStandardDeviation, 0, fitnessRange.Max);
        }

        public override string ToString()
        {
            return $"Total = {NormalizedResult}, Distance = {NormalizedDistance}, Usage = {NormalizedUsage}" +
                   $", Sparsity = {NormalizedEnemySparsity}, Std Dev = {NormalizedEnemyStandardDeviation}";
        }

        private float GetUsageOfRoomsAndLocks(Individual individual, Dungeon dungeon, float desiredPercentage = 1.0f)
        {
            float fNeededLocks;
            if (dungeon.LockIds.Count > 0)
            {
                fNeededLocks = Math.Abs(dungeon.LockIds.Count * desiredPercentage - individual.neededLocks)/(dungeon.LockIds.Count * desiredPercentage);
            }
            else
            {
                fNeededLocks = individual.neededLocks;
            }
            var fNeededRooms = Math.Abs(dungeon.Rooms.Count * desiredPercentage - individual.neededRooms)/(dungeon.Rooms.Count * desiredPercentage);
            var result = fNeededLocks + fNeededRooms;
            return result;
        }

        private float DistanceFromInput(Individual individual, Dungeon dungeon)
        {
            var rooms = dungeon.Rooms.Count;
            var keys = dungeon.KeyIds.Count;
            var locks = dungeon.LockIds.Count;
            var linearCoefficient = individual.linearCoefficient;
            var fRooms = Math.Abs(DesiredInput.DesiredRooms - rooms) / (float) DesiredInput.DesiredRooms;
            var fKeys = Math.Abs(DesiredInput.DesiredKeys - keys) / (float) DesiredInput.DesiredKeys;
            var fLocks = Math.Abs(DesiredInput.DesiredLocks - locks) / (float) DesiredInput.DesiredLocks;
            var fLinearCoefficient = Math.Abs(DesiredInput.DesiredLinearity - linearCoefficient) /
                                 DesiredInput.DesiredLinearity;
            var distance = fRooms + fKeys + fLocks + fLinearCoefficient;
            return distance;
        }

        /// Return the standard deviation of number of enemies by room.
        private static float StdDevEnemyByRoom(in Dungeon dungeon, in int enemies)
        {
            var roomsWithEnemies = dungeon.Rooms.Count(room => room.Enemies > 0);
            if (roomsWithEnemies == 0) return FitnessPenalty;
            var mean = enemies/(float)roomsWithEnemies;
            var std = dungeon.Rooms.Where(room => room.Enemies > 0).Sum(room => (float) Math.Pow(room.Enemies - mean, 2));
            var coefficientOfVariation = (float) Math.Sqrt(std / roomsWithEnemies)/Math.Abs(mean);
            return coefficientOfVariation;
        }

        public bool IsBetter(Fitness other)
        {
            return Result < other.Result;
        }

        public void UpdateResult(bool normalized = true)
        {
            if (normalized)
            {
                NormalizedResult = 3*NormalizedDistance + 2*NormalizedUsage + NormalizedEnemySparsity + NormalizedEnemyStandardDeviation;
            }
            Result = 3*Distance + 2*Usage + EnemySparsity + EnemyStandardDeviation;
        }
    }
}