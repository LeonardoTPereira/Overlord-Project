using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;

namespace Game.NarrativeGenerator.Quests.QuestGrammarNonterminals
{
    public class Explore : NonTerminalQuest
    {
        public Explore(int lim, Dictionary<string, int> questWeightsByType) : base(lim, questWeightsByType)
        {
            maxQuestChance = 2.6f;
        }
        
        public void Option(List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
        {
            DrawQuestType();
            DefineNextQuest(questSos, possibleNpcSos);
        }

        protected void DefineNextQuest(List<QuestSO> questSos, List<NpcSo> possibleNpcSos)
        {

            if (r > 2.6)
            {
                CreateAndSaveSecretRoomQuestSo(questSos);
                var t = new Talk(lim, QuestWeightsByType);
                t.Option(questSos, possibleNpcSos);
            }
            else
            {
                CreateAndSaveSecretRoomQuestSo(questSos);
            }
        }

        private static void CreateAndSaveSecretRoomQuestSo(List<QuestSO> questSos)
        {
            var secretRoomQuest = ScriptableObject.CreateInstance<SecretRoomQuestSO>();
            secretRoomQuest.Init("Explore Room", false, questSos.Count > 0 ? questSos[questSos.Count-1] : null);
            //TODO initiate data for secretRoomQuest
            questSos.Add(secretRoomQuest);
        }
    }
}