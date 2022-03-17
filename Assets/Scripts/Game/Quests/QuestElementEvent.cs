using System;

namespace Game.Quests
{
    public delegate void QuestElementEvent(object sender, QuestElementEventArgs e);
    public class QuestElementEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public QuestElementEventArgs(bool success)
        {
            Success = success;
        }
    }
}