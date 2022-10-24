using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;
using System.Linq;
using System.Text;
using Game.ExperimentControllers;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using MyBox;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class MasteryQuestSo : QuestSo
    {
        public override string SymbolType => Constants.MasteryQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get
            {                    
                Dictionary<string, Func<int, float>> masteryQuestWeights = new Dictionary<string, Func<int, float>>();
                masteryQuestWeights.Add( nameof(KillQuestSo), Constants.OneOptionQuestLineWeight );
               //masteryQuestWeights.Add( Constants.DAMAGE_QUEST, Constants.TwoOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.EmptyQuest, Constants.OneOptionQuestEmptyWeight );
                return masteryQuestWeights;
            } 
        }

        public override QuestSo DefineQuestSo ( List<QuestSo> questSos, in GeneratorSettings generatorSettings)
        {
            switch ( SymbolType )
            {
                case Constants.KillQuest:
                    return CreateAndSaveKillQuestSo(questSos, generatorSettings.PossibleWeapons, generatorSettings.EnemiesToKill);
                case Constants.DamageQuest:
                    return CreateAndSaveDamageQuestSo(questSos, generatorSettings.PossibleWeapons);
                default:
                    Debug.LogError("help something went wrong! - Mastery doesn't contain symbol: "+SymbolType);
                break;
            }

            return null;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void CreateQuestString()
        {
            throw new NotImplementedException();
        }

        private static KillQuestSo CreateAndSaveKillQuestSo(List<QuestSo> questSos, WeaponTypeRuntimeSetSO enemyTypes, RangedInt enemiesToKill)
        {
            var killQuest = CreateInstance<KillQuestSo>();
            var selectedEnemyTypes = new EnemiesByType ();
            var questId = killQuest.GetInstanceID();
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var nEnemiesToKill = RandomSingleton.GetInstance().Random.Next( enemiesToKill.Max - enemiesToKill.Min) + enemiesToKill.Min;
            for (var i = 0; i < nEnemiesToKill; i++)
            {
                selectedEnemyTypes.EnemiesByTypeDictionary.AddItemWithId(selectedEnemyType, questId);
            }
            killQuest.Init(KillEnemyTypesToString(selectedEnemyTypes), false, questSos.Count > 0 
                ? questSos[^1] : null, selectedEnemyTypes);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = killQuest;
            }
            
            questSos.Add(killQuest);
            return killQuest;
        }

        private static DamageQuestSo CreateAndSaveDamageQuestSo(List<QuestSo> questSos, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var damageQuest = ScriptableObject.CreateInstance<DamageQuestSo>();
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var totalDamage = RandomSingleton.GetInstance().Random.Next(100) + 20;
            damageQuest.Init(selectedEnemyType.EnemyTypeName, false, 
                questSos.Count > 0 ? questSos[^1] : null, selectedEnemyType, totalDamage);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = damageQuest;
            }
            
            questSos.Add(damageQuest);
            return damageQuest;
        }

        private static string KillEnemyTypesToString(EnemiesByType  selectedEnemyTypes)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < selectedEnemyTypes.EnemiesByTypeDictionary.Count; i++)
            {
                var typeAmountPair = selectedEnemyTypes.EnemiesByTypeDictionary.ElementAt(i);
                stringBuilder.Append($"Kill {typeAmountPair.Value} {typeAmountPair.Key}");
                if (typeAmountPair.Value.QuestIds.Count > 1)
                {
                    stringBuilder.Append("s");
                }
                if (i < (selectedEnemyTypes.EnemiesByTypeDictionary.Count - 1))
                {
                    stringBuilder.Append(" and ");
                }
            }
            return stringBuilder.ToString();
        }
    }
}