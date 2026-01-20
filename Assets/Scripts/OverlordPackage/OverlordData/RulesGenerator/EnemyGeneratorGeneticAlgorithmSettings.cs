using System;
using UnityEngine;

namespace Overlord.RulesGenerator.EnemyGeneration
{
    [System.Serializable]
    public class EnemyGeneratorGeneticAlgorithmSettings
    {
        [HideInInspector] public int numberOfMovements = 7;
        [HideInInspector] public int numberOfWeapons = 6;
        public int maxGenerations = 500;
        public int initialPopulationSize = 35;
        public int intermediatePopulationSize = 100;
        public int mutationRate = 20;
        public int geneMutationRate = 30;
        public int numberOfCompetitors = 2;
        public int numberOfDesiredElitesPerEnemy = 3;
        public float minimumAcceptableFitnessPerEnemy = 0.5f;
        public float difficulty;
    }
}