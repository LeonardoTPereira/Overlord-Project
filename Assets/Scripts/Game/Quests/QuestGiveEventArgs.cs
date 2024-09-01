using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;

namespace Game.Quests
{
    public class QuestGiveEventArgs : QuestElementEventArgs
    {
        public GiveQuestSo GiveQuestData {get; set; }

        public QuestGiveEventArgs(GiveQuestSo questSo):base(questSo.Id)
        {
            GiveQuestData = questSo;
        }
    }
}