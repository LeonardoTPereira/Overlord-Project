using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

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
            questOpener.Append(CreateOpener(openedQuest, speaker));
            questOpener.Append(openedQuest.ToString());
            return questOpener.ToString();
        }

        private static string CreateOpener(QuestSo openedQuest, NpcSo speaker)
        {
            switch (openedQuest)
            {
                case ExchangeQuestSo:
                    return "I need you to trade:\n";
                case GatherQuestSo:
                    return "I need you to collect:\n";
                case KillQuestSo:
                    return "I need you to kill some monsters for me:\n";
                case DamageQuestSo:
                    return "I need you to hit this structure:\n";
                case GiveQuestSo:
                    return "I need you to give:\n";
                case ListenQuestSo:
                    return "I need you to listen carefully to the message from:\n";
                case ReadQuestSo:
                    return "I need you to read the message in:\n";
                case ReportQuestSo:
                    return "I need you to report this to:\n";
                case ExploreQuestSo:
                    return "I need you to explore this dungeon:\n";
                case GotoQuestSo:
                    return "I need you to go to a special place for me:\n";
                default:
                    Debug.LogError($"No quest type for this quest {openedQuest.GetType()} " +
                                   "was found to create dialogue");
                    return null;
            }
        }

        public static string CreateQuestCloser(QuestSo closedQuest, NpcSo speaker)
        {
            var questCloser = new StringBuilder();
            questCloser.Append("Oh my! ");
            switch (closedQuest)
            {
                case ExchangeQuestSo:
                    questCloser.Append("You traded them all!\n");
                    break;
                case GatherQuestSo:
                    questCloser.Append("You got them all!\n");
                    break;
                case KillQuestSo:
                    questCloser.Append("You got rid of all of them!\n");
                    break;
                case DamageQuestSo:
                    questCloser.Append("You did pretty good damage to it!\n");
                    break;
                case GiveQuestSo:
                    questCloser.Append("You gave them everything they needed!\n");
                    break;
                case ListenQuestSo:
                    questCloser.Append("Thanks for listening to their message!\n");
                    break;
                case ReadQuestSo:
                    questCloser.Append("You read the message!\n");
                    break;
                case ReportQuestSo:
                    questCloser.Append("You reported the info!\n");
                    break;
                case ExploreQuestSo:
                    questCloser.Append("You explored enough of the dungeon!\n");
                    break;
                case GotoQuestSo:
                    questCloser.Append("You went to the needed room!\n");
                    break;
                default:
                    Debug.LogError($"No quest type for this quest {closedQuest.GetType()} " +
                                   "was found to create dialogue");
                    return null;
            }

            questCloser.Append("Thank you very much!");
            questCloser.Append($"<complete={closedQuest.Id}>");
            return questCloser.ToString();
        }

#if UNITY_EDITOR
        [ButtonMethod]
        public static string CreateMockGoToQuest()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("I need you to go to the room <goto=10,12>");
            return stringBuilder.ToString();
        }
#endif
        public static string CreateExchangeDialogue(ExchangeQuestSo quest, NpcSo npc)
        {
            var questExchangeDialogue = new StringBuilder();
            questExchangeDialogue.Append("You really got all the items.");
            var spriteString = quest.ReceivedItem.GetToolSpriteString();
            questExchangeDialogue.Append($"Take this {quest.ReceivedItem.ItemName} {spriteString} as a reward!");
            questExchangeDialogue.Append($"<trade={npc.NpcName}, {quest.Id}>");
            return questExchangeDialogue.ToString();
        }
    }
}