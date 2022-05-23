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
            var questsSos = new List<QuestSO>();
            MarkovChain questChain = new MarkovChain();
            var chainCost = 0;
            do
            {
                questChain.GetLastSymbol().SetDictionary( ProfileCalculator.StartSymbolWeights );
                while ( questChain.GetLastSymbol().canDrawNext )
                {
                    questChain.GetLastSymbol().SetNextSymbol( questChain );
                    SaveCurrentQuest( questChain, questsSos, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                }
                chainCost += (int)Enums.QuestWeights.Hated*2;
            } while (chainCost < (int)Enums.QuestWeights.Loved );
            Debug.Log("FINAL QUEST SO:");
            foreach (QuestSO quest in questsSos)
            {
                Debug.Log(quest.symbolType);
            }
            return questsSos;
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