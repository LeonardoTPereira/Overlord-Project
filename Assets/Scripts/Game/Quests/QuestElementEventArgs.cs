using System;

namespace Game.Quests
{
    public delegate void QuestElementEvent(object sender, QuestElementEventArgs e);
    public abstract class QuestElementEventArgs : EventArgs
    {
    }
}