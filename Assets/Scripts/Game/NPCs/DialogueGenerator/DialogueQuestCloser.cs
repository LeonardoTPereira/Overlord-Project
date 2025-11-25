using System.Text;
using Overlord.NarrativeGenerator.Quests;
using Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals;
using UnityEngine;

namespace Game.NPCs
{
    public static class DialogueQuestCloser
    {
        public static string CreateQuestCloser(QuestSo closedQuest, NpcSo speaker)
        {
            var questCloserString = new StringBuilder();
            QuestDialogue questCloser;

            switch (closedQuest)
            {
                case ExchangeQuestSo:
                    questCloser = new ExchangeQuestCloser();
                    break;
                case GatherQuestSo:
                    questCloser = new GatherQuestCloser();
                    break;
                case KillQuestSo:
                    questCloser = new KillQuestCloser();
                    break;
                case DamageQuestSo:
                    questCloser = new DamageQuestCloser();
                    break;
                case GiveQuestSo:
                    questCloser = new GiveQuestCloser();
                    break;
                case ListenQuestSo:
                    questCloser = new ListenQuestCloser();
                    break;
                case ReadQuestSo:
                    questCloser = new ReadQuestCloser();
                    break;
                case ReportQuestSo:
                    questCloser = new ReportQuestCloser();
                    break;
                case ExploreQuestSo:
                    questCloser = new ExploreQuestCloser();
                    break;
                case GotoQuestSo:
                    questCloser = new GoToQuestCloser();
                    break;
                default:
                    Debug.LogError($"No quest type for this quest {closedQuest.GetType()} " +
                                   "was found to create dialogue");
                    return null;
            }

            questCloserString.Append( questCloser.CreateQuestDialogue(closedQuest, speaker) );
            questCloserString.Append($"<complete={closedQuest.Id}>");

            return questCloserString.ToString();
        }
    }
}