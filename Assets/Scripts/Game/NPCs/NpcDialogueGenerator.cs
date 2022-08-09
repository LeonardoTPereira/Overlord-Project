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
            switch (speaker.SocialFactor)
            {
                case < 3:
                    greeting.Append("...");
                    break;
                case < 5:
                    greeting.Append("Hey,");
                    break;
                case < 8:
                    greeting.Append("Hello,");
                    break;
                default:
                    greeting.Append(speaker.ViolenceFactor < 8 ? "What a pleasure," : "What do you want with me?");
                    break;
            }
            greeting.Append(" I'm "+speaker.NpcName+", the "+speaker.Job+".");
            return greeting.ToString();
        }

        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            var questOpener = new StringBuilder();
            questOpener.Append("Greetings adventurer! I was expecting you!");
            if (openedQuest.IsKillQuest())
            {
                questOpener.Append("I need you to kill some monsters for me:\n");
                var killQuest = openedQuest as KillQuestSo;
                foreach (var enemyByAmount in killQuest.EnemiesToKillByType.EnemiesByTypeDictionary)
                {
                    questOpener.Append($"{enemyByAmount.Value.ToString()} {enemyByAmount.Key.EnemyTypeName}s, ");
                }
                questOpener.Remove(questOpener.Length - 3, 2);
            }
            else if(openedQuest.IsTalkQuest())
            {
                var talkQuest = openedQuest as ListenQuestSo;
                questOpener.Append(talkQuest.Npc == speaker
                    ? "I needed to speak with you!\n"
                    : $"I need you to speak with {talkQuest.Npc.NpcName}!\n");
            }
            else if(openedQuest.IsItemQuest())
            {
                questOpener.Append("I need you to get some items for me:\n");
                var itemQuest = openedQuest as ItemQuestSo;
                foreach (var itemByAmount in itemQuest.ItemsToCollectByType)
                {
                    questOpener.Append($"{itemByAmount.Value.ToString()} {itemByAmount.Key.ItemName}s, ");
                }
                questOpener.Remove(questOpener.Length - 3, 2);
            }
            return questOpener.ToString();
        }
        
        public static string CreateQuestCloser(QuestSo closedQuest, NpcSo speaker)
        {
            var questOpener = new StringBuilder();
            questOpener.Append("Oh my!");
            if (closedQuest.IsKillQuest())
            {
                questOpener.Append("You got rid of all of them! Thank you very much!\n");
            }
            else if(closedQuest.IsTalkQuest())
            {
                questOpener.Append("Thanks for reaching out to them!\n");
            }
            else if(closedQuest.IsItemQuest())
            {
                questOpener.Append("You got them all! Thank you very much!\n");
            }
            return questOpener.ToString();
        }
    }
}