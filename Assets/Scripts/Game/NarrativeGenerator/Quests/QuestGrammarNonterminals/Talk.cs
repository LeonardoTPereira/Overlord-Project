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
                talkQuestWeights.Add( Constants.TALK_TERMINAL, Constants.OneOptionQuestLineWeight );
                talkQuestWeights.Add( Constants.EMPTY_TERMINAL, Constants.OneOptionQuestEmptyWeight );
                return talkQuestWeights;
            } 
            set {}
        }
        public override string symbolType {
            get { return Constants.TALK_QUEST; }
        }

        public void DefineQuestSO ( List<QuestSO> questSos, List<NpcSo> possibleNpcs )
        {
            CreateAndSaveTalkQuestSo(questSos, possibleNpcs);
        }

        private static void CreateAndSaveTalkQuestSo(List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
        {
            var talkQuest = ScriptableObject.CreateInstance<TalkQuestSO>();
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