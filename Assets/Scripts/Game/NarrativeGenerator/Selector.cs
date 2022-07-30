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
            int i = 0;
            do
            {
                int j =0;
                var questLine = new QuestList();
                MarkovChain questChain = new MarkovChain();
                while ( questChain.GetLastSymbol().canDrawNext && j < 10)
                {
                    j ++;
                    Symbol lastSelectedQuest = questChain.GetLastSymbol();
                    lastSelectedQuest.SetDictionary( ProfileCalculator.StartSymbolWeights );
                    lastSelectedQuest.SetNextSymbol( questChain );
                    lastSelectedQuest.DefineQuestSO( questChain, questLine.Quests, possibleNpcs, possibleTreasures, possibleEnemyTypes );
                    UpdateListContents( lastSelectedQuest );
                }
                i++;
                questLine.Quests[^1].EndsStoryLine = true;
                questLine.NpcInCharge = possibleNpcs.GetRandom();
                questLineList.Add(questLine);
                foreach (QuestSO item in questLine.Quests)
                {
                    Debug.Log(item.symbolType);
                }
            } while ( wasQuestAdded.ContainsValue(false) && i < 10);
            Debug.Log(i);
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