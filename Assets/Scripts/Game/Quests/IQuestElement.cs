namespace Game.Quests
{
    public interface IQuestElement
    {
        public static event QuestElementEvent QuestElementEventHandler;
        protected void OnQuestTaskResolved(QuestElementEventArgs args)
        {
            QuestElementEventHandler?.Invoke(this, args);
        }
    }
}