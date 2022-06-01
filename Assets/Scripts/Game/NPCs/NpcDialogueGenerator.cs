using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;

namespace Game.NPCs
{
    public static class NpcDialogueGenerator
    {
        public static string CreateGreeting(NpcSo speaker)
        {
            var greeting = new StringBuilder();
            if(speaker.SocialFactor < 3)
            {
                greeting.Append("...");
            }
            else if (speaker.SocialFactor < 5)
            {
                greeting.Append("Hey,");
            }
            else if (speaker.SocialFactor < 8)
            {
                greeting.Append("Hello,");
            }
            else
            {
                greeting.Append(speaker.ViolenceFactor < 8 ? "What a pleasure," : "What do you want with me?");
            }
            greeting.Append(" I'm "+speaker.NpcName+", the "+speaker.Job+".");
            return greeting.ToString();
        }

        public static string CreateQuestOpener(QuestSO openedQuest)
        {
            var questOpener = new StringBuilder();
            questOpener.Append("Greetings adventurer! I was expecting you!");
            if (openedQuest.IsKillQuest())
            {
                questOpener.Append("I need you to kill some monsters for me:\n");
                var killQuest = openedQuest as KillQuestSO;
                foreach (var enemyByAmount in killQuest.EnemiesToKillByType.EnemiesByTypeDictionary)
                {
                    questOpener.Append($"{enemyByAmount.Value.ToString()} {enemyByAmount.Key.name}s, ");
                }
                questOpener.Remove(questOpener.Length - 3, 2);
            }
            return questOpener.ToString();
        }
        
        //TODO create quest finisher
    }
}