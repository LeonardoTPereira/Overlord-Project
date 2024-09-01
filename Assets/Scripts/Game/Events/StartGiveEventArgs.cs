using System;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;

namespace Game.Quests
{
    public delegate void StartGiveEvent(object sender, StartGiveEventArgs eventArgs);
    
    public class StartGiveEventArgs : EventArgs
    {
        public int GiveQuestId {get; set; }

        public StartGiveEventArgs(int giveQuestId)
        {
            GiveQuestId = giveQuestId;
        }
    }
}