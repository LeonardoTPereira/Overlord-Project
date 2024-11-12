using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class ReadQuestOpener
    {
        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            return "I need you to read the message in:\n";
        }

    }
}