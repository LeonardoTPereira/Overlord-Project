using Overlord.NarrativeGenerator.Quests;

namespace Game.Quests
{
    public class QuestCheckPointEventArgs : QuestElementEventArgs
    {
        public QuestSo QuestData {get; set; }

        public QuestCheckPointEventArgs(QuestSo questSo):base(questSo.Id)
        {
            QuestData = questSo;
        }
    }
}