using Game.NarrativeGenerator.Quests;
using Game.NPCs;

namespace Game.Quests
{
    public delegate void QuestCompletedEvent(object sender, NewQuestEventArgs e);
    public delegate void QuestOpenedEvent(object sender, NewQuestEventArgs e);
    public class NewQuestEventArgs
    {
        public QuestSO Quest {get; set; }
        public NpcSo NpcInCharge { get; set; }

        public NewQuestEventArgs(QuestSO quest, NpcSo npc)
        {
            Quest = quest;
            NpcInCharge = npc;
        }
    }
}