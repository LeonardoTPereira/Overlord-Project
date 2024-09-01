namespace Game.Quests
{
    public interface IQuestElement
    {
        public static event QuestElementEvent QuestElementEventHandler;
        public static event QuestElementEvent QuestCompletedEventHandler;

        public void OnQuestTaskResolved(object sender, QuestElementEventArgs args)
        {
            QuestElementEventHandler?.Invoke(sender, args);
        }
        
        public void OnQuestCompleted(object sender, QuestElementEventArgs args)
        {
            QuestCompletedEventHandler?.Invoke(sender, args);
        }
    }
}