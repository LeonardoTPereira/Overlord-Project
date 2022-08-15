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
            CreateQuestLineForEachNpc(possibleNpcs, possibleTreasures, possibleEnemyTypes, questLineList);

            while ( wasQuestAdded.ContainsValue(false) )
            {
                NpcSo selectedNpc = possibleNpcs[Random.Range(0, possibleNpcs.Count)];
                CreateQuestLineForNpc(selectedNpc, possibleNpcs, possibleTreasures, possibleEnemyTypes, questLineList);
            }
            return questLineList;
        }

        private void CreateQuestLineForEachNpc(List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures,
            WeaponTypeRuntimeSetSO possibleEnemyTypes, QuestLineList questLineList)
        {
            foreach (var npcInCharge in possibleNpcs)
            {
               CreateQuestLineForNpc(npcInCharge, possibleNpcs, possibleTreasures, possibleEnemyTypes, questLineList);
            }
        }

        private void CreateQuestLineForNpc ( NpcSo npcInCharge, List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures,
            WeaponTypeRuntimeSetSO possibleEnemyTypes, QuestLineList questLineList)
        {
            var questLine = CreateQuestLine(possibleNpcs, possibleTreasures, possibleEnemyTypes);
            questLine.Quests[^1].EndsStoryLine = true;
            Debug.Log(questLine.Quests.Count);
            questLine.NpcInCharge = npcInCharge;
            questLineList.QuestLines.Add(questLine);
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