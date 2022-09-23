﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
{
    public static class EnemySparsity
    {
        private struct EnemyDistance
        {
            public int EnemyAmount { get; }
            public float Distance { get; }

            public EnemyDistance(float distance, int enemies)
            {
                EnemyAmount = enemies;
                Distance = distance;
            }
        }

        private static int TotalEnemies { get; set; }

        public static float GetEnemySparsity(in Dungeon dungeon, in int totalEnemies)
        {
            TotalEnemies = totalEnemies;
            
            var averagePosition = GetAveragePositionOfEnemies(dungeon);
            
            var distances = GetEnemyDistances(dungeon, averagePosition);

            var distanceAvg = GetAverageDistance(distances);

            if (distanceAvg == 0) return 0;
            
            var distanceStdDev = GetDistanceStdDev(distances, distanceAvg);
            
            var result = distanceStdDev / distanceAvg;
            
            return result;
        }

        private static float GetAverageDistance(List<EnemyDistance> distances)
        {
            var average = 0.0f;
            var total = 1;
            foreach (var distance in distances)
            {
                average += (distance.Distance * distance.EnemyAmount - average)/total;
                total++;
            }

            return average;
        }

        private static float GetDistanceStdDev(List<EnemyDistance> distances, float distanceAvg)
        {
            var stdDev = 0.0f;
            foreach (var distance in distances)
            {
                stdDev += (float)Math.Pow(distance.Distance - distanceAvg, 2)*distance.EnemyAmount;
            }
            stdDev /= TotalEnemies;
            var result = (float) Math.Sqrt(stdDev);
            return result;
        }

        private static List<EnemyDistance> GetEnemyDistances(Dungeon dungeon, Vector2 averagePosition)
        {
            var distances = new List<EnemyDistance>();
            foreach (var room in dungeon.Rooms)
            {
                Vector2 roomPosition;
                roomPosition.x = room.X + dungeon.MinX;
                roomPosition.y = room.Y + dungeon.MinY;
                var distance = Vector2.Distance(roomPosition, averagePosition);
                distances.Add(new EnemyDistance(distance, room.Enemies));
            }
            return distances;
        }

        private static Vector2 GetAveragePositionOfEnemies(Dungeon dungeon)
        {
            var averagePosition = Vector2.zero;
            // Calculate the average position of enemies
            foreach (var room in dungeon.Rooms)
            {
                Vector2 roomPosition;
                roomPosition.x = room.X + dungeon.MinX;
                roomPosition.y = room.Y + dungeon.MinY;
                averagePosition.x += roomPosition.x * room.Enemies;
                averagePosition.x += roomPosition.y * room.Enemies;
            }
            averagePosition.x /= TotalEnemies;
            averagePosition.y /= TotalEnemies;
            
            return averagePosition;
        }
    }
}