using Overlord.NarrativeGenerator.Quests;
using Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals;
using UnityEngine;

namespace Game.NPCs
{
    public static class DialogueQuestCheckPoint
    {
        public static string CreateQuestCheckPoint(QuestSo quest, NpcSo speaker)
        {
            QuestDialogue questCheckPoint;
            switch (quest)
            {
                case ExchangeQuestSo:
                    questCheckPoint = new ExchangeQuestCheckPoint();
                    break;
                // case GatherQuestSo:
                //     questCheckPoint = new GatherQuestCheckPoint();
                //     break;
                // case KillQuestSo:
                //     questCheckPoint = new KillQuestCheckPoint();
                //     break;
                // case DamageQuestSo:
                //     questCheckPoint = new DamageQuestCheckPoint();
                //     break;
                // case GiveQuestSo:
                //     questCheckPoint = new GiveQuestCheckPoint();
                //     break;
                case ListenQuestSo:
                    questCheckPoint = new ListenQuestCheckPoint();
                    break;
                // case ReadQuestSo:
                //     questCheckPoint = new ReadQuestCheckPoint();
                //     break;
                case ReportQuestSo:
                    questCheckPoint = new ReportQuestCheckPoint();
                    break;
                // case ExploreQuestSo:
                //     questCheckPoint = new ExploreQuestCheckPoint();
                //     break;
                // case GotoQuestSo:
                //     questCheckPoint = new GoToQuestCheckPoint();
                //     break;
                default:
                    Debug.LogError($"No quest type for this quest {quest.GetType()} " +
                                   "was found to create dialogue");
                    return null;
            }
            return questCheckPoint.CreateQuestDialogue( quest, speaker );
        }
    }
}