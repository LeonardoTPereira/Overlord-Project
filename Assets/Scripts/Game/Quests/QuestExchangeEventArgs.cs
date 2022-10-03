using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;

namespace Game.Quests
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