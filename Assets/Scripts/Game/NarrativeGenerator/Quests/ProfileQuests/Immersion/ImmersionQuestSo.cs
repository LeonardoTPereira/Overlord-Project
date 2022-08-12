using ScriptableObjects;
using Util;
using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ImmersionQuestSo : QuestSo
    {
        public override string SymbolType => Constants.ImmersionQuest;

        public override Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get {
                if ( nextSymbolChances != null )
                    return nextSymbolChances;
                    
                var immersionQuestWeights = new Dictionary<string, Func<int, int>>
                {
                    {Constants.LISTEN_QUEST, Constants.FourOptionQuestLineWeight},
                    {Constants.READ_QUEST, Constants.FourOptionQuestLineWeight},
                    {Constants.GIVE_QUEST, Constants.FourOptionQuestLineWeight},
                    {Constants.REPORT_QUEST, Constants.FourOptionQuestLineWeight},
                    {Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight}
                };
                return immersionQuestWeights;
            } 
        }

        public override void DefineQuestSo ( List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( this.SymbolType )
            {
                case Constants.LISTEN_QUEST:
                    CreateAndSaveListenQuestSo(questSos, possibleNpcSos);
                break;
                case Constants.READ_QUEST:
                    CreateAndSaveReadQuestSo(questSos, possibleItems);
                break;
                case Constants.GIVE_QUEST:
                    CreateAndSaveGiveQuestSo(questSos, possibleNpcSos, possibleItems);
                break;
                case Constants.REPORT_QUEST:
                    CreateAndSaveReportQuestSo(questSos, possibleNpcSos);
                break;
                default:
                    Debug.LogError("help something went wrong!");
                break;
            }
        }

        public override bool HasAvailableElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveElementWithId<T>(T questElement, int questId)
        {
            throw new NotImplementedException();
        }

        private static void CreateAndSaveListenQuestSo (List<QuestSo> questSos, List<NpcSo> possibleNpcSos)
        {
            var listenQuest = CreateInstance<ListenQuestSo>();
            var selectedNpc = possibleNpcSos.GetRandom();
            listenQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = listenQuest;
            }
            
            questSos.Add(listenQuest);
        }

        private static void CreateAndSaveReadQuestSo (List<QuestSo> questSos, TreasureRuntimeSetSO possibleItems)
        {
            var readQuest = CreateInstance<ReadQuestSo>();
            var selectedItem = possibleItems.GetRandomItem();
            readQuest.Init("Read "+selectedItem.ItemName, false, questSos.Count > 0 ? questSos[^1] : null, selectedItem);
            
            if (questSos.Count > 0)
            {
                questSos[^1].Next = readQuest;
            }
            
            questSos.Add(readQuest);
        }

        private static void CreateAndSaveGiveQuestSo (List<QuestSo> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems)
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
        }

        private static void CreateAndSaveReportQuestSo(List<QuestSo> questSos, List<NpcSo> possibleNpcSos)
        {
            var reportQuest = CreateInstance<ReportQuestSo>();
            var selectedNpc = possibleNpcSos.GetRandom();
            reportQuest.Init("Report back to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);

            if (questSos.Count > 0)
            {
                questSos[^1].Next = reportQuest;
            }
            
            questSos.Add(reportQuest);
        }
    }
}