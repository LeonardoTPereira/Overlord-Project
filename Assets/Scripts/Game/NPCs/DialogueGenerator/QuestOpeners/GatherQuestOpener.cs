using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class GatherQuestOpener
    {
        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            int text = UnityEngine.Random.Range(0, 3);
            switch (text)
            {
                case 0:
                    return "I need you to collect:\n";
                case 1:
                    return "Why are there so many things laying on the floor!? Please, can't you do something about this? Collect ";
                default:
                    return "There's a lot of treasures around this dungeon. Don't be shy, feel free to collect it all! In fact, I think you reall should collect ";
            }
        }
    }
}