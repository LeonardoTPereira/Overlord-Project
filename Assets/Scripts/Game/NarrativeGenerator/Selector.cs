using System.Collections.Generic;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator
{
    public class Selector
    {
        Dictionary<string,bool> wasQuestAdded = new Dictionary<string,bool>();

        public QuestLineList CreateMissions(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, 
            WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            return DrawMissions(possibleNpcs, possibleTreasures, possibleEnemyTypes);
        }

        private QuestLineList DrawMissions(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures, WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            CreateQuestDict();
            var questLineList = ScriptableObject.CreateInstance<QuestLineList>();
            questLineList.Init();            
            do
            {
                CreateQuestLineForEachNpc(possibleNpcs, possibleTreasures, possibleEnemyTypes, questLineList);
            //TODO: Verify with Leo if it would be interesting 
            //to have a minumum number of questlines
            } while ( wasQuestAdded.ContainsValue(false) );
            return questLineList;
        }

        private void CreateQuestLineForEachNpc(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures,
            WeaponTypeRuntimeSetSO possibleEnemyTypes, QuestLineList questLineList)
        {
            foreach (var npcInCharge in possibleNpcs)
            {
                var questLine = CreateQuestLine(possibleNpcs, possibleTreasures, possibleEnemyTypes);
                questLine.Quests[^1].EndsStoryLine = true;
                questLine.NpcInCharge = npcInCharge;
                questLineList.QuestLines.Add(questLine);
            }
        }

        private QuestLine CreateQuestLine(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures,
            WeaponTypeRuntimeSetSO possibleEnemyTypes)
        {
            var questLine = ScriptableObject.CreateInstance<QuestLine>();
            questLine.Init();
            var questChain = new MarkovChain();
            while (questChain.GetLastSymbol().CanDrawNext)
            {
                var lastSelectedQuest = questChain.GetLastSymbol();
                lastSelectedQuest.SetDictionary(ProfileCalculator.StartSymbolWeights);
                lastSelectedQuest.SetNextSymbol(questChain);

                var nonTerminalSymbol = questChain.GetLastSymbol();
                UpdateListContents(nonTerminalSymbol);
                nonTerminalSymbol.SetNextSymbol(questChain);

                questChain.GetLastSymbol().DefineQuestSo(questLine.Quests, possibleNpcs, possibleTreasures, possibleEnemyTypes);
            }

            return questLine;
        }

        private void CreateQuestDict ()
        {
            wasQuestAdded.Add(Constants.MasteryQuest, false);
        }

        private void UpdateListContents ( ISymbol lastQuest )
        {
            wasQuestAdded[lastQuest.SymbolType] = true;
        }
    }
}