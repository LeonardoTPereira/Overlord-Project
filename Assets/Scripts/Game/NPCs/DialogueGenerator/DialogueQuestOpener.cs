using Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Overlord.NarrativeGenerator.Quests;
using UnityEngine;
using Overlord.NarrativeGenerator.NPCs;

namespace Game.NPCs
{
    public static class DialogueQuestOpener
    {
        public static string CreateQuestOpener(QuestSo openedQuest, NpcSo speaker)
        {
            QuestDialogue questOpener;
            switch (openedQuest)
            {
                case ExchangeQuestSo:
                    questOpener = new ExchangeQuestOpener();
                    break;
                case GatherQuestSo:
                    questOpener = new GatherQuestOpener();
                    break;
                case KillQuestSo:
                    questOpener = new KillQuestOpener();
                    break;
                case DamageQuestSo:
                    questOpener = new DamageQuestOpener();
                    break;
                case GiveQuestSo:
                    questOpener = new GiveQuestOpener();
                    break;
                case ListenQuestSo:
                    questOpener = new ListenQuestOpener();
                    break;
                case ReadQuestSo:
                    questOpener = new ReadQuestOpener();
                    break;
                case ReportQuestSo:
                    questOpener = new ReportQuestOpener();
                    break;
                case ExploreQuestSo:
                    questOpener = new ExploreQuestOpener();
                    break;
                case GotoQuestSo:
                    questOpener = new GoToQuestOpener();
                    break;
                default:
                    Debug.LogError($"No quest type for this quest {openedQuest.GetType()} " +
                                   "was found to create dialogue");
                    return null;
            }
            return questOpener.CreateQuestDialogue( openedQuest, speaker );
        }
    }
}