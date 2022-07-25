using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
using Game.NPCs;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator
{
    public class Selector
    {
        Dictionary<string,bool> wasQuestAdded = new Dictionary<string,bool>();

        public void CreateMissions(QuestGeneratorManager m)
        {
            m.Quests.questLines = DrawMissions(m.PlaceholderNpcs, m.PlaceholderItems, m.PossibleWeapons);
        }

        private List<QuestList> DrawMissions(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            CreateQuestDict();
            int i = 0;
            var questLineList = new List<QuestList>();
            do
            {
                var questLine = new QuestList();
                MarkovChain questChain = new MarkovChain();
                while ( questChain.GetLastSymbol().canDrawNext )
                {
                    questChain.GetLastSymbol().SetDictionary( ProfileCalculator.StartSymbolWeights );
                    questChain.GetLastSymbol().SetNextSymbol( questChain );
                    SaveCurrentQuest( questChain, questLine.Quests, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                    UpdateListContents( questChain.GetLastSymbol() );
                }
                questLine.Quests[^1].EndsStoryLine = true;
                questLine.NpcInCharge = possibleNpcs.GetRandom();
                questLineList.Add(questLine);
                i++;
            } while ( wasQuestAdded.ContainsValue(false) && i < 30);
            Debug.Log(i);
            return questLineList;
        }

        private void CreateQuestDict ()
        {
            wasQuestAdded.Add(Constants.TALK_QUEST, false);
            wasQuestAdded.Add(Constants.GET_QUEST, false);
            wasQuestAdded.Add(Constants.KILL_QUEST, false);
            wasQuestAdded.Add(Constants.EXPLORE_QUEST, false);
        }

        private void UpdateListContents ( Symbol lastQuest )
        {
            switch ( lastQuest.symbolType )
            {
                case Constants.TALK_QUEST:
                case Constants.GET_QUEST:
                case Constants.KILL_QUEST:
                case Constants.EXPLORE_QUEST:
                    wasQuestAdded[lastQuest.symbolType] = true;
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