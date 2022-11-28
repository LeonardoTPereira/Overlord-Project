using Game.NPCs;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class GiveQuestData
    {
        public NpcSo NpcToReceive { get; set; }
        public ItemSo ItemToGive { get; set; }
        public int QuestId { get; set; }

        public GiveQuestData()
        {
            NpcToReceive = null;
            ItemToGive = null;
        }
        
        public GiveQuestData(NpcSo npcToReceive, ItemSo itemToGive, int questId)
        {
            NpcToReceive = npcToReceive;
            ItemToGive = itemToGive;
            QuestId = questId;
        }
        
        public GiveQuestData(GiveQuestData originalData)
        {
            NpcToReceive = originalData.NpcToReceive;
            ItemToGive = originalData.ItemToGive;
            QuestId = originalData.QuestId;
        }
    }
}