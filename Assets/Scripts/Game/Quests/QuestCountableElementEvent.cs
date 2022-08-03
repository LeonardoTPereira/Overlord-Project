namespace Game.Quests
{
    public class QuestCountableElementEventArgs : QuestElementEventArgs
    {
        public int Amount { get; set; }
        public QuestCountableElementEventArgs(bool success, int amount, int questId): base(questId)
        {
            Amount = amount;
        }
    }
}