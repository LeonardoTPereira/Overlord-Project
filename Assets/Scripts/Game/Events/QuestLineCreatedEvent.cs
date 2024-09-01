using System;
using Game.NarrativeGenerator.Quests;

namespace Game.Events
{
    public delegate void QuestLineCreatedEvent(object sender, QuestLineCreatedEventArgs e);
    public class QuestLineCreatedEventArgs : EventArgs
    {
        public QuestLineList QuestLines { get; set; }

        public QuestLineCreatedEventArgs(QuestLineList questLines)
        {
            QuestLines = questLines;
        }
    }
}