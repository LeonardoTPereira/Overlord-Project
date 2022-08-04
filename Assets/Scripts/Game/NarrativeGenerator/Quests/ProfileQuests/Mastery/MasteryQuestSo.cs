using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;
using System.Linq;
using System.Text;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class MasteryQuestSo : QuestSo
    {
        public override string symbolType {
            get { return Constants.MASTERY_QUEST; }
        }

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

        public override void DefineQuestSo ( List<QuestSo> QuestSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( this.symbolType )
            {
                case Constants.KILL_QUEST:
                    CreateAndSaveKillQuestSo(QuestSos, enemyTypes);
                break;
                case Constants.DAMAGE_QUEST:
                    CreateAndSaveDamageQuestSo(QuestSos, enemyTypes);
                break;
                default:
                    Debug.LogError("help something went wrong!");
                break;
            }
        }

        public static void CreateAndSaveKillQuestSo(List<QuestSo> QuestSos, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var killQuest = ScriptableObject.CreateInstance<KillQuestSo>();
            var selectedEnemyTypes = new EnemiesByType ();
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var nEnemiesToKill = RandomSingleton.GetInstance().Random.Next(5) + 5;
            selectedEnemyTypes.EnemiesByTypeDictionary.Add(selectedEnemyType, nEnemiesToKill);
            killQuest.Init(KillEnemyTypesToString(selectedEnemyTypes), false, QuestSos.Count > 0 ? QuestSos[^1] : null, selectedEnemyTypes);
            
            if (QuestSos.Count > 0)
            {
                QuestSos[^1].Next = killQuest;
            }
            
            QuestSos.Add(killQuest);
        }

        public static void CreateAndSaveDamageQuestSo(List<QuestSo> QuestSos, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var damageQuest = ScriptableObject.CreateInstance<DamageQuestSo>();
            var selectedEnemyTypes = new EnemiesByType ();
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var totalDamage = RandomSingleton.GetInstance().Random.Next(100) + 20;
            selectedEnemyTypes.EnemiesByTypeDictionary.Add(selectedEnemyType, totalDamage);
            damageQuest.Init(DamageEnemyTypesToString(selectedEnemyTypes), false, QuestSos.Count > 0 ? QuestSos[^1] : null, selectedEnemyTypes );
            
            if (QuestSos.Count > 0)
            {
                QuestSos[^1].Next = damageQuest;
            }
            
            QuestSos.Add(damageQuest);
        }

        private static string KillEnemyTypesToString(EnemiesByType  selectedEnemyTypes)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < selectedEnemyTypes.EnemiesByTypeDictionary.Count; i++)
            {
                var typeAmountPair = selectedEnemyTypes.EnemiesByTypeDictionary.ElementAt(i);
                stringBuilder.Append($"Kill {typeAmountPair.Value} {typeAmountPair.Key}");
                if (typeAmountPair.Value > 1)
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

        private static string DamageEnemyTypesToString(EnemiesByType selectedEnemyTypes)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < selectedEnemyTypes.EnemiesByTypeDictionary.Count; i++)
            {
                var typeAmountPair = selectedEnemyTypes.EnemiesByTypeDictionary.ElementAt(i);
                stringBuilder.Append($"Deal {typeAmountPair.Value} damage to {typeAmountPair.Key}");
                if (typeAmountPair.Value > 1)
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