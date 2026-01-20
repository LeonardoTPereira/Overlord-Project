using ScriptableObjects;
using Util;
using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Util.Enums;
using Overlord.NarrativeGenerator.NPCs;

namespace Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals
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

        public override QuestSo DefineQuestSo (List<QuestSo> questSos, NpcSo npcInCharge, in NarrativeSettings narrativeSettings, Language language)
        {
            switch ( SymbolType )
            {
                case Constants.ListenQuest:
                    return CreateAndSaveListenQuestSo(questSos, npcInCharge, narrativeSettings.PlaceholderNpcs, language);
                case Constants.ReadQuest:
                    return CreateAndSaveReadQuestSo(questSos, npcInCharge, narrativeSettings.ReadableItems, language);
                case Constants.GiveQuest:
                    return CreateAndSaveGiveQuestSo(questSos, npcInCharge, narrativeSettings.PlaceholderNpcs, narrativeSettings.Tools, language);
                case Constants.ReportQuest:
                    return CreateAndSaveReportQuestSo(questSos, npcInCharge, narrativeSettings.PlaceholderNpcs, language);
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

        public override void CreateQuestString(Language l)
        {
            throw new NotImplementedException();
        }

        private static ListenQuestSo CreateAndSaveListenQuestSo (List<QuestSo> questSos, NpcSo npcInCharge, List<NpcSo> possibleNpcSos, Language language)
        {
            var listenQuest = CreateInstance<ListenQuestSo>();
            NpcSo selectedNpc;
            do{
                selectedNpc = possibleNpcSos.GetRandom();

            } while ( selectedNpc == npcInCharge && possibleNpcSos.Count != 1);

            if (language == Language.Portuguese)
                listenQuest.Init("Fale com "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            else if (language == Language.English)
                listenQuest.Init("Talk to " + selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            listenQuest.NpcInCharge = npcInCharge;
            if (questSos.Count > 0)
            {
                questSos[^1].Next = listenQuest;
            }
            
            questSos.Add(listenQuest);
            return listenQuest;
        }

        private static ReadQuestSo CreateAndSaveReadQuestSo (List<QuestSo> questSos, NpcSo npcInCharge, TreasureRuntimeSetSo possibleItems, Language language)
        {
            var readQuest = CreateInstance<ReadQuestSo>();
            var selectedItem = possibleItems.GetRandomItem();

            if (language == Language.Portuguese)
                readQuest.Init("Leia o conte�do do artefato "+selectedItem.ItemName, false, questSos.Count > 0 ? questSos[^1] : null, selectedItem);
            else if (language == Language.English)
                readQuest.Init("Read " + selectedItem.ItemName, false, questSos.Count > 0 ? questSos[^1] : null, selectedItem);

            if (questSos.Count > 0)
            {
                questSos[^1].Next = readQuest;
            }
            readQuest.NpcInCharge = npcInCharge;
            
            questSos.Add(readQuest);
            return readQuest;
        }

        private static GiveQuestSo CreateAndSaveGiveQuestSo (List<QuestSo> questSos, NpcSo npcInCharge, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSo possibleItems, Language language)
        {
            var giveQuest = CreateInstance<GiveQuestSo>();
            var selectedNpc = possibleNpcSos.GetRandom();
            var selectedItem = possibleItems.GetRandomItem();

            if (language == Language.Portuguese)
                giveQuest.Init($"Dê o item {selectedItem} para {selectedNpc.NpcName}", false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc, selectedItem);
            else if (language == Language.English)
                giveQuest.Init($"Give {selectedItem} to {selectedNpc.NpcName}", false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc, selectedItem);

            giveQuest.NpcInCharge = npcInCharge;
            if (questSos.Count > 0)
            {
                questSos[^1].Next = giveQuest;
            }
            
            questSos.Add(giveQuest);
            return giveQuest;
        }

        private static ReportQuestSo CreateAndSaveReportQuestSo(List<QuestSo> questSos, NpcSo npcInCharge, List<NpcSo> possibleNpcSos, Language language)
        {
            var reportQuest = CreateInstance<ReportQuestSo>();
            NpcSo selectedNpc;
            do{
                selectedNpc = possibleNpcSos.GetRandom();

            } while ( selectedNpc == npcInCharge && possibleNpcSos.Count != 1);

            if (language == Language.Portuguese)
                reportQuest.Init("Retorne e reporte para "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            else if (language == Language.English)
                reportQuest.Init("Report back to " + selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            reportQuest.NpcInCharge = npcInCharge;
            if (questSos.Count > 0)
            {
                questSos[^1].Next = reportQuest;
            }
            
            questSos.Add(reportQuest);
            return reportQuest;
        }
    }
}