using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class ListenQuestOpener
    {
        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            return "I need you to listen carefully to the message from:\n";
        }

    }
}