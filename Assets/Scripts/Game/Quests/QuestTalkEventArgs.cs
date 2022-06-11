using Game.NPCs;

namespace Game.Quests
{
    public class QuestTalkEventArgs : QuestElementEventArgs
    {
        public NpcSo Npc {get; set; }

        public QuestTalkEventArgs(NpcSo npc)
        {
            Npc = npc;
        }
    }
}