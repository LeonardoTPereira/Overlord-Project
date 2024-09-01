using Game.NarrativeGenerator.ItemRelatedNarrative;
using ScriptableObjects;

namespace Game.Events
{
    public delegate void ItemTradeEvent(object sender, ItemTradeEventArgs e);

    public class ItemTradeEventArgs : TreasureCollectEventArgs
    {
        public ItemAmountDictionary GivenItems { get; set; }

        public ItemTradeEventArgs(ItemAmountDictionary givenItems, ItemSo item, int questId): base(item, questId)
        {
            GivenItems = givenItems;
            Item = item;
            QuestId = questId;
        }
    }
}