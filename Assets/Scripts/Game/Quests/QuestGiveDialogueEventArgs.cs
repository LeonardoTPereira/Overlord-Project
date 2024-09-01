using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;

namespace Game.Quests
{
    public class QuestGiveDialogueEventArgs : QuestElementEventArgs
    {
        public string NpcName {get; set; }

        public QuestGiveDialogueEventArgs(string npcName, int questId):base(questId)
        {
            NpcName = npcName;
        }
    }
}