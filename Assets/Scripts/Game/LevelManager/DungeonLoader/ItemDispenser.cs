using Game.NarrativeGenerator.ItemRelatedNarrative;
using Unity.Mathematics;
using Util;

namespace Game.LevelManager.DungeonLoader
{
    public static class ItemDispenser
    {
        private static int _remainingItems;

        public static void DistributeItemsInDungeon(Map map, ItemsAmount items, int total)
        {
            var itemsInQuestByType = (ItemsAmount) items.Clone();
            _remainingItems = total;
            var totalTreasureRooms = map.NTreasureRooms;
            var itemsPerRoom = 0;
            if (totalTreasureRooms > 0)
            {
                itemsPerRoom = _remainingItems/totalTreasureRooms;
                _remainingItems %= totalTreasureRooms;
            }
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (dungeonPart.Value is DungeonRoom dungeonRoom && !dungeonRoom.IsStartRoom() && dungeonRoom.HasItemPreference)
                {
                    dungeonRoom.Items = SelectItemsForRoom(itemsInQuestByType, itemsPerRoom);
                }
            }

            if (_remainingItems <= 0) return;
            
            foreach (var dungeonPart in map.DungeonPartByCoordinates)
            {
                if (dungeonPart.Value is DungeonRoom dungeonRoom && !dungeonRoom.IsStartRoom())
                {
                    dungeonRoom.Items = SelectItemsForRoom(itemsInQuestByType, itemsPerRoom);
                }
            }
        }

        private static ItemsAmount SelectItemsForRoom(ItemsAmount itemsInQuestByType, int itemsPerRoom)
        {
            var itemsByType = new ItemsAmount();
            var selectedItems = 0;
            var itemsInRoom = itemsPerRoom + _remainingItems;
            _remainingItems = 0;
            while (selectedItems < itemsInRoom)
            {
                var selectedType = itemsInQuestByType.GetRandom();
                var maxPossibleNewItems = math.min(selectedType.Value.QuestIds.Count, itemsInRoom - selectedItems);
                var newItems = RandomSingleton.GetInstance().Random.Next(1, maxPossibleNewItems);
                itemsByType.AddNItemsFromType(selectedType, newItems);
                itemsInQuestByType.RemoveCurrentTypeIfEmpty(selectedType.Key);
                selectedItems += newItems;
            }
            return itemsByType;
        }
    }
}