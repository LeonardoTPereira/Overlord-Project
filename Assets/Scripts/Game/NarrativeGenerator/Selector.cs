using System.Collections.Generic;
using System.Linq;
using System;
using Game.ExperimentControllers;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using MyBox;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator
{
    public static class Selector
    {
        private static Dictionary<string,bool> _wasQuestAdded;
        private static GeneratorSettings _generatorSettings;

        public static QuestLineList CreateMissions(in GeneratorSettings generatorSettings)
        {
            _generatorSettings = generatorSettings;
            _wasQuestAdded = new Dictionary<string,bool>();
            return DrawMissions();
        }

        private static QuestLineList DrawMissions()
        {
            CreateQuestDict();
            var questLineList = ScriptableObject.CreateInstance<QuestLineList>();
            questLineList.Init();            
            CreateQuestLineForEachNpc(questLineList);

            var i = 0;
            while ( _wasQuestAdded.ContainsValue(false) && i < 100 )
            {
                i++;
                var selectedNpc = _generatorSettings.PlaceholderNpcs.GetRandom();
                ContinueQuestLineForNpc(selectedNpc, questLineList);
            }
            return questLineList;
        }

        private static void CreateQuestLineForEachNpc(QuestLineList questLineList)
        {
            foreach (var npcInCharge in _generatorSettings.PlaceholderNpcs)
            {
               CreateQuestLineForNpc(npcInCharge, questLineList);
            }
        }

        private static void CreateQuestLineForNpc ( NpcSo npcInCharge, QuestLineList questLineList)
        {
            var questLine = CreateQuestLine();
            questLine.PopulateQuestLine(_generatorSettings);
            UpdateListContents(questLine);
            questLine.Quests[^1].EndsStoryLine = true;
            questLine.NpcInCharge = npcInCharge;
            questLineList.QuestLines.Add(questLine);
        }
        
        private static void ContinueQuestLineForNpc ( NpcSo npcInCharge, QuestLineList questLineList)
        {
            var questLine = questLineList.QuestLines.Single(questLine => questLine.NpcInCharge.NpcName == npcInCharge.NpcName);
            if (questLine != null)
            {
                questLine.Quests[^1].EndsStoryLine = false;
                questLine.CompleteMissingQuests(_generatorSettings, _wasQuestAdded );
                UpdateListContents(questLine);
                questLine.Quests[^1].EndsStoryLine = true;
            }
            else
            {
                Debug.LogError($"No QuestLine Found With {npcInCharge.NpcName} In Charge");
                CreateQuestLineForNpc(npcInCharge, questLineList);
            }
        }

        private static QuestLine CreateQuestLine()
        {
            var questLine = ScriptableObject.CreateInstance<QuestLine>();
            questLine.Init();
            return questLine;
        }

        private static void CreateQuestDict ()
        {
            _wasQuestAdded.Add(nameof(KillQuestSo), false);
        }

        private static void UpdateListContents (QuestLine questLine)
        {
            foreach (var quest in questLine.Quests.Where(quest => quest != null))
            {
                _wasQuestAdded[quest.GetType().Name] = true;
            }
        }
    }
}