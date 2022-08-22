using System.Collections.Generic;
using System.Linq;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;

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
                var selectedNpc = possibleNpcs[Random.Range(0, possibleNpcs.Count)];
                ContinueQuestLineForNpc(selectedNpc, possibleNpcs, possibleTreasures, possibleEnemyTypes,
                    questLineList);
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
            var questLine = CreateQuestLine();
            questLine.PopulateQuestLine(possibleNpcs, possibleTreasures, possibleEnemyTypes);
            UpdateListContents(questLine);
            questLine.Quests[^1].EndsStoryLine = true;
            Debug.Log(questLine.Quests.Count);
            questLine.NpcInCharge = npcInCharge;
            questLineList.QuestLines.Add(questLine);
        }
        
        private void ContinueQuestLineForNpc ( NpcSo npcInCharge, List<NpcSo> possibleNpcs, TreasureRuntimeSetSO possibleTreasures,
            WeaponTypeRuntimeSetSO possibleEnemyTypes, QuestLineList questLineList)
        {
            var questLine = questLineList.QuestLines.Single(questLine => questLine.NpcInCharge.NpcName == npcInCharge.NpcName);
            if (questLine != null)
            {
                questLine.Quests[^1].EndsStoryLine = false;
                questLine.PopulateQuestLine(possibleNpcs, possibleTreasures, possibleEnemyTypes);
                UpdateListContents(questLine);
                questLine.Quests[^1].EndsStoryLine = true;
                Debug.Log(questLine.Quests.Count);
            }
            else
            {
                Debug.LogError($"No QuestLine Found With {npcInCharge.NpcName} In Charge");
                CreateQuestLineForNpc(npcInCharge, possibleNpcs, possibleTreasures, possibleEnemyTypes, questLineList);
            }
        }

        private QuestLine CreateQuestLine()
        {
            var questLine = ScriptableObject.CreateInstance<QuestLine>();
            questLine.Init();
            return questLine;
        }

        private void CreateQuestDict ()
        {
            wasQuestAdded.Add(nameof(KillQuestSo), false);
        }

        private void UpdateListContents (QuestLine questLine)
        {
            foreach (var quest in questLine.Quests.Where(quest => quest != null))
            {
                wasQuestAdded[quest.GetType().Name] = true;
            }
        }
    }
}