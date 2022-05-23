namespace Game.Quests
{
    public class QuestCountableElementEventArgs : QuestElementEventArgs
    {
        public int Amount { get; set; }
        public QuestCountableElementEventArgs(bool success, int amount)
        {
            Amount = amount;
        }
    }
}