using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using MyBox;
using UnityEngine;
using System;
using Util;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarNonterminals
{
    public class Talk : NonTerminalQuest
    {
        public override Dictionary<string, Func<int,int>> nextSymbolChances
        {
            get {
                Dictionary<string, Func<int, int>> talkQuestWeights = new Dictionary<string, Func<int, int>>();
                talkQuestWeights.Add( Constants.LISTEN_QUEST, Constants.OneOptionQuestLineWeight );
                talkQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
                return talkQuestWeights;
            } 
            set {}
        }
        public override string symbolType {
            get { return Constants.IMMERSION_QUEST; }
        }

        public void DefineQuestSO ( List<QuestSO> questSos, List<NpcSo> possibleNpcs )
        {
            CreateAndSaveListenQuestSO(questSos, possibleNpcs);
        }

        private static void CreateAndSaveListenQuestSO(List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
        {
            var talkQuest = ScriptableObject.CreateInstance<ListenQuestSO>();
            var selectedNpc = possibleNpcSos.GetRandom();
            talkQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[^1] : null, selectedNpc);
            //talkQuest.SaveAsAsset();
            if (questSos.Count > 0)
            {
                questSos[^1].Next = talkQuest;
            }
            
            questSos.Add(talkQuest);
        }
    }
}