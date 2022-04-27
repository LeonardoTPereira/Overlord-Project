using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.QuestGrammarNonterminals
{
    public class Talk : NonTerminalQuest
    {

        public Talk(int lim, Dictionary<string, int> questWeightsByType) : base(lim, questWeightsByType)
        {
            maxQuestChance = 2.4f;
        }
    
        public void Option( MarkovChain chain, List<QuestSO> questSos, List<NpcSO> possibleNpcSos)
        {
            CreateAndSaveTalkQuestSo(questSos, possibleNpcSos);
            SetNextSymbol( chain );
        }

        private static void CreateAndSaveTalkQuestSo(List<QuestSO> questSos, List<NpcSO> possibleNpcSos)
        {
            var talkQuest = ScriptableObject.CreateInstance<TalkQuestSO>();
            var selectedNpc = possibleNpcSos.GetRandom();
            talkQuest.Init("Talk to "+selectedNpc.NpcName, false, questSos.Count > 0 ? questSos[questSos.Count-1] : null, selectedNpc);
            talkQuest.SaveAsAsset();
            questSos.Add(talkQuest);
        }
    }
}