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
            for ( int j = 0; j < 1; j++ )
            {
                bool containsKill = false, containsTalk = false, containsGet = false, containsExplore = false;
                var questSos = new List<QuestSO>();
                int i = 0;
                do
                {
                    MarkovChain questChain = new MarkovChain();
                    questChain.GetLastSymbol().SetDictionary( ProfileCalculator.StartSymbolWeights );
                    while ( questChain.GetLastSymbol().canDrawNext )
                    {
                        questChain.GetLastSymbol().SetNextSymbol( questChain );
                        if (  questChain.GetLastSymbol().symbolType != Constants.KILL_QUEST && questChain.GetLastSymbol().symbolType != Constants.TALK_QUEST && questChain.GetLastSymbol().symbolType != Constants.GET_QUEST && questChain.GetLastSymbol().symbolType != Constants.EXPLORE_QUEST )
                            SaveCurrentQuest( questChain, questSos, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                        UpdateListContents( questChain.GetLastSymbol(), ref containsKill ,ref containsTalk ,ref containsGet ,ref containsExplore );
                    }
                    i += 1;
                } while ( (!containsKill || !containsTalk || !containsGet || !containsExplore) );
                QuestsToJson.AddGeneratedQuests( j, questSos );
            }
            QuestsToJson.CreateJson();
            return null;//questSos;
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
            QuestSO newQuest = ScriptableObject.CreateInstance<QuestSO>();
            newQuest.symbolType = questChain.GetLastSymbol().symbolType;
            questSos.Add(newQuest);
            // switch ( questChain.GetLastSymbol().symbolType )
            // {
            //     case Constants.TALK_QUEST:
            //     case Constants.TALK_TERMINAL:
            //         var t = new Talk();
            //         t.DefineQuestSO( questSos, possibleNpcs );
            //         break;
            //     case Constants.GET_QUEST:
            //     case Constants.GET_TERMINAL:
            //     case Constants.ITEM_TERMINAL:
            //     case Constants.DROP_TERMINAL:
            //         var g = new Get();
            //         g.DefineQuestSO( questChain, questSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
            //         break;
            //     case Constants.KILL_QUEST:
            //     case Constants.KILL_TERMINAL:
            //         var k = new Kill();
            //         k.DefineQuestSO( questSos, possibleEnemyTypes );
            //         break;
            //     case Constants.EXPLORE_QUEST:
            //     case Constants.EXPLORE_TERMINAL:
            //     case Constants.SECRET_TERMINAL:
            //         var e = new Explore();
            //         e.DefineQuestSO( questSos );
            //         break;
            // }
        }

  
    }
}