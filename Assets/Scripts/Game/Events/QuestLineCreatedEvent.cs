using System;
using Game.NarrativeGenerator.Quests;

namespace Game.Events
{
    public delegate void QuestLineCreatedEvent(object sender, QuestLineCreatedEventArgs e);
    public class QuestLineCreatedEventArgs : EventArgs
    {
        public QuestLine Quests { get; set; }

        public QuestLineCreatedEventArgs(QuestLine quests)
        {
            Quests = quests;
        }
    }
}