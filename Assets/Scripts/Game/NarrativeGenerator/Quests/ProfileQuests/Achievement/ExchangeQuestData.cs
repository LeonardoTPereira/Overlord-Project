using Game.NarrativeGenerator.ItemRelatedNarrative;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public struct ExchangeQuestData
    {
        public ItemAmountDictionary CopyOfItemsToTrade { get; set; }
        public ItemSo ReceivedItem { get; set; }
        public int QuestId { get; set; }

        public ExchangeQuestData(ItemAmountDictionary copyOfItemsToTrade, ItemSo receivedItem, int questId)
        {
            CopyOfItemsToTrade = copyOfItemsToTrade;
            ReceivedItem = receivedItem;
            QuestId = questId;
        }
        
        public ExchangeQuestData(ExchangeQuestData copiedData)
        {
            CopyOfItemsToTrade = copiedData.CopyOfItemsToTrade;
            ReceivedItem = copiedData.ReceivedItem;
            QuestId = copiedData.QuestId;
        }
    }
}