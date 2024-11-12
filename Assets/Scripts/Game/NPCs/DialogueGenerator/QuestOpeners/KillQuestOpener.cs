using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class KillQuestOpener
    {
        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            int text = UnityEngine.Random.Range(0, 3);
            switch (text)
            {
                case 0:
                    return "I need you to kill some monsters for me:\n";
                case 1:
                    return "Nasty monsters! They are EVERYWHERE!! Please don't tell anyone, but I'm a bit scared of them... Could you please kill ";
                default:
                    return "The monsters in this dungeon sure are annoying! Do us a favor and kill ";
            }
        }
    }
}