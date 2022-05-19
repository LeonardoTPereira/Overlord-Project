using System.Collections.Generic;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using ScriptableObjects;
using System;
using Util;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class KillQuestSO : QuestSO
    {
        public EnemiesByType  EnemiesToKillByType { get; set; }
        public Dictionary<float, int> EnemiesToKillByFitness { get; set; }

        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> killSymbolWeights = new Dictionary<string, Func<int, int>>();
                killSymbolWeights.Add( Constants.KILL_TERMINAL, Constants.OneOptionQuestLineWeight );
                killSymbolWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return killSymbolWeights;
            } 
            set {}
        }
        
        public override void Init()
        {
            base.Init();
            EnemiesToKillByType = new EnemiesByType ();
            EnemiesToKillByFitness = new Dictionary<float, int>();
        }

        public void Init(string questName, bool endsStoryLine, QuestSO previous, EnemiesByType  enemiesByType)
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToKillByType = enemiesByType;
        }
        public void Init(string questName, bool endsStoryLine, QuestSO previous, Dictionary<float, int> enemiesByFitness)
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToKillByFitness = enemiesByFitness;
        }
        
        public void AddEnemy(WeaponTypeSO enemy, int amount)
        {
            if (EnemiesToKillByType.EnemiesByTypeDictionary.TryGetValue(enemy, out var currentAmount))
            {
                EnemiesToKillByType.EnemiesByTypeDictionary[enemy] = currentAmount + amount;
            }
            else
            {
                EnemiesToKillByType.EnemiesByTypeDictionary.Add(enemy, amount);
            }
        }
        
        public void AddEnemy(float enemyFitness, int amount)
        {
            if (EnemiesToKillByFitness.TryGetValue(enemyFitness, out var currentAmount))
            {
                EnemiesToKillByFitness[enemyFitness] = currentAmount + amount;
            }
            else
            {
                EnemiesToKillByFitness.Add(enemyFitness, amount);
            }
        }
    }
}
