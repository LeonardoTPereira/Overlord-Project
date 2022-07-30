using ScriptableObjects;
using Util;
using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using System.Collections.Generic;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class ImmersionQuestSO : QuestSO
    {
        public override string symbolType {
            get { return Constants.IMMERSION_QUEST; }
        }

        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> immersionQuestWeights = new Dictionary<string, Func<int, int>>();
                immersionQuestWeights.Add( Constants.LISTEN_QUEST, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.READ_QUEST, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.GIVE_QUEST, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.REPORT_QUEST, Constants.FourOptionQuestLineWeight );
                immersionQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
                return immersionQuestWeights;
            } 
        }

        public override void DefineQuestSO ( MarkovChain chain, List<QuestSO> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( chain.GetLastSymbol().symbolType )
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

        private static void CreateAndSaveListenQuestSo (List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
        {
            var immersionQuest = ScriptableObject.CreateInstance<ListenQuestSO>();
            var selectedNpc = possibleNpcSos.GetRandom();
            immersionQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            //talkQuest.SaveAsAsset();
            if (questSos.Count > 0)
            {
                questSos[^1].Next = immersionQuest;
            }
            
            questSos.Add(immersionQuest);
        }

        private static void CreateAndSaveReadQuestSo (List<QuestSO> questSos, TreasureRuntimeSetSO possibleItems)
        {
            var immersionQuest = ScriptableObject.CreateInstance<ReadQuestSO>();
            // var selectedNpc = possibleNpcSos.GetRandom();
            // immersionQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            //talkQuest.SaveAsAsset();
            if (questSos.Count > 0)
            {
                questSos[^1].Next = immersionQuest;
            }
            
            questSos.Add(immersionQuest);
        }

        private static void CreateAndSaveGiveQuestSo (List<QuestSO> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleTreasures)
        {
            var immersionQuest = ScriptableObject.CreateInstance<GiveQuestSO>();
            var selectedNpc = possibleNpcSos.GetRandom();
            immersionQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            //talkQuest.SaveAsAsset();
            if (questSos.Count > 0)
            {
                questSos[^1].Next = immersionQuest;
            }
            
            questSos.Add(immersionQuest);
        }

        private static void CreateAndSaveReportQuestSo(List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
        {
            var immersionQuest = ScriptableObject.CreateInstance<ReportQuestSO>();
            var selectedNpc = possibleNpcSos.GetRandom();
            immersionQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            //talkQuest.SaveAsAsset();
            if (questSos.Count > 0)
            {
                questSos[^1].Next = immersionQuest;
            }
            
            questSos.Add(immersionQuest);
        }
    }
}