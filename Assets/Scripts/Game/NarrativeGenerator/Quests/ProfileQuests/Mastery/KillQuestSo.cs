using System.Collections.Generic;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using ScriptableObjects;
using System;
using Util;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    [Serializable]
    public class KillQuestSo : MasteryQuestSo
    {
        [field: SerializeField]
        public EnemiesByType EnemiesToKillByType { get; set; }
        public Dictionary<float, int> EnemiesToKillByFitness { get; set; }
        public override string SymbolType => Constants.KILL_QUEST;

        public override void Init()
        {
            base.Init();
            EnemiesToKillByType = new EnemiesByType ();
            EnemiesToKillByFitness = new Dictionary<float, int>();
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, EnemiesByType  enemiesByType)
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToKillByType = enemiesByType;
        }
        public void Init(string questName, bool endsStoryLine, QuestSo previous, Dictionary<float, int> enemiesByFitness)
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToKillByFitness = enemiesByFitness;
        }
        
        public override void Init(QuestSo copiedQuest)
        {
            base.Init(copiedQuest);
            EnemiesToKillByType = new EnemiesByType ();
            var killQuest = copiedQuest as KillQuestSo;
            if (killQuest != null)
            {
                foreach (var enemyByType in killQuest.EnemiesToKillByType.EnemiesByTypeDictionary)
                {
                    EnemiesToKillByType.EnemiesByTypeDictionary.Add(enemyByType.Key, enemyByType.Value);
                }
            }
            else
            {
                throw new ArgumentException(
                    $"Expected argument of type {typeof(KillQuestSo)}, got type {copiedQuest.GetType()}");
            }
        }
        
        public override QuestSo Clone()
        {
            var cloneQuest = CreateInstance<KillQuestSo>();
            cloneQuest.Init(this);
            return cloneQuest;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            return !IsCompleted 
                   &&  EnemiesToKillByType.EnemiesByTypeDictionary.ContainsKey(questElement as WeaponTypeSO 
                   ?? throw new InvalidOperationException());
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            EnemiesToKillByType.EnemiesByTypeDictionary.RemoveItemWithId(questElement as WeaponTypeSO, questId);
            if ( EnemiesToKillByType.EnemiesByTypeDictionary.Count == 0)
            {
                IsCompleted = true;
            }
        }
    }
}
