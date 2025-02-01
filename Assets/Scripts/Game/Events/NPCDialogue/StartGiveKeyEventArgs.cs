using System;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;

namespace Game.Quests
{
    public delegate void StartGiveKeyEvent(object sender, StartGiveKeyEventArgs eventArgs);
    
    public class StartGiveKeyEventArgs : EventArgs
    {
        public int GivedKey {get; set; }

        public StartGiveKeyEventArgs(int givedKey)
        {
            GivedKey = givedKey;
        }
    }
}