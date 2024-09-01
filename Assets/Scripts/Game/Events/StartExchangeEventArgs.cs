using System;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;

namespace Game.Quests
{
    public delegate void StartExchangeEvent(object sender, StartExchangeEventArgs eventArgs);
    
    public class StartExchangeEventArgs : EventArgs
    {
        public int ExchangeQuestId {get; set; }

        public StartExchangeEventArgs(int exchangeQuestId)
        {
            ExchangeQuestId = exchangeQuestId;
        }
    }
}