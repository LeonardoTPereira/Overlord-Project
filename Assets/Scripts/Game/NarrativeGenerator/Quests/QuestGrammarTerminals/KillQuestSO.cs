using System.Collections.Generic;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using ScriptableObjects;
using System;
using System.Linq;
using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
using Util;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    [Serializable]
    public class KillQuestSO : QuestSO
    {
        [field: SerializeField]
        public EnemiesByType EnemiesToKillByType { get; set; }
        public Dictionary<float, int> EnemiesToKillByFitness { get; set; }

        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> killSymbolWeights = new Dictionary<string, Func<int, int>>();
                killSymbolWeights.Add( Constants.KILL_TERMINAL, Constants.OneOptionQuestLineWeight );
                killSymbolWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return killSymbolWeights;
            } 
        }
        public override string symbolType {
            get { return Constants.KILL_TERMINAL; }
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
        
        public override void Init(QuestSO copiedQuest)
        {
            base.Init(copiedQuest);
            EnemiesToKillByType = new EnemiesByType ();
            foreach (var enemyByType in (copiedQuest as KillQuestSO).EnemiesToKillByType.EnemiesByTypeDictionary)
            {
                EnemiesToKillByType.EnemiesByTypeDictionary.Add(enemyByType.Key, enemyByType.Value);
            }
        }
        
        public override QuestSO Clone()
        {
            var cloneQuest = CreateInstance<KillQuestSO>();
            cloneQuest.Init(this);
            return cloneQuest;
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

        public void SubtractEnemy(WeaponTypeSO weaponTypeSo)
        {
            EnemiesToKillByType.EnemiesByTypeDictionary[weaponTypeSo]--;
        }

        public bool CheckIfCompleted()
        {
            return EnemiesToKillByType.EnemiesByTypeDictionary.All(enemyToKill => enemyToKill.Value == 0);
        }

        public bool HasEnemyToKill(WeaponTypeSO weaponTypeSo)
        {
            if (!EnemiesToKillByType.EnemiesByTypeDictionary.TryGetValue(weaponTypeSo, out var killsLeft)) return false;
            return killsLeft > 0;
        }
    }
}
