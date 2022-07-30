using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
// using Game.NarrativeGenerator.Quests.QuestGrammarNonterminals;
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
                    Symbol lastSelectedQuest = questChain.GetLastSymbol();
                    lastSelectedQuest.SetDictionary( ProfileCalculator.StartSymbolWeights );
                    lastSelectedQuest.SetNextSymbol( questChain );

                    Symbol newSymbol = questChain.GetLastSymbol();
                    UpdateListContents( newSymbol );
                    newSymbol.SetNextSymbol( questChain );

                    questChain.GetLastSymbol().DefineQuestSO( questChain, questLine.Quests, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                }
                questLine.Quests[^1].EndsStoryLine = true;
                questLine.NpcInCharge = possibleNpcs.GetRandom();
                questLineList.Add(questLine);
            //TODO: Verify with Leo if it would be interesting 
            //to have a minumum number of questlines
            } while ( wasQuestAdded.ContainsValue(false) ;
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
    }
}