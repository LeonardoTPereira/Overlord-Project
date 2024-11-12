using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class GiveQuestOpener
    {
        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            int text = UnityEngine.Random.Range(0, 3);
            switch (text)
            {
                case 0:
                    return "I need you to give:\n";
                case 1:
                    return "Someone's birthday is comming up! Can you give ";
                default:
                    return "I was thinking on giving someone a little present... Can you help me with that? Please, give ";
            }
        }
    }
}