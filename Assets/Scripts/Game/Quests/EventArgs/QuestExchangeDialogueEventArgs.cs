using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
using Overlord.NarrativeGenerator.Events;

namespace Game.Quests
{
    public class QuestExchangeDialogueEventArgs : QuestElementEventArgs
    {
        public string NpcName {get; set; }

        public QuestExchangeDialogueEventArgs(string npcName, int questId):base(questId)
        {
            NpcName = npcName;
        }
    }
}