using ScriptableObjects;
using Util;
using MyBox;
using System;
using System.Collections.Generic;
using Game.ExperimentControllers;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ImmersionQuestSo : QuestSo
    {
        public override string SymbolType => Constants.ImmersionQuest;

        public override Dictionary<string, Func<int,float>> NextSymbolChances
        {
            get
            {
                var immersionQuestWeights = new Dictionary<string, Func<int, float>>
                    {
                        {Constants.ListenQuest, Constants.FourOptionQuestLineWeight},
                        {Constants.ReadQuest, Constants.FourOptionQuestLineWeight},
                        {Constants.GiveQuest, Constants.FourOptionQuestLineWeight},
                        {Constants.ReportQuest, Constants.FourOptionQuestLineWeight},
                        {Constants.EmptyQuest, Constants.OneOptionQuestEmptyWeight}
                    };
                return immersionQuestWeights;
            }
        }

        public override QuestSo DefineQuestSo ( List<QuestSo> questSos, in GeneratorSettings generatorSettings)
        {
            switch ( SymbolType )
            {
                case Constants.ListenQuest:
                    return CreateAndSaveListenQuestSo(questSos, generatorSettings.PlaceholderNpcs);
                case Constants.ReadQuest:
                    return CreateAndSaveReadQuestSo(questSos, generatorSettings.ReadableItems);
                case Constants.GiveQuest:
                    return CreateAndSaveGiveQuestSo(questSos, generatorSettings.PlaceholderNpcs, generatorSettings.Tools);
                case Constants.ReportQuest:
                    return CreateAndSaveReportQuestSo(questSos, generatorSettings.PlaceholderNpcs);
                default:
                    Debug.LogError("help something went wrong! - Immersion doesn't contain symbol: "+SymbolType);
                break;
            }

            return null;
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void CreateQuestString()
        {
            throw new NotImplementedException();
        }

        private static ListenQuestSo CreateAndSaveListenQuestSo (List<QuestSo> questSos, List<NpcSo> possibleNpcSos)
        {
            var listenQuest = CreateInstance<ListenQuestSo>();
            var selectedNpc = possibleNpcSos.GetRandom();
            listenQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = listenQuest;
            }
            
            questSos.Add(listenQuest);
            return listenQuest;
        }

        private static ReadQuestSo CreateAndSaveReadQuestSo (List<QuestSo> questSos, TreasureRuntimeSetSo possibleItems)
        {
            var readQuest = CreateInstance<ReadQuestSo>();
            var selectedItem = possibleItems.GetRandomItem();
            readQuest.Init("Read "+selectedItem.ItemName, false, questSos.Count > 0 ? questSos[^1] : null, selectedItem);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = readQuest;
            }
            
            questSos.Add(readQuest);
            return readQuest;
        }

        private static GiveQuestSo CreateAndSaveGiveQuestSo (List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSo possibleItems)
        {
            var giveQuest = CreateInstance<GiveQuestSo>();
            var selectedNpc = possibleNpcSos.GetRandom();
            var selectedItem = possibleItems.GetRandomItem();
            giveQuest.Init($"Give {selectedItem} to {selectedNpc.NpcName}", false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc, selectedItem);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = giveQuest;
            }
            
            questSos.Add(giveQuest);
            return giveQuest;
        }

        private static ReportQuestSo CreateAndSaveReportQuestSo(List<QuestSo> questSos, List<NpcSo> possibleNpcSos)
        {
            var reportQuest = CreateInstance<ReportQuestSo>();
            var selectedNpc = possibleNpcSos.GetRandom();
            reportQuest.Init("Report back to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);

            if (questSos.Count > 0)
            {
                questSos[^1].Next = reportQuest;
            }
            
            questSos.Add(reportQuest);
            return reportQuest;
        }
    }
}