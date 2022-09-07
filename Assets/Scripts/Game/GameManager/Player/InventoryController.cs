using Game.Events;
using Game.LevelManager.DungeonManager;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using ScriptableObjects;
using UnityEngine;

namespace Game.GameManager.Player
{
    public class InventoryController : MonoBehaviour
    {
        public ItemAmountDictionary Inventory { get; set; }

        protected void OnEnable()
        {
            TreasureController.TreasureCollectEventHandler += AddItem;
            ExchangeQuestSo.ItemTradeEventHandler += TradeItems;
        }
        
        protected void OnDisable()
        {
            TreasureController.TreasureCollectEventHandler -= AddItem;
            ExchangeQuestSo.ItemTradeEventHandler -= TradeItems;
        }

        private void AddItem(object sender, TreasureCollectEventArgs eventArgs)
        {
            Inventory.AddItemWithId(eventArgs.Item, eventArgs.QuestId);
        }

        private void GiveItem(object sender, TreasureCollectEventArgs eventArgs)
        {
            RemoveItem(eventArgs.Item, eventArgs.QuestId);
        }
        
        private void TradeItems(object sender, ItemTradeEventArgs eventArgs)
        {
            foreach (var items in eventArgs.GivenItems)
            {
                foreach (var id in items.Value.QuestIds)
                {
                    RemoveItem(items.Key, id);
                }
            }
        }

        private void RemoveItem(ItemSo item, int id)
        {
            Inventory.RemoveItemWithId(item, id);
        }

    }
}