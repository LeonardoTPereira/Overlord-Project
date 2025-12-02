using System;
using UnityEngine;

namespace Overlord.LevelGenerator.EvolutionaryAlgorithm
{
    [Serializable]
    public class IndividualJson
    {

        [SerializeField] private IndividualJsonList data;

        public IndividualJson()
        {
            data = new IndividualJsonList();
        }

        public void AddFitness(Individual individual)
        {
            data.Add(individual);
        }

        public void SaveJson()
        {
            data.SaveJson();
        }
    }
}