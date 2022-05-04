using System;
using System.Linq;
using UnityEngine;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    [Serializable]
    public class Fitness
    {
        public float Result { get; set; }
        public float Distance { get; set; }
        public float Usage { get; set; }
        public float EnemySparsity { get; set; }
        public float EnemyStandardDeviation { get; set; }
        public FitnessParameters DesiredParameters { get; set; }

        public Fitness( FitnessParameters parameters )
        {
            DesiredParameters = parameters;
            Result = Common.UNKNOWN;
        }
        
        public void Calculate(Individual individual) {
            var dungeon = individual.dungeon;
            Distance = DistanceFromInput(individual, dungeon);
            Usage = GetUsageOfRoomsAndLocks(individual, dungeon);
            EnemySparsity = 1.0f - EvolutionaryAlgorithm.EnemySparsity.GetEnemySparsity(dungeon, DesiredParameters.DesiredEnemies);
            EnemyStandardDeviation = StdDevEnemyByRoom(dungeon, DesiredParameters.DesiredEnemies);
            Result = Distance + Usage + EnemySparsity + EnemyStandardDeviation;
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
            var fRooms = Math.Abs(DesiredParameters.DesiredRooms - rooms) / (float) DesiredParameters.DesiredRooms;
            var fKeys = Math.Abs(DesiredParameters.DesiredKeys - keys) / (float) DesiredParameters.DesiredKeys;
            var fLocks = Math.Abs(DesiredParameters.DesiredLocks - locks) / (float) DesiredParameters.DesiredLocks;
            var fLinearCoefficient = Math.Abs(DesiredParameters.DesiredLinearity - linearCoefficient) /
                                 DesiredParameters.DesiredLinearity;
            var distance = fRooms + fKeys + fLocks + fLinearCoefficient;
            return distance;
        }

        /// Return the standard deviation of number of enemies by room.
        private static float StdDevEnemyByRoom(in Dungeon dungeon, in int enemies)
        {
            var roomsWithEnemies = dungeon.Rooms.Count(room => room.Enemies > 0);
            if (roomsWithEnemies == 0) return 1;
            var mean = enemies/(float)roomsWithEnemies;
            var std = dungeon.Rooms.Where(room => room.Enemies > 0).Sum(room => (float) Math.Pow(room.Enemies - mean, 2));
            var result = (float) Math.Sqrt(std / roomsWithEnemies)/mean;
            return result;
        }

        public bool IsBetter(Fitness other)
        {
            return Result < other.Result;
        }
    }
}