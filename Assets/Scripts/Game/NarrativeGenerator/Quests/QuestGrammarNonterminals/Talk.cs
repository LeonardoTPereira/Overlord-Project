using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using MyBox;
using UnityEngine;
using System;
using Util;

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

        public void DefineQuestSO ( List<QuestSO> questSos, List<NpcSO> possibleNpcs )
        {
            CreateAndSaveTalkQuestSo(questSos, possibleNpcs);
        }

        private static void CreateAndSaveTalkQuestSo(List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
        {
            var talkQuest = ScriptableObject.CreateInstance<TalkQuestSO>();
            var selectedNpc = possibleNpcSos.GetRandom();
            talkQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, selectedNpc);
            //talkQuest.SaveAsAsset();
            questSos.Add(talkQuest);
        }
    }
}