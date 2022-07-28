using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
using Game.NPCs;
using MyBox;
using ScriptableObjects;
using UnityEngine;
using Util;
using System;

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
            } while ( wasQuestAdded.ContainsValue(false));
            return questLineList;
        }

        private void CreateQuestDict ()
        {
            wasQuestAdded.Add(Constants.IMMERSION_QUEST, false);
            wasQuestAdded.Add(Constants.ACHIEVEMENT_QUEST, false);
            wasQuestAdded.Add(Constants.MASTERY_QUEST, false);
            wasQuestAdded.Add(Constants.CREATIVITY_QUEST, false);
        }

        private void UpdateListContents ( Symbol lastQuest )
        {
            wasQuestAdded[lastQuest.symbolType] = true;
        }

        private void SaveCurrentQuest ( MarkovChain questChain, List<QuestSO> questSos, List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes )
        {
            switch ( questChain.GetLastSymbol().symbolType )
            {
                case Constants.IMMERSION_QUEST:
                case Constants.LISTEN_QUEST:
                    var t = new Talk();
                    t.DefineQuestSO( questSos, possibleNpcs );
                    break;
                case Constants.ACHIEVEMENT_QUEST:
                case Constants.GATHER_QUEST:
                case Constants.ITEM_QUEST:
                    var g = new Get();
                    g.DefineQuestSO( questChain, questSos, possibleNpcs, possibleTreasures, possibleEnemyTypes);
                    break;
                case Constants.MASTERY_QUEST:
                case Constants.KILL_QUEST:
                    var k = new Kill();
                    k.DefineQuestSO( questSos, possibleEnemyTypes );
                    break;
                case Constants.CREATIVITY_QUEST:
                case Constants.EXPLORE_QUEST:
                case Constants.SECRET_QUEST:
                    var e = new Explore();
                    e.DefineQuestSO( questSos );
                    break;
            }
        }
    }
}