using System;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;

namespace Game.Quests
{
    public delegate void StartCheckpointEvent(object sender, StartCheckpointEventArgs eventArgs);
    
    public class StartCheckpointEventArgs : EventArgs
    {
        public int QuestId { get; set; }

        public StartCheckpointEventArgs(int questId)
        {
            QuestId = questId;
        }
    }
}