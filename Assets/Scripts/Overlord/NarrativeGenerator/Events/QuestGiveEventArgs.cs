using Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals;

namespace Overlord.NarrativeGenerator.Events
{
    public class QuestGiveEventArgs : QuestElementEventArgs
    {
        public GiveQuestSo GiveQuestData {get; set; }

        public QuestGiveEventArgs(GiveQuestSo questSo):base(questSo.Id)
        {
            GiveQuestData = questSo;
        }
    }
}