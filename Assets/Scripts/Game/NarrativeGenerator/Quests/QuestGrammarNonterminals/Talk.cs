using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using ScriptableObjects;
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
                Dictionary<string, Func<int, int>> aux = new Dictionary<string, Func<int, int>>();
                aux.Add( Constants.TALK_TERMINAL, x => (int)Mathf.Clamp( 1/(x*0.25f), 0, 100) );
                aux.Add( Constants.EMPTY_TERMINAL, x => (int)Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 100));
                return aux;
            } 
            set {}
        }
        public void DefineQuestSO ( List<QuestSO> questSos, List<NpcSO> possibleNpcs )
        {
            CreateAndSaveTalkQuestSo(questSos, possibleNpcs);
        }

        public static void CreateAndSaveTalkQuestSo(List<QuestSO> questSos, List<NpcSO> possibleNpcSos)
        {
            var talkQuest = ScriptableObject.CreateInstance<TalkQuestSO>();
            var selectedNpc = possibleNpcSos.GetRandom();
            talkQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, selectedNpc);
            talkQuest.SaveAsAsset();
            questSos.Add(talkQuest);
        }
    }
}