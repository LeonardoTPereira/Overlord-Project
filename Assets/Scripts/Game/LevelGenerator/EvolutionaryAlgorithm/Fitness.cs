using System;
using System.IO;
using Game.LevelGenerator.EvolutionaryAlgorithm;
using UnityEngine;

namespace Game.LevelGenerator
{
    [Serializable]
    public class Fitness
    {
        /// The fitness factor for the entered user goal.
        public float fGoal;
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
        /// The fitness factor for the enemy standard deviation.
        public float fSTD;
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
        public void Calculate(ref Individual _individual) {
            // Create aliases for the individual's attributes
            Dungeon dungeon = _individual.dungeon;
            int rooms = dungeon.Rooms.Count;
            int keys = dungeon.KeyIds.Count;
            int locks = dungeon.LockIds.Count;
            float linearCoefficient = _individual.linearCoefficient;
            // Calculate the distance between the attributes of the generated
            // dungeon to the entered parameters
            fRooms = Math.Abs(DesiredParameters.DesiredRooms - rooms);
            fKeys = Math.Abs(DesiredParameters.DesiredKeys - keys);
            fLocks = Math.Abs(DesiredParameters.DesiredLocks - locks);
            fLinearCoefficient = Math.Abs(DesiredParameters.DesiredLinearity - linearCoefficient);
            float distance = fRooms + fKeys + fLocks + fLinearCoefficient;
            float fit = 2 * distance;
            // If the level has locked doors
            if (dungeon.LockIds.Count > 0)
            {
                // Calculate the number of locks needed to finish the level
                _individual.neededLocks = AStar.FindRoute(dungeon);
                // Validate the calculated number of needed locks
                if (_individual.neededLocks > dungeon.LockIds.Count)
                {
                    throw new InvalidDataException("Inconsistency! The number of " +
                                        "needed locks is higher than the number of total " +
                                        "locks of the level." +
                                        "\n  Total locks=" + dungeon.LockIds.Count +
                                        "\n  Needed locks=" + _individual.neededLocks);
                }
                fNeededLocks = dungeon.LockIds.Count - _individual.neededLocks;
                // Calculate the number of rooms needed to finish the level
                float neededRooms = 0f;
                for (int i = 0; i < 3; i++)
                {
                    DFS dfs = new DFS(dungeon);
                    dfs.FindRoute();
                    neededRooms += dfs.NVisitedRooms;
                }
                _individual.neededRooms = neededRooms / 3.0f;
                // Validate the calculated number of needed rooms
                if (_individual.neededRooms > dungeon.Rooms.Count)
                {
                    throw new InvalidDataException("Inconsistency! The number of " +
                                                   "needed rooms is higher than the number of total " +
                                                   "rooms of the level." +
                                                   "\n  Total rooms=" + dungeon.Rooms.Count +
                                                   "\n  Needed rooms=" + _individual.neededRooms);
                }
                fNeededRooms = dungeon.Rooms.Count - _individual.neededRooms;
                // Update the fitness by summing the number of needed rooms and
                // the number of needed locks
                fit += fNeededLocks + fNeededRooms;
            }
            fGoal = fit;
            // Update the fitness by subtracting the enemy sparsity
            // (the higher the better)
            float sparsity = -EnemySparsity(dungeon, DesiredParameters.DesiredEnemies);
            fEnemySparsity = sparsity;
            float std = STDEnemyByRoom(dungeon, DesiredParameters.DesiredEnemies);
            fSTD = std;
            fit = fit + sparsity + std;
            result = fit;
        }

        /// Calculate and return the enemy sparsity in the entered dungeon.
        private static float EnemySparsity(
            Dungeon _dungeon,
            int _enemies
        ) {
            // Calculate the average position of enemies
            float avg_x = 0f;
            float avg_y = 0f;
            foreach (Room room in _dungeon.Rooms)
            {
                int xp = room.X + _dungeon.MinX;
                int yp = room.Y + _dungeon.MinY;
                avg_x += xp * room.Enemies;
                avg_y += yp * room.Enemies;
            }
            avg_x = avg_x / _enemies;
            avg_y = avg_y / _enemies;
            // Calculate the sparsity
            float sparsity = 0f;
            foreach (Room room in _dungeon.Rooms)
            {
                int xp = room.X + _dungeon.MinX;
                int yp = room.Y + _dungeon.MinY;
                double dist = 0f;
                dist += Math.Pow(xp - avg_x, 2);
                dist += Math.Pow(yp - avg_y, 2);
                dist *= room.Enemies;
                sparsity += (float) Math.Sqrt(dist);
            }
            return sparsity / _enemies;
        }

        /// Return the standard deviation of number of enemies by room.
        private static float STDEnemyByRoom(
            Dungeon _dungeon,
            int _enemies
        ) {
            // The start and goal rooms are not count because they are mandatory
            // empty rooms
            float rooms = _dungeon.Rooms.Count - 2;
            float mean = _enemies / rooms;
            // Calculate standard deviation
            float std = 0f;
            for (int i = 1; i < _dungeon.Rooms.Count; i++)
            {
                Room room = _dungeon.Rooms[i];
                if (!room.Equals(_dungeon.GetGoal()))
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
        public static bool IsBest(
            Individual _i1,
            Individual _i2
        ) {
            Debug.Assert(
                _i1 != null || _i2 != null,
                Common.CANNOT_COMPARE_INDIVIDUALS
            );
            if (_i1 is null) { return false; }
            if (_i2 is null) { return true; }
            return _i2.Fitness.result > _i1.Fitness.result;
        }
    }
}