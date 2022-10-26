using Game.Events;
using Game.LevelManager.DungeonManager;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.Quests.QuestGrammarTerminals;
using Game.NPCs;
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
            NpcController.ItemTradeEventHandler += TradeItems;
            NpcController.ItemGiveEventHandler += GiveItem;
        }
        
        protected void OnDisable()
        {
            TreasureController.TreasureCollectEventHandler -= AddItem;
            NpcController.ItemTradeEventHandler -= TradeItems;
            NpcController.ItemGiveEventHandler -= GiveItem;
        }

        private void Awake()
        {
            Inventory = new ItemAmountDictionary();
        }

        private void AddItem(object sender, TreasureCollectEventArgs eventArgs)
        {
            Inventory.AddItemWithId(eventArgs.Item, eventArgs.QuestId);
        }

        private void GiveItem(object sender, ItemGiveEventArgs eventArgs)
        {
            RemoveItem(eventArgs.GivenItem, eventArgs.QuestId);
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
            Inventory.AddItemWithId(eventArgs.Item, eventArgs.QuestId);
        }

        private void RemoveItem(ItemSo item, int id)
        {
            Inventory.RemoveItemWithId(item, id);
        }

    }
}