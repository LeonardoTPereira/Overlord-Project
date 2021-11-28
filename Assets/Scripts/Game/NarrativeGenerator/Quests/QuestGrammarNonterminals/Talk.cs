using System.Collections.Generic;
using MyBox;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.nao_terminais
{
    public class Talk : NonTerminalQuest
    {

        public Talk(int lim, Dictionary<string, int> questWeightsByType) : base(lim, questWeightsByType)
        {
            maxQuestChance = 2.4f;
        }
    
        public void Option(List<QuestSO> questSos, List<NpcSO> possibleNpcSos)
        {
            DrawQuestType();
            DefineNextQuest(questSos, possibleNpcSos);
        }
    
        private void DefineNextQuest(List<QuestSO> questSos, List<NpcSO> possibleNpcSos)
        {
            if (r > 2.7)
            {
                CreateAndSaveTalkQuestSo(questSos, possibleNpcSos);
                Option(questSos, possibleNpcSos);
            }
            else
            {
                CreateAndSaveTalkQuestSo(questSos, possibleNpcSos);
            }
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