using Game.NPCs;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GiveQuestData
    {
        public NpcSo NpcToReceive { get; set; }
        public ItemSo ItemToGive { get; set; }

        public GiveQuestData()
        {
            NpcToReceive = null;
            ItemToGive = null;
        }
        
        public GiveQuestData(NpcSo npcToReceive, ItemSo itemToGive)
        {
            NpcToReceive = npcToReceive;
            ItemToGive = itemToGive;
        }
    }
}