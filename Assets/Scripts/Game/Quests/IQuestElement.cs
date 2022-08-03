namespace Game.Quests
{
    public interface IQuestElement
    {
        public static event QuestElementEvent QuestElementEventHandler;
        public int QuestId { get; set; }
        public void OnQuestTaskResolved(object sender, QuestElementEventArgs args)
        {
            QuestElementEventHandler?.Invoke(sender, args);
        }
    }
}