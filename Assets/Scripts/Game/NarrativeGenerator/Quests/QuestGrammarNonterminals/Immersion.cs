// using System.Collections.Generic;
// using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
// using Game.NPCs;
// using MyBox;
// using UnityEngine;
// using System;
// using Util;
// using ScriptableObjects;

// namespace Game.NarrativeGenerator.Quests.QuestGrammarNonterminals
// {
//     public class Immersion : NonTerminalQuest
//     {
//         public override Dictionary<string, Func<int,int>> nextSymbolChances
//         {
//             get {
//                 Dictionary<string, Func<int, int>> immersionQuestWeights = new Dictionary<string, Func<int, int>>();
//                 immersionQuestWeights.Add( Constants.LISTEN_QUEST, Constants.OneOptionQuestLineWeight );
//                 immersionQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
//                 return immersionQuestWeights;
//             } 
//         }
//         public override string symbolType {
//             get { return Constants.IMMERSION_QUEST; }
//         }

//         public void DefineQuestSO ( List<QuestSO> questSos, List<NpcSo> possibleNpcs )
//         {
//             CreateAndSaveListenQuestSO(questSos, possibleNpcs);
//         }

//         private static void CreateAndSaveListenQuestSO (List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
//         {
//             var immersionQuest = ScriptableObject.CreateInstance<ListenQuestSO>();
//             var selectedNpc = possibleNpcSos.GetRandom();
//             immersionQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
//             //talkQuest.SaveAsAsset();
//             if (questSos.Count > 0)
//             {
//                 questSos[^1].Next = immersionQuest;
//             }
            
//             questSos.Add(immersionQuest);
//         }

//         private static void CreateAndSaveReadQuestSO (List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
//         {
//             var immersionQuest = ScriptableObject.CreateInstance<ListenQuestSO>();
//             var selectedNpc = possibleNpcSos.GetRandom();
//             immersionQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
//             //talkQuest.SaveAsAsset();
//             if (questSos.Count > 0)
//             {
//                 questSos[^1].Next = immersionQuest;
//             }
            
//             questSos.Add(immersionQuest);
//         }

//         private static void CreateAndSaveGiveQuestSO (List<QuestSO> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleTreasures)
//         {
//             var immersionQuest = ScriptableObject.CreateInstance<ListenQuestSO>();
//             var selectedNpc = possibleNpcSos.GetRandom();
//             immersionQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
//             //talkQuest.SaveAsAsset();
//             if (questSos.Count > 0)
//             {
//                 questSos[^1].Next = immersionQuest;
//             }
            
//             questSos.Add(immersionQuest);
//         }

//         private static void CreateAndSaveReportQuestSO (List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
//         {
//             var immersionQuest = ScriptableObject.CreateInstance<ListenQuestSO>();
//             var selectedNpc = possibleNpcSos.GetRandom();
//             immersionQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
//             //talkQuest.SaveAsAsset();
//             if (questSos.Count > 0)
//             {
//                 questSos[^1].Next = immersionQuest;
//             }
            
//             questSos.Add(immersionQuest);
//         }
//     }
// }