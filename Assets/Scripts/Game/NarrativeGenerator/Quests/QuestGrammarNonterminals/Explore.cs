using System.Collections.Generic;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
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
        
        public void Option( MarkovChain chain, List<QuestSO> questSos, List<NpcSO> possibleNpcSos )
        {
            CreateAndSaveSecretRoomQuestSo( questSos );
            SetNextSymbol( chain );
        }

        private static void CreateAndSaveSecretRoomQuestSo(List<QuestSO> questSos)
        {
            var secretRoomQuest = ScriptableObject.CreateInstance<SecretRoomQuestSO>();
            secretRoomQuest.Init("Explore Room", false, questSos.Count > 0 ? questSos[questSos.Count-1] : null);
            //TODO initiate data for secretRoomQuest
            secretRoomQuest.SaveAsAsset();
            questSos.Add(secretRoomQuest);
        }
    }
}