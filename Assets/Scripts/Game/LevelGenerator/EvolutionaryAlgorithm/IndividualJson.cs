using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using UnityEngine;

namespace Game.LevelGenerator.EvolutionaryAlgorithm
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