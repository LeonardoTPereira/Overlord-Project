using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;
using System;

namespace Game.NarrativeGenerator.Quests.QuestGrammarNonterminals
{
    public class Kill : NonTerminalQuest
    {
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
            get { return Constants.KILL_QUEST; }
        }
        public void DefineQuestSO ( List<QuestSO> questSos, WeaponTypeRuntimeSetSO enemyTypes )
        {
            CreateAndSaveKillQuestSo( questSos, enemyTypes );
        }
        private static void CreateAndSaveKillQuestSo(List<QuestSO> questSos, WeaponTypeRuntimeSetSO enemyTypes)
        {
            var killQuest = ScriptableObject.CreateInstance<KillQuestSO>();
            var selectedEnemyTypes = new EnemiesByType ();
            //TODO select more enemies
            var selectedEnemyType = enemyTypes.GetRandomItem();
            var nEnemiesToKill = RandomSingleton.GetInstance().Random.Next(5) + 20;
            selectedEnemyTypes.EnemiesByTypeDictionary.Add(selectedEnemyType, nEnemiesToKill);
            killQuest.Init(EnemyTypesToString(selectedEnemyTypes), false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, selectedEnemyTypes);
            //killQuest.SaveAsAsset();
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