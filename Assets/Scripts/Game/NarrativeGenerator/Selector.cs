using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataCollection;
using Game.Events;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;
using Enums = Util.Enums;

namespace Game.NarrativeGenerator
{
    public class Selector
    {
        public void CreateMissions(QuestGeneratorManager m)
        {
            m.Quests.graph = DrawMissions(m.PlaceholderNpcs, m.PlaceholderItems, m.PossibleWeapons);
        }

        private List<QuestSO> DrawMissions(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            bool containsKill = false, containsTalk = false, containsGet = false, containsExplore = false;
            var questsSos = new List<QuestSO>();
            foreach (KeyValuePair<string, Func<int, int>> item in ProfileCalculator.StartSymbolWeights)
            {
                Debug.Log($" {item.Key} - {item.Value(0)} ");
            }
            int i = 0;
            int j = 0;
            do
            {
                j = 0;
                MarkovChain questChain = new MarkovChain();
                questChain.GetLastSymbol().SetDictionary( ProfileCalculator.StartSymbolWeights );
                while ( questChain.GetLastSymbol().canDrawNext && j < 20)
                {
                    j++;
                    questChain.GetLastSymbol().SetNextSymbol( questChain );
                    SaveCurrentQuest( questChain, questsSos, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                    UpdateListContents( questChain.GetLastSymbol(), ref containsKill ,ref containsTalk ,ref containsGet ,ref containsExplore );
                }
                i++;
            } while ( (!containsKill || !containsTalk || !containsGet || !containsExplore) && i < 50 );
            Debug.Log($"i:{i} j:{j}");
            Debug.Log("FINAL QUEST SO:");
            foreach (QuestSO quest in questsSos)
            {
                Debug.Log(quest.symbolType);
            }
            return questsSos;
        }

        private void UpdateListContents ( Symbol lastQuest, ref bool containsKill ,ref bool containsTalk ,ref bool containsGet ,ref bool containsExplore )
        {
            switch ( lastQuest.symbolType )
            {
                case Constants.TALK_QUEST:
                    containsTalk = true;
                    break;
                case Constants.GET_QUEST:
                    containsGet = true;
                    break;
                case Constants.KILL_QUEST:
                    containsKill = true;
                    break;
                case Constants.EXPLORE_QUEST:
                    containsExplore = true;
                    break;
            }
        }

        private void SaveCurrentQuest ( MarkovChain questChain, List<QuestSO> questSos, List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes )
        {
            switch ( questChain.GetLastSymbol().symbolType )
            {
                case Constants.TALK_QUEST:
                case Constants.TALK_TERMINAL:
                    var t = new Talk();
                    t.DefineQuestSO( questSos, possibleNpcs );
                    break;
                case Constants.GET_QUEST:
                case Constants.GET_TERMINAL:
                case Constants.ITEM_TERMINAL:
                case Constants.DROP_TERMINAL:
                    var g = new Get();
                    g.DefineQuestSO( questChain, questSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
                    break;
                case Constants.KILL_QUEST:
                case Constants.KILL_TERMINAL:
                    var k = new Kill();
                    k.DefineQuestSO( questSos, possibleEnemyTypes );
                    break;
                case Constants.EXPLORE_QUEST:
                case Constants.EXPLORE_TERMINAL:
                case Constants.SECRET_TERMINAL:
                    var e = new Explore();
                    e.DefineQuestSO( questSos );
                    break;
            }
        }

  
    }
}