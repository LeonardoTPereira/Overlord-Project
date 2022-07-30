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
    public class MasteryQuestSO : QuestSO
    {
        public override string symbolType {
            get { return Constants.MASTERY_QUEST; }
        }

        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> masteryQuestWeights = new Dictionary<string, Func<int, int>>();
                masteryQuestWeights.Add( Constants.KILL_QUEST, Constants.TwoOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.DAMAGE_QUEST, Constants.TwoOptionQuestLineWeight );
                masteryQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
                return masteryQuestWeights;
            } 
        }

        public override void DefineQuestSO ( MarkovChain chain, List<QuestSO> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( chain.GetLastSymbol().symbolType )
            {
                case Constants.KILL_QUEST:
                    CreateAndSaveKillQuestSo(questSos, enemyTypes);
                break;
                case Constants.DAMAGE_QUEST:
                    CreateAndSaveDamageQuestSo(questSos, enemyTypes);
                break;
                default:
                    Debug.LogError("help something went wrong!");
                break;
            }
        }

        public static void CreateAndSaveKillQuestSo(List<QuestSO> questSos, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var killQuest = ScriptableObject.CreateInstance<KillQuestSO>();
            var selectedEnemyTypes = new EnemiesByType ();
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var nEnemiesToKill = RandomSingleton.GetInstance().Random.Next(5) + 5;
            selectedEnemyTypes.EnemiesByTypeDictionary.Add(selectedEnemyType, nEnemiesToKill);
            killQuest.Init(EnemyTypesToString(selectedEnemyTypes), false, questSos.Count > 0 ? questSos[^1] : null, selectedEnemyTypes);
            //killQuest.SaveAsAsset();
            if (questSos.Count > 0)
            {
                questSos[^1].Next = killQuest;
            }
            
            questSos.Add(killQuest);
        }

        public static void CreateAndSaveDamageQuestSo(List<QuestSO> questSos, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var killQuest = ScriptableObject.CreateInstance<DamageQuestSO>();
            var selectedEnemyTypes = new EnemiesByType ();
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var nEnemiesToKill = RandomSingleton.GetInstance().Random.Next(5) + 5;
            selectedEnemyTypes.EnemiesByTypeDictionary.Add(selectedEnemyType, nEnemiesToKill);
            killQuest.Init(EnemyTypesToString(selectedEnemyTypes), false, questSos.Count > 0 ? questSos[^1] : null, selectedEnemyTypes);
            //killQuest.SaveAsAsset();
            if (questSos.Count > 0)
            {
                questSos[^1].Next = killQuest;
            }
            
            questSos.Add(killQuest);
        }

        private static string EnemyTypesToString(EnemiesByType  selectedEnemyTypes)
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
    }
}