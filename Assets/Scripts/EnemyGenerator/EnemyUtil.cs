using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyGenerator
{
    public static class EnemyUtil
    {
        //The population size of the EA
        public static int popSize = 10000;
        public const int crossChance = 99;
        public const int mutChance = 10;
        public static int maxGenerations = 30;
        public static float desiredFitness;
        public const float easyFitness = 14.0f;
        public const float mediumFitness = 17.0f;
        public const float hardFitness = 22.5f;
        public const float veryHardFitness = 25.5f;
        public const int nBestEnemies = 20;
        public const int minDamage = 1;
        public const int maxDamage = 4;
        public const int minHealth = 1;
        public const int maxHealth = 6;
        public const float minAtkSpeed = 0.75f;
        public const float maxAtkSpeed = 4.0f;
        public const float minMoveSpeed = 0.8f;
        public const float maxMoveSpeed = 3.2f;
        public const float minActivetime = 1.5f;
        public const float maxActiveTime = 10;
        public const float minResttime = 0.3f;
        public const float maxRestTime = 1.5f;
        public const float minProjectileSpeed = 1;
        public const float maxProjectileSpeed = 4;
    }
}