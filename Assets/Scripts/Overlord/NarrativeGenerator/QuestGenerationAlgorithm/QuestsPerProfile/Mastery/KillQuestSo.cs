using System.Collections.Generic;
using Overlord.NarrativeGenerator.EnemyRelatedNarrative;
using ScriptableObjects;
using System;
using System.Text;
using Util;
using UnityEngine;
using static Util.Enums;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    [Serializable]
    public class KillQuestSo : MasteryQuestSo
    {
        [field: SerializeField]
        public EnemiesByType EnemiesToKillByType { get; set; }
        private EnemiesByType OriginalEnemiesToKillByType { get; set; }
        public Dictionary<float, int> EnemiesToKillByFitness { get; set; }
        public override string SymbolType => Constants.KillQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get => _nextSymbolChances;
            set => _nextSymbolChances = value;
        }

        public override void Init()
        {
            base.Init();
            EnemiesToKillByType = new EnemiesByType();
            EnemiesToKillByFitness = new Dictionary<float, int>();
            OriginalEnemiesToKillByType = new EnemiesByType();
        }

        public void Init(string questName, bool endsStoryLine, QuestSo previous, EnemiesByType enemiesByType)
        {
            base.Init(questName, endsStoryLine, previous);
            EnemiesToKillByType = enemiesByType;
            OriginalEnemiesToKillByType = enemiesByType;
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
            OriginalEnemiesToKillByType = new EnemiesByType();
            var killQuest = copiedQuest as KillQuestSo;
            if (killQuest != null)
            {
                EnemiesToKillByType.EnemiesByTypeDictionary = (WeaponTypeAmountDictionary) killQuest.EnemiesToKillByType.EnemiesByTypeDictionary.Clone();
                OriginalEnemiesToKillByType.EnemiesByTypeDictionary = (WeaponTypeAmountDictionary) killQuest.EnemiesToKillByType.EnemiesByTypeDictionary.Clone();
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

        public override string GetEnemyAmountString(Language language)
        {
            CreateQuestString(language);
            return QuestText;
        }

        public override string GetEnemyString(Language language)
        {
            var stringBuilder = new StringBuilder();
            foreach (var enemyByAmount in EnemiesToKillByType.EnemiesByTypeDictionary)
            {
                var spriteString = enemyByAmount.Key.GetEnemySpriteString(language);
                stringBuilder.Append($"{enemyByAmount.Key.RealTypeName(language)}s {spriteString}, ");
            }

            if (stringBuilder.Length == 0)
            {
                QuestText = stringBuilder.ToString();
            }
            
            if (stringBuilder.Length > 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            return stringBuilder.ToString();
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            if (questId != Id) return false;
            return !IsCompleted 
                   &&  EnemiesToKillByType.EnemiesByTypeDictionary.ContainsKey(questElement as WeaponTypeSo 
                                                                               ?? throw new InvalidOperationException());
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            EnemiesToKillByType.EnemiesByTypeDictionary.RemoveItemWithId(questElement as WeaponTypeSo, questId);
            Debug.Log(EnemiesToKillByType.EnemiesByTypeDictionary.Count);
            if ( EnemiesToKillByType.EnemiesByTypeDictionary.Count == 0)
            {
                IsCompleted = true;
            }
        }

        public override void CreateQuestString(Language language)
        {
            var stringBuilder = new StringBuilder();

            if (language == Language.Portuguese)
                stringBuilder.Append("Derrote ");
            else
                stringBuilder.Append("Kill ");

            foreach (var enemyByAmount in OriginalEnemiesToKillByType.EnemiesByTypeDictionary)
            {
                var spriteString = enemyByAmount.Key.GetEnemySpriteString(language);
                stringBuilder.Append($"{enemyByAmount.Value.QuestIds.Count} {enemyByAmount.Key.RealTypeName(language)}s {spriteString}, ");
            }

            if (stringBuilder.Length == 0)
            {
                QuestText = stringBuilder.ToString();
            }
            
            if (stringBuilder.Length > 2)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }
            QuestText = stringBuilder.ToString();
        }
    }
}