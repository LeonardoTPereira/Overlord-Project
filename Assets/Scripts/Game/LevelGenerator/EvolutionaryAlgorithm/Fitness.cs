using System;
using System.IO;
using UnityEngine;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    [Serializable]
    public class Fitness
    {
        /// The fitness factor for the number of rooms.
        public float fRooms;
        /// The fitness factor for the number of keys.
        public float fKeys;
        /// The fitness factor for the number of locks.
        public float fLocks;
        /// The fitness factor for the linear coefficient.
        public float fLinearCoefficient;
        /// The fitness factor for the enemy sparsity.
        public float fEnemySparsity;
        /// The fitness factor for the number of needed rooms.
        public float fNeededRooms;
        /// The fitness factor for the number of needed locks.
        public float fNeededLocks;
        public float result;
        public FitnessParameters DesiredParameters { get; set; }

        public Fitness( FitnessParameters parameters )
        {
            DesiredParameters = parameters;
            result = Common.UNKNOWN;
        }

        /// Calculate the fitness value of the entered individual.
        ///
        /// An individual's fitness is defined by two factors: the user aimed
        /// settings and the gameplay factor. The user aimed settings are
        /// measured by the distance of the aimed number of rooms, number of
        /// keys, number of locks and the linear coefficient. The gameplay
        /// factor sums: (1) the distance between the total number of locks
        /// weighted by 0.8 and the number of needed locks to open to finish
        /// the level; (2) the distance between the total number of rooms and
        /// the number of needed rooms to visit to finish the level, and; (3)
        /// the negative value of the sparsity of enemies. The last item is
        /// negative because this fitness aims to minimize its value while
        /// maximizing the sparsity of enemies.
        public void Calculate(Individual individual) {
            // Create aliases for the individual's attributes
            Dungeon dungeon = individual.dungeon;
            int rooms = dungeon.Rooms.Count;
            int keys = dungeon.KeyIds.Count;
            int locks = dungeon.LockIds.Count;
            float linearCoefficient = individual.linearCoefficient;
            fRooms = Math.Abs(DesiredParameters.DesiredRooms - rooms);
            fKeys = Math.Abs(DesiredParameters.DesiredKeys - keys);
            fLocks = Math.Abs(DesiredParameters.DesiredLocks - locks);
            fLinearCoefficient = Math.Abs(DesiredParameters.DesiredLinearity - linearCoefficient);
            float distance = fRooms + fKeys + fLocks + fLinearCoefficient;
            float fit = 2 * distance;
            fNeededLocks = dungeon.LockIds.Count - individual.neededLocks;
            fNeededRooms = dungeon.Rooms.Count - individual.neededRooms;
            fit += fNeededLocks + fNeededRooms;
            // Update the fitness by subtracting the enemy sparsity
            // (the higher the better)
            float sparsity = -EnemySparsity(dungeon, DesiredParameters.DesiredEnemies);
            fEnemySparsity = sparsity;
            float std = StdDevEnemyByRoom(dungeon, DesiredParameters.DesiredEnemies);
            fit = fit + sparsity + std;
            result = fit;
        }

        /// Calculate and return the enemy sparsity in the entered dungeon.
        private static float EnemySparsity( in Dungeon _dungeon, in int _enemies ) {
            // Calculate the average position of enemies
            float avgX = 0f;
            float avgY = 0f;
            foreach (Room room in _dungeon.Rooms)
            {
                int xp = room.X + _dungeon.MinX;
                int yp = room.Y + _dungeon.MinY;
                avgX += xp * room.Enemies;
                avgY += yp * room.Enemies;
            }
            avgX /= _enemies;
            avgY /= _enemies;
            // Calculate the sparsity
            float sparsity = 0f;
            foreach (Room room in _dungeon.Rooms)
            {
                int xp = room.X + _dungeon.MinX;
                int yp = room.Y + _dungeon.MinY;
                double dist = 0f;
                dist += Math.Pow(xp - avgX, 2);
                dist += Math.Pow(yp - avgY, 2);
                dist *= room.Enemies;
                sparsity += (float) Math.Sqrt(dist);
            }
            return sparsity / _enemies;
        }

        /// Return the standard deviation of number of enemies by room.
        private static float StdDevEnemyByRoom(in Dungeon dungeon, in int enemies) {
            // The start and goal rooms are not count because they are mandatory
            // empty rooms
            float rooms = dungeon.Rooms.Count - 2;
            float mean = enemies / rooms;
            // Calculate standard deviation
            float std = 0f;
            for (int i = 1; i < dungeon.Rooms.Count; i++)
            {
                Room room = dungeon.Rooms[i];
                if (!room.Equals(dungeon.GetGoal()))
                {
                    std += (float) Math.Pow(room.Enemies - mean, 2);
                }
            }
            return (float) Math.Sqrt(std / rooms);
        }

        /// Return true if the first individual (`_i1`) is best than the second
        /// (`_i2`), and false otherwise.
        ///
        /// The best is the individual that is closest to the local goal in the
        /// MAP-Elites population. This is, the best is the one that's fitness
        /// has the lesser value. If `_i1` is null, then `_i2` is the best
        /// individual. If `_i2` is null, then `_i1` is the best individual. If
        /// both individuals are null, then the comparison cannot be performed.
        public static bool IsBest(in Individual individual1, in Individual individual2) {
            Debug.Assert(
                individual1 != null || individual2 != null,
                Common.CANNOT_COMPARE_INDIVIDUALS
            );
            if (individual1 is null) { return false; }
            if (individual2 is null) { return true; }
            return individual2.Fitness.result > individual1.Fitness.result;
        }
    }
}