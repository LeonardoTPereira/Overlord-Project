using System;

namespace Game.Quests
{
    public delegate void QuestElementEvent(object sender, QuestElementEventArgs e);
    public class QuestElementEventArgs : EventArgs
    {
        public int QuestId { get; set; }

        public QuestElementEventArgs(int questId)
        {
            QuestId = questId;
        }
    }
}