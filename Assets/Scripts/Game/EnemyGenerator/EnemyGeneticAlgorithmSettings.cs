using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.EnemyGenerator
{
    [System.Serializable]
    public class EnemyGeneratorGeneticAlgorithmSettings
    {
        public int maxGenerations = 500;
        public int initialPopulationSize = 35;
        public int intermediatePopulationSize = 100;
        public int mutationRate = 20;
        public int geneMutationRate = 30;
        public int numberOfCompetitors = 2;
        public int numberOfDesiredElitesPerEnemy = 3;
        public float minimumAcceptableFitnessPerEnemy = 0.5f;
    }
}