namespace Game.Quests
{
    public interface IQuestElement
    {
        public static event QuestElementEvent QuestElementEventHandler;
        public void OnQuestTaskResolved(object sender, QuestElementEventArgs args)
        {
            QuestElementEventHandler?.Invoke(sender, args);
        }
    }
}