using System.Text;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using MyBox;
using UnityEngine;

namespace Game.NPCs
{
    public static class DialogueQuestCloser
    {
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
    }
}