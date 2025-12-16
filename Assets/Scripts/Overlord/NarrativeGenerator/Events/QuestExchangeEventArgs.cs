using Overlord.NarrativeGenerator.Quests.QuestGrammarTerminals;

namespace Overlord.NarrativeGenerator.Events
{
    public class QuestExchangeEventArgs : QuestElementEventArgs
    {
        public ExchangeQuestSo ExchangeQuestData {get; set; }

        public QuestExchangeEventArgs(ExchangeQuestSo questSo):base(questSo.Id)
        {
            ExchangeQuestData = questSo;
        }
    }
}