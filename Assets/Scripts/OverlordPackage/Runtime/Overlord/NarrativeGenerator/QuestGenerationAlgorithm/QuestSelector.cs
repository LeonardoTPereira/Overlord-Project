using System.Collections.Generic;
using System.Linq;
using Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;
using Util;
using Overlord.NarrativeGenerator.Quests;
using Overlord.NarrativeGenerator.NPCs;

namespace Overlord.NarrativeGenerator
{
    public static class QuestSelector
    {
        private static Dictionary<string, bool> _wasQuestAdded;
        private static NarrativeSettings _narrativeSettings;
        private static Enums.Language _language;

        public static QuestLineList CreateMissions(in NarrativeSettings narrativeSettings, Enums.Language language)
        {
            _narrativeSettings = narrativeSettings;
            _wasQuestAdded = new Dictionary<string, bool>();
            _language = language;
            return DrawMissions();
        }

        private static QuestLineList DrawMissions()
        {
            CreateQuestDict();
            var questLineList = ScriptableObject.CreateInstance<QuestLineList>();
            questLineList.Init();
            CreateQuestLineForEachNpc(questLineList);

            var i = 0;
            while (_wasQuestAdded.ContainsValue(false) && i < 100)
            {
                i++;
                var selectedNpc = _narrativeSettings.PlaceholderNpcs.GetRandom();
                ContinueQuestLineForNpc(selectedNpc, questLineList);
            }
            return questLineList;
        }

        private static void CreateQuestLineForEachNpc(QuestLineList questLineList)
        {
            foreach (var npcInCharge in _narrativeSettings.PlaceholderNpcs)
            {
                CreateQuestLineForNpc(npcInCharge, questLineList);
            }
        }

        private static void CreateQuestLineForNpc(NpcSo npcInCharge, QuestLineList questLineList)
        {
            var questLine = CreateQuestLine();
            questLine.PopulateQuestLine(_narrativeSettings, npcInCharge);
            UpdateListContents(questLine);
            questLine.Quests[^1].EndsStoryLine = true;
            questLine.NpcInCharge = npcInCharge;
            questLineList.QuestLines.Add(questLine);
        }

        private static void ContinueQuestLineForNpc(NpcSo npcInCharge, QuestLineList questLineList)
        {
            var questLine = questLineList.QuestLines.Single(questLine => questLine.NpcInCharge.NpcName == npcInCharge.NpcName);
            if (questLine != null)
            {
                questLine.Quests[^1].EndsStoryLine = false;
                questLine.CompleteMissingQuests(_narrativeSettings, npcInCharge, _wasQuestAdded);
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

        private static void CreateQuestDict()
        {
            _wasQuestAdded.Add(nameof(KillQuestSo), false);
        }

        private static void UpdateListContents(QuestLine questLine)
        {
            foreach (var quest in questLine.Quests.Where(quest => quest != null))
            {
                _wasQuestAdded[quest.GetType().Name] = true;
            }
        }
    }
}