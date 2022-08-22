using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;
using System.Linq;
using System.Text;
using Game.NarrativeGenerator.EnemyRelatedNarrative;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class MasteryQuestSo : QuestSo
    {
        public override string SymbolType => Constants.MasteryQuest;

        public override Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get {
                if ( nextSymbolChances != null )
                    return nextSymbolChances;
                    
                Dictionary<string, Func<int, int>> masteryQuestWeights = new Dictionary<string, Func<int, int>>();
                masteryQuestWeights.Add( Constants.KILL_QUEST, Constants.TwoOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.DAMAGE_QUEST, Constants.TwoOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
                return masteryQuestWeights;
            } 
        }

        public override QuestSo DefineQuestSo ( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( SymbolType )
            {
                case Constants.KILL_QUEST:
                    return CreateAndSaveKillQuestSo(questSos, enemyTypes);
                case Constants.DAMAGE_QUEST:
                    return CreateAndSaveDamageQuestSo(questSos, enemyTypes);
                default:
                    Debug.LogError("help something went wrong!");
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

        private static KillQuestSo CreateAndSaveKillQuestSo(List<QuestSo> questSos, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var killQuest = CreateInstance<KillQuestSo>();
            var selectedEnemyTypes = new EnemiesByType ();
            var questId = killQuest.GetInstanceID();
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var nEnemiesToKill = RandomSingleton.GetInstance().Random.Next(5) + 5;
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
                if (typeAmountPair.Value.Count > 1)
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

        private static string DamageEnemyTypesToString(DamageQuestData damageData)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Deal {damageData.Damage} damage to {damageData.Enemy.EnemyTypeName}");
            return stringBuilder.ToString();
        }
    }
}