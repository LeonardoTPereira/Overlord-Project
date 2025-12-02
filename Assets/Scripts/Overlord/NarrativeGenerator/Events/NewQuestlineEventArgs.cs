using Overlord.NarrativeGenerator.NPCs;
using Overlord.NarrativeGenerator.Quests;

namespace Overlord.NarrativeGenerator.Events
{
    public delegate void QuestLineCompletedEvent(object sender, NewQuestLineEventArgs e);
    public delegate void QuestLineOpenedEvent(object sender, NewQuestLineEventArgs e);
    public class NewQuestLineEventArgs
    {
        public QuestLine QuestLine {get; set; }

        public bool IsMainQuestLine => QuestLine.IsMainQuest;
        public NpcSo NpcInCharge => QuestLine.NpcInCharge;

        public NewQuestLineEventArgs(QuestLine questLine)
        {
            QuestLine = questLine;
        }
    }
}